﻿using MazdaInventory.ConnectionManager;
using MazdaInventory.Model;
using MazdaInventory.SQLite;
using MazdaInventory.SQLLite;
using System;
using System.Collections.Generic;
using System.Net;

namespace MazdaInventory.Commons
{
    class MainController :INetworkCommunicationListener
    {
        Boolean lReturnValue;
        IConnectionCallbacks lIConnectionCallbacks;

        public MainController()
        {
            GC.Collect();
        }

       public Boolean LoginRequest(String UserName, String Password, IConnectionCallbacks callbackHandler, string requestID)
		{
            Console.WriteLine("LoginRequest start");

            lIConnectionCallbacks = callbackHandler;

			Boolean lIsNetworkAvailable = Utilities.CheckInternetConnection();
			if (lIsNetworkAvailable == false)
			{
                Console.WriteLine("LoginRequest lIsNetworkAvailable start");
                Dictionary<String, Object> lErrorDictionary = new Dictionary<String, Object>();
				lErrorDictionary[Defines.KeyConnectionErrorType] = ConnectionErrorType.EConnectionError_NoNetwork;
				lErrorDictionary[Defines.KeyConnectionResponseMessage] = Defines.NetworkUnavailable;
				lIConnectionCallbacks.ConnectionFailedWithError(lErrorDictionary, requestID);

				return lReturnValue;
			}

			if (callbackHandler == null)
			{
                Console.WriteLine("LoginRequest callbackHandler start");
                lReturnValue = false;
				return lReturnValue;
			}

			if (String.IsNullOrEmpty(UserName))
			{
                Console.WriteLine("LoginRequest IsNullOrEmpty start");
                Dictionary<String, Object> lErrorDictionary = new Dictionary<String, Object>();
				lErrorDictionary[Defines.KeyConnectionErrorType] = ConnectionErrorType.EConnectionError_LocalValidationFailed_NilOrBlankUserName;
				lErrorDictionary[Defines.KeyConnectionResponseMessage] = "Local Validation Failed Nil Or Blank UserName";
				lIConnectionCallbacks.ConnectionFailedWithError(lErrorDictionary, requestID);

				return lReturnValue;
			}

			if (String.IsNullOrEmpty(Password))
			{
                Console.WriteLine("LoginRequest IsNullOrEmpty start");
                Dictionary<String, Object> lErrorDictionary = new Dictionary<String, Object>();
				lErrorDictionary[Defines.KeyConnectionErrorType] = ConnectionErrorType.EConnectionError_LocalValidationFailed_NilOrBlankPassword;
				lErrorDictionary[Defines.KeyConnectionResponseMessage] = "EConnectionError_LocalValidationFailed_NilOrBlankPassword";
				lIConnectionCallbacks.ConnectionFailedWithError(lErrorDictionary, requestID);

				return lReturnValue;
			}

			HttpConnectionManager lHttpConnectionManager = new HttpConnectionManager(this, requestID);
			String userNameAndPassword = UserName + "<@>" + Password;
            var _ = lHttpConnectionManager.ExecuteWebRequestAsync(Utilities.getLoginURLAccordingToEnvironment(), userNameAndPassword);

			return false;
		}

		/// Notifies the HTTPRespons.
		/// Callback for HttpConnectionManager
		public void notifyHTTPRespons(object result, string RequestID)
		{
			if (RequestID == Defines.LoginRequestServerHit) // Specific For Login as Per Previous Prod v1.0
			{
				Dictionary<String, Object> temp = (Dictionary<String, Object>)result;
                
                //HttpStatusCode lHTTPStatusCode = (HttpStatusCode)temp.GetValueOrDefault(Defines.KeyConnectionResponseHTTPStatusCode);
                HttpStatusCode lHTTPStatusCode = (HttpStatusCode)temp[Defines.KeyConnectionResponseHTTPStatusCode];
                String lContent = (String)temp[Defines.KeyConnectionResponseContentString];

				bool IsError = (bool)temp[Defines.IsError];
				bool IsExp = (bool)temp[Defines.IsException];

				bool lIsLoginSuccessFull = false;
				String lConnectionResponseMessage = Defines.ValueServerErrorConnectionResponseMessage;
				ConnectionErrorType lConnectionErrorType = ConnectionErrorType.EConnectionError_ServerSide_Others;

				{
					if (String.IsNullOrWhiteSpace(lContent))
					{
						lConnectionResponseMessage = "Response contained empty body...";
						lConnectionErrorType = ConnectionErrorType.EConnectionError_ServerSide_EmptyResponse;

						ConnectionErrorType lOriginalConnectionErrorType = (ConnectionErrorType)temp[Defines.KeyConnectionErrorType];
						if (lOriginalConnectionErrorType == ConnectionErrorType.EConnectionError_ServerSide_HTTPStatusCodeIsNot200)
						{
							lConnectionResponseMessage = Defines.ValueHTTPStatusCodeIsNot200ConnectionResponseMessage;
							lConnectionErrorType = lOriginalConnectionErrorType;
						}
					}
					else if (lContent.Contains("login was successful."))
					{
						lConnectionResponseMessage = "login was successful.";
						lConnectionErrorType = ConnectionErrorType.EConnectionError_None;

						lIsLoginSuccessFull = true;
					}
					else if (lContent.Contains("Authentication failed"))
					{
						lConnectionResponseMessage = "Authentication failed";
						lConnectionErrorType = ConnectionErrorType.EConnectionError_ServerSide_AuthenticationFailed;
					}
					else if (lContent.Contains("To maintain your login session"))
					{
						lConnectionResponseMessage = Defines.ValueSessionTimeoutConnectionResponseMessage;
						lConnectionErrorType = ConnectionErrorType.EConnectionError_ServerSide_SessionTimeout;
					}

					Dictionary<String, Object> lDictionary = new Dictionary<String, Object>();
					lDictionary[Defines.KeyConnectionResponseHTTPStatusCode] = lHTTPStatusCode;
					lDictionary[Defines.KeyConnectionResponseMessage] = lConnectionResponseMessage;
					if (lIsLoginSuccessFull)
					{
						lDictionary[Defines.KeyConnectionResponseContentString] = lContent;

						Console.WriteLine(lContent);

						lDictionary[Defines.KeyConnectionResponseHeaders] = temp[Defines.KeyConnectionResponseHeaders];
						lDictionary[Defines.KeyLoginCookies] = temp[Defines.KeyLoginCookies];
						lIConnectionCallbacks.ConnectionWasSuccessFullWithResult(lDictionary, RequestID);
					}
					else
					{
						lDictionary[Defines.KeyConnectionErrorType] = lConnectionErrorType;
						lIConnectionCallbacks.ConnectionFailedWithError(lDictionary, RequestID);
					}
				}
			}
            else if (RequestID == Defines.DealerRequestServerHit) // Specific For Dealer as Per Previous Prod v1.0
            {
                Dictionary<String, Object> lDictionary = (Dictionary<String, Object>)result;

                Dealer lDealer = (Dealer)lDictionary[Defines.KeyConnectionResponseContentObject];
                ConnectionErrorType lOriginalConnectionErrorType = (ConnectionErrorType)lDictionary[Defines.KeyConnectionErrorType];
                //String lContent = (String)lDictionary[Defines.KeyConnectionResponseContentString];

                bool IsError = (bool)lDictionary[Defines.IsError];
                bool IsExp = (bool)lDictionary[Defines.IsException];

                if (IsExp || IsError)
                {
                }
                else
                {
                    String lConnectionResponseMessage = "";
                    ConnectionErrorType lConnectionErrorType = ConnectionErrorType.EConnectionError_ServerSide_Others;

                    Boolean lIsSuccessFull = true;

                    if (lOriginalConnectionErrorType == ConnectionErrorType.EConnectionError_ServerSide_HTTPStatusCodeIsNot200)
                    {
                        lIsSuccessFull = false;

                        lConnectionResponseMessage = "HTTP StatusCode Is Not 200";
                        lConnectionErrorType = lOriginalConnectionErrorType;
                    }

                    Dictionary<String, Object> lReturnDictionary = new Dictionary<String, Object>();
                    lReturnDictionary[Defines.KeyConnectionResponseHTTPStatusCode] = lDictionary[Defines.KeyConnectionResponseHTTPStatusCode];
                    lReturnDictionary[Defines.KeyConnectionResponseMessage] = lConnectionResponseMessage;

                    if (lIsSuccessFull)
                    {
                        lReturnDictionary[Defines.KeyConnectionResponseContentObject] = lDealer;
                        lIConnectionCallbacks.ConnectionWasSuccessFullWithResult(lReturnDictionary, RequestID);
                    }
                    else
                    {
                        lReturnDictionary[Defines.KeyConnectionErrorType] = lConnectionErrorType;
                        lIConnectionCallbacks.ConnectionFailedWithError(lReturnDictionary, RequestID);
                    }
                }
            }
            else // For others Apis (Regions and Carlines)
			{
				Dictionary<String, Object> lResultDictionary = (Dictionary<String, Object>)result;

				String lContent = (String)lResultDictionary[Defines.KeyConnectionResponseContentString];

				bool IsError = (bool)lResultDictionary[Defines.IsError];
				bool IsExp = (bool)lResultDictionary[Defines.IsException];
				String lConnectionResponseMessage = Defines.ValueServerErrorConnectionResponseMessage;
				bool lIsDataSuccessFul = false;
				ConnectionErrorType lConnectionErrorType = ConnectionErrorType.EConnectionError_ServerSide_Others;
				if (IsExp || IsError)
				{
					Dictionary<String, Object> lErrorDictionary = (System.Collections.Generic.Dictionary<string, object>)result;
					lIConnectionCallbacks.ConnectionFailedWithError(lErrorDictionary, RequestID);
				}
				else
				{
					// Hardcoded checks are as per previous Prod build v1.0
					if (String.IsNullOrWhiteSpace(lContent))
					{
						lConnectionResponseMessage = Defines.ValueServerErrorConnectionResponseMessage;
						lConnectionErrorType = ConnectionErrorType.EConnectionError_ServerSide_EmptyResponse;
						if (lResultDictionary.ContainsKey(Defines.KeyConnectionErrorType))
						{
							ConnectionErrorType lOriginalConnectionErrorType = (ConnectionErrorType)lResultDictionary[Defines.KeyConnectionErrorType];
							if (lOriginalConnectionErrorType == ConnectionErrorType.EConnectionError_ServerSide_HTTPStatusCodeIsNot200)
							{
								lConnectionResponseMessage = Defines.ValueHTTPStatusCodeIsNot200ConnectionResponseMessage;
								lConnectionErrorType = lOriginalConnectionErrorType;
							}
						}
						else
						{
							lConnectionResponseMessage = Defines.ValueHTTPStatusCodeIsNot200ConnectionResponseMessage;
							lConnectionErrorType = ConnectionErrorType.EConnectionError_ServerSide_EmptyResponse;
						}
					}
					else if (lContent.Contains("location data was successfully fetched."))
					{
						lConnectionResponseMessage = "location data was successfully fetched.";
						lConnectionErrorType = ConnectionErrorType.EConnectionError_None;

						String cookie = (String)lResultDictionary[Defines.KeyLocationDataCookies];
					}
					else if (lContent.Contains("Authentication failed"))
					{
						lConnectionResponseMessage = Defines.ValueServerErrorConnectionResponseMessage;
						lConnectionErrorType = ConnectionErrorType.EConnectionError_ServerSide_AuthenticationFailed;
					}
					else if (lContent.Contains("To maintain your locationData session"))
					{
						lConnectionResponseMessage = Defines.ValueSessionTimeoutConnectionResponseMessage;
						lConnectionErrorType = ConnectionErrorType.EConnectionError_ServerSide_SessionTimeout;
					}
					else
					{
						if (Utilities.isValidJSON(lContent)) // For checking reposne is vaild JSON or not
						{
							//   SQLiteRowData rowData = new SQLiteRowData(RequestID, lContent);
							//  SQLiteManager.SharedInstance().SaveItem(rowData);
							//  if (SQLiteManager.SharedInstance().GetItemForKey(RequestID) != null)
							{
								Console.WriteLine("lcontent saved in SQLite");
								Console.WriteLine(RequestID + "---" + lContent);
								lIsDataSuccessFul = true;
							}
						}
						else
						{
							ExcepetionMainScreen(RequestID);
							return;
						}
					}
					Dictionary<String, Object> lDictionary = new Dictionary<string, object>();
					lDictionary.Add(RequestID, lContent);
                    string serverResponse = Utilities.GetTimestamp(DateTime.Now);
                    string appEnvironmentValue = Utilities.getEnvironmentValue();
                    
					if (lIsDataSuccessFul)
					{
						lIConnectionCallbacks.ConnectionWasSuccessFullWithResult(lDictionary, RequestID); // Connection callback successful
					}
					else
					{
						
						lDictionary[Defines.KeyConnectionResponseMessage] = lConnectionResponseMessage; // Connection callback failure
						lIConnectionCallbacks.ConnectionFailedWithError(lDictionary, RequestID);
					}

                }
            }
		}

		/// Notifies the HTTPError.
		public void notifyHTTPError(object result, String RequestID)
		{
			try
			{
				// GC.Collect();

				Dictionary<String, Object> lResultDictionary = (Dictionary<String, Object>)result;
				String lContent = (String)lResultDictionary[Defines.KeyConnectionResponseContentString];

				bool IsError = (bool)lResultDictionary[Defines.IsError];
				bool IsExp = (bool)lResultDictionary[Defines.IsException];
				String lConnectionResponseMessage = Defines.ValueServerErrorConnectionResponseMessage;
				//bool lIsLocationDataSuccessFul = false;
				//ConnectionErrorType lConnectionErrorType = ConnectionErrorType.EConnectionError_ServerSide_Others;

				Dictionary<String, Object> lErrorDictionary = (System.Collections.Generic.Dictionary<string, object>)result;

				lIConnectionCallbacks.ConnectionFailedWithError(lErrorDictionary, RequestID);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				ExcepetionMainScreen(RequestID);
			}
		}

		/// Excepetion handled in mainscreen.
		public void ExcepetionMainScreen(String RequestID)
		{
			Dictionary<String, Object> lResultDictionary = new Dictionary<String, Object>();

			String lContent = (String)lResultDictionary[Defines.KeyConnectionResponseContentString];
			lResultDictionary[Defines.KeyConnectionResponseMessage] = Defines.ValueServerErrorConnectionResponseMessage;
			lResultDictionary[Defines.KeyConnectionResponseMessage] = lResultDictionary[Defines.KeyConnectionResponseMessage];
			lIConnectionCallbacks.ConnectionFailedWithError(lResultDictionary, RequestID);
		}

        public void SetDealerInformation(Dealer dealer, String reqId)
        {
            String lConnectionResponseMessage = "";
            
            Dictionary<String, Object> lReturnDictionary = new Dictionary<String, Object>();
            lReturnDictionary[Defines.KeyConnectionResponseHTTPStatusCode] = "200"; ;
            lReturnDictionary[Defines.KeyConnectionResponseMessage] = lConnectionResponseMessage;
            lReturnDictionary[Defines.KeyConnectionResponseContentObject] = dealer;
            lIConnectionCallbacks.ConnectionWasSuccessFullWithResult(lReturnDictionary, reqId);
        }

        public Boolean GetDealerData(String searchType, String param1, String param2, IConnectionCallbacks callbackHandler, string requestID)
        {
            lIConnectionCallbacks = callbackHandler;
            SQLiteRowData checkIfDealerExist = new SQLiteRowData();

            checkIfDealerExist = SQLiteManager.SharedInstance().GetItemForKey("dealer").Result;

            if (checkIfDealerExist != null)
            {
                string dealerDataSqlite = checkIfDealerExist.Value;
                Dealer lDealer = Utilities.SaveDealerData(dealerDataSqlite);
                SetDealerInformation(lDealer, requestID);
            }
            else
            {
                Boolean lIsNetworkAvailable = Utilities.CheckInternetConnection();
                if (lIsNetworkAvailable == false)
                {
                    Dictionary<String, Object> lErrorDictionary = new Dictionary<String, Object>();
                    lErrorDictionary[Defines.KeyConnectionErrorType] = ConnectionErrorType.EConnectionError_NoNetwork;
                    lErrorDictionary[Defines.KeyConnectionResponseMessage] = "Network Unavailable";
                    callbackHandler.ConnectionFailedWithError(lErrorDictionary, requestID);

                    return lReturnValue;
                }

                if (callbackHandler == null)
                {
                    lReturnValue = false;
                    return lReturnValue;
                }
                HttpConnectionManager lHttpConnectionManager = new HttpConnectionManager(this, requestID);
				string baseUrl = "";
				switch (searchType)
				{
					case "zip":
						baseUrl = Utilities.getDealersByZip() + "/" + param1 + "/" + param2 + "/";
						break;
					case "name":
						baseUrl = Utilities.getDealersByName() + "/" + param1 + "/";
						break;
					case "corp":
						baseUrl = Utilities.getDealersByRegion() + "/" + param1 + "/" + param2 + "/";
						break;
					default:
						break;
				}
				var _ = lHttpConnectionManager.ExecuteWebRequestAsync(baseUrl, "");
            }
            return true;
        }
    }
}
