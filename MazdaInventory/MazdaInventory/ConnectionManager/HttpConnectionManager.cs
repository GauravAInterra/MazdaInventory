using MazdaInventory.Commons;
using MazdaInventory.Model;
using MazdaInventory.SQLite;
using MazdaInventory.SQLLite;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MazdaInventory.ConnectionManager
{
    public class HttpConnectionManager
    {
        ///static string variable for cookie.
        ///  public static String sCookie = null;
        public int TestPublic;
        protected int TestProtect;

        ///string variable for Server Response.
        public string ServerResponse = "";
        ///bool variable for error.
        public Boolean IsError;
        ///bool variable for exception.
        public Boolean IsExcepetion;
        /// string variable for cookie.
        public String cookies;
        public String requestIDforhit;

        //Dealer lDealer = null;

        INetworkCommunicationListener objNetworkCommunicationListener;

        /// Initializes a new instance of the <see cref="T:MazdaMBA.ConnectionManager.HttpConnectionManager"/> class.
        public HttpConnectionManager(INetworkCommunicationListener obj, String RequestID)
        {

            this.addHttpLIstener(obj);
            this.requestIDforhit = RequestID;
        }

        /// Adds the http Listener.
        public void addHttpLIstener(INetworkCommunicationListener obj)
        {
            this.objNetworkCommunicationListener = obj;
        }

        public async Task<bool> ExecuteWebRequestAsync(string baseUrl, String controllerUrl)
        {
            try
            {
                Console.WriteLine("ExecuteWebRequestAsync start");
                var baseAddress = new Uri(baseUrl);
                var restClient = new RestClient(baseUrl);
                restClient.CookieContainer = new CookieContainer();
                var ServerConnectionRequestParm = new RestRequest(Method.POST);

                if (requestIDforhit == Defines.LoginRequestServerHit)
                {
                    String[] tempString = controllerUrl.Split(new[] { "<@>" }, StringSplitOptions.None);
                    ServerConnectionRequestParm.Timeout = Defines.Timeout;
                    ServerConnectionRequestParm.AddParameter("username", tempString[0]);
                    ServerConnectionRequestParm.AddParameter("password", tempString[1]);
                    ServerConnectionRequestParm.AddParameter("login-form-type", "pwd");
                }
                else
                {
                    string tempCooki = Utilities.getCookies();
                    if (tempCooki != null && tempCooki != "")
                    {
                        ServerConnectionRequestParm.AddCookie("PD-S-SESSION-ID", tempCooki);
                    }
                    ServerConnectionRequestParm.Resource = controllerUrl;
                }
                string serverRequest = Utilities.GetTimestamp(DateTime.Now);
                string serverRequestTime = serverRequest + "." + DateTime.Now.Millisecond;
                string appEnvironmentValue = Utilities.getEnvironmentValue();

                Task<IRestResponse> task = restClient.ExecuteTaskAsync(ServerConnectionRequestParm);
                var serverResponse = await task;
                Console.WriteLine("This is server response : \n" + serverResponse.ToString());

                String Content = serverResponse.Content;
                Console.WriteLine("This is server response : \n" + Content);
                //HttpStatusCode code = serverResponse.StatusCode;
                string serverResponseValueTime = Utilities.GetTimestamp(DateTime.Now);
                string serverResponseTime = serverResponseValueTime + "." + DateTime.Now.Millisecond;
                HttpStatusCode lStatus = serverResponse.StatusCode;
                if (serverResponse.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("This is server response1");
                    if (requestIDforhit == Defines.LoginRequestServerHit)
                    {
                        ServerResponse = Content;
                        Console.WriteLine("This is server response1.1"+ServerResponse);
                        IsError = false;
                        IsExcepetion = false;
                        string cookieValue = null;


                        if (!(Content.IndexOf("To maintain your login session", StringComparison.CurrentCultureIgnoreCase) != -1))
                        {
                            if (serverResponse.Headers != null)
                            {
                                var headerList = serverResponse.Headers.ToList().Find(x => x.Name == "Set-Cookie");

                                if (headerList != null)
                                {
                                    cookieValue = headerList.Value.ToString();
                                }
                                else
                                {
                                    for (int i = 0; i < serverResponse.Headers.Count(); i++)
                                    {
                                        string cookieTest = serverResponse.Headers.ElementAt(i).Value.ToString();
                                        if (cookieTest.Contains("PD-S-SESSION-ID"))
                                        {
                                            cookieValue = cookieTest;
                                        }
                                    }
                                }
                                this.cookies = "Set-Cookie=" + cookieValue;
                                cookies = cookies.Substring(0, (cookies.Length - (cookies.Length - cookies.IndexOf(";", StringComparison.CurrentCulture))));
                                cookies = cookies.Substring(cookies.IndexOf("=", StringComparison.CurrentCulture) + 1, (cookies.Length - (cookies.IndexOf("=", StringComparison.CurrentCulture) + 1)));
                            }
                        }
                    }
                    
                    if (Content.IndexOf("To maintain your login session", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        if (requestIDforhit == Defines.LoginRequestServerHit)
                        {
                            Dictionary<String, Object> lSuccessDictionary = new Dictionary<String, Object>();
                            lSuccessDictionary[Defines.KeyConnectionResponseHTTPStatusCode] = lStatus;
                            lSuccessDictionary[Defines.KeyConnectionResponseContentString] = Content;
                            lSuccessDictionary[Defines.IsError] = this.IsError;
                            lSuccessDictionary[Defines.IsException] = this.IsExcepetion;

                            lSuccessDictionary[Defines.KeyLoginCookies] = this.cookies;
                            objNetworkCommunicationListener.notifyHTTPRespons(lSuccessDictionary, requestIDforhit);
                        }
                        else
                        {
                            Console.WriteLine("Maintaining Login session");
                            Dictionary<String, Object> lErrorDictionary = new Dictionary<String, Object>();
                            lErrorDictionary[Defines.KeyConnectionResponseHTTPStatusCode] = serverResponse.StatusCode;
                            lErrorDictionary[Defines.KeyConnectionResponseContentString] = Content;
                            lErrorDictionary[Defines.KeyConnectionResponseMessage] = Defines.ValueSessionTimeoutConnectionResponseMessage;
                            lErrorDictionary[Defines.IsError] = true;
                            lErrorDictionary[Defines.IsException] = false;
                            objNetworkCommunicationListener.notifyHTTPError(lErrorDictionary, requestIDforhit);
                        }
                    }
                    else
                    {
                        Console.WriteLine("This is server response2");
                        Dictionary<String, Object> lSuccessDictionary = new Dictionary<String, Object>();
                        lSuccessDictionary[Defines.KeyConnectionResponseHTTPStatusCode] = serverResponse.StatusCode;
                        lSuccessDictionary[Defines.KeyConnectionResponseContentString] = Content;
                        lSuccessDictionary[Defines.KeyConnectionResponseHeaders] = serverResponse.Headers;
                        lSuccessDictionary[Defines.IsError] = false;
                        lSuccessDictionary[Defines.IsException] = false;

                        lSuccessDictionary[Defines.KeyConnectionErrorType] = ConnectionErrorType.EConnectionError_None;
                        if (requestIDforhit == Defines.LoginRequestServerHit)
                            lSuccessDictionary[Defines.KeyLoginCookies] = this.cookies;
                        objNetworkCommunicationListener.notifyHTTPRespons(lSuccessDictionary, requestIDforhit);
                    }
                }
                else
                {
                    Dictionary<String, Object> lErrorDictionary = new Dictionary<String, Object>();
                    lErrorDictionary[Defines.KeyConnectionResponseHTTPStatusCode] = serverResponse.StatusCode;
                    lErrorDictionary[Defines.KeyConnectionErrorType] = ConnectionErrorType.EConnectionError_ServerSide_HTTPStatusCodeIsNot200;
                    lErrorDictionary[Defines.KeyConnectionResponseMessage] = Defines.ValueHTTPStatusCodeIsNot200ConnectionResponseMessage;
                    lErrorDictionary[Defines.IsError] = true;
                    lErrorDictionary[Defines.IsException] = false;

                    objNetworkCommunicationListener.notifyHTTPError(lErrorDictionary, requestIDforhit);
                }

                ServerConnectionRequestParm = null;
            }
            catch (Exception e)
            {
                Dictionary<String, Object> lErrorDictionary = new Dictionary<String, Object>();
                lErrorDictionary[Defines.IsError] = false;
                lErrorDictionary[Defines.IsException] = true;

                if (e.Message.Contains("Invalid URI"))
                {
                    lErrorDictionary[Defines.KeyConnectionErrorType] = ConnectionErrorType.EConnectionError_URL_BadRequest;
                    lErrorDictionary[Defines.KeyConnectionResponseMessage] = Defines.ValueInvalidURLConnectionResponseMessage;
                }
                else if (e.Message.Contains("A task was canceled"))
                {
                    lErrorDictionary[Defines.KeyConnectionErrorType] = ConnectionErrorType.EConnectionError_URL_Timeout;
                    lErrorDictionary[Defines.KeyConnectionResponseMessage] = Defines.ValueConnectionTimeoutConnectionResponseMessage;
                }
                else
                {
                    lErrorDictionary[Defines.KeyConnectionErrorType] = ConnectionErrorType.EConnectionError_None;
                    if (e is WebException)
                        lErrorDictionary[Defines.KeyConnectionResponseMessage] = Defines.NetworkUnavailable;
                    else
                        lErrorDictionary[Defines.KeyConnectionResponseMessage] = Defines.ValueServerErrorConnectionResponseMessage;
                }

                objNetworkCommunicationListener.notifyHTTPError(lErrorDictionary, requestIDforhit);
            }
            return true;
        }
    }
}
