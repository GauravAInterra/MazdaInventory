using MazdaInventory.Model;
using MazdaInventory.SQLite;
using MazdaInventory.SQLLite;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MazdaInventory.Commons
{
    class Utilities
    {

        public static Dealer DealerValue;
        public static Dealer tempDealerValue = null;
        public static CookieContainer CookieContainer = new CookieContainer();
        public static string getCookies()
        {
            string cookies = "";
            LoginModel tempLoginModel = SQLiteManager.SharedInstance().GetUserPreferences();
            if (tempLoginModel != null)
            {
                cookies = tempLoginModel.Cookies;
            }
            return cookies;
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static bool CheckInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
		/// Gets the environment value.
		/// </summary>
		/// <returns>The environment value.</returns>
		public static string getEnvironmentValue()
        {
            string environmentValue = "QA";
            SQLiteRowData checkForQAEnvironmentActivated = SQLiteManager.SharedInstance().GetItemForKey("EnvironmentValue").Result;

            if (checkForQAEnvironmentActivated != null)
            {
                environmentValue = checkForQAEnvironmentActivated.Value;
            }

            return environmentValue;
        }

        public static string getLoginURLAccordingToEnvironment()
        {
            string environmentValue = getEnvironmentValue();
            string retValueStr = "";
            switch (environmentValue)
            {
                case "QA":
                    retValueStr = "https://portaltest.mazdausa.com/pkmslogin.form";
                    break;
                case "TEST":
                    retValueStr = "https://portaltest.mazdausa.com/pkmslogin.form";
                    break;
                case "PROD":
                    retValueStr = "https://portal.mazdausa.com/pkmslogin.form";
                    break;
            }

            Console.WriteLine("\n" + retValueStr + "\n");

            return retValueStr;
        }

        public static bool IsLoginUserDealer()
        {
            bool isDealer = false;
            return isDealer;
        }

        public static bool isValidJSON(string json)
        {
            bool isValid = false;
            try
            {
                JToken token = JObject.Parse(json);
                var data = token["output"];

                if (data[0][0].ToString().ToLower() == "s01" && data[1] != null && data[1].ToString() != "null")
                    isValid = true;
                else
                    isValid = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Utilities isValidJSON " + ex.Message);
                isValid = false;
            }
            return isValid;
        }

        public static void saveEnvironment(string enviromentValue)
        {
            SQLite.SQLiteRowData data = new SQLite.SQLiteRowData();
            data = new SQLiteRowData("EnvironmentValue", enviromentValue);
            SQLiteManager.SharedInstance().SaveItem(data);
        }

        public static SQLiteRowData getTimePeriod()
        {
            SQLiteRowData dataValueTime = new SQLite.SQLiteRowData();
            dataValueTime = SQLiteManager.SharedInstance().GetItemForKey("SelectedSettingTimePeriod").Result;


            return dataValueTime;
        }

        public static void saveDefaultView(string selectedDefaultView)
        {
            if (selectedDefaultView != null)
            {
                SQLite.SQLiteRowData data = new SQLite.SQLiteRowData();
                data = new SQLiteRowData("SelectedSettingDefaultView", selectedDefaultView);
                SQLiteManager.SharedInstance().SaveItem(data);
            }
        }

        public static SQLiteRowData getDefaultView()
        {
            SQLite.SQLiteRowData dafaultView = new SQLite.SQLiteRowData();
            dafaultView = SQLiteManager.SharedInstance().GetItemForKey("SelectedSettingDefaultView").Result;


            return dafaultView;
        }

        public static void deleteAllDataFromSQlite()
        {
            SQLiteManager.SharedInstance().DeleteAllItems();
        }

        public static void saveTimePeriod(string selectedTimePeriod)
        {
            if (selectedTimePeriod != null)
            {
                SQLite.SQLiteRowData data = new SQLite.SQLiteRowData();
                data = new SQLiteRowData("SelectedSettingTimePeriod", selectedTimePeriod);
                SQLiteManager.SharedInstance().SaveItem(data);
            }
        }

        public static string getDealersByZip()
        {
            string environmentValue = getEnvironmentValue();
            string retValueStr = "";
            switch (environmentValue)
            {
                case "QA":
                    retValueStr = "https://portaltest.mazdausa.com/m371/webservices/mx/dealerLocateByZip/";
                    break;
                case "TEST":
                    retValueStr = "https://portaltest.mazdausa.com/m371/webservices/mx/dealerLocateByZip/";
                    break;
                case "PROD":
                    retValueStr = "https://portal.mazdausa.com/m174/webservices/mx/dealerLocateByZip/";
                    break;
                default:
                    break;
            }

            Console.WriteLine("\n" + retValueStr + "\n");
            return retValueStr;
        }

        public static string getDealersByName()
        {
            string environmentValue = getEnvironmentValue();
            string retValueStr = "";
            switch (environmentValue)
            {
                case "QA":
                    retValueStr = "https://portaltest.mazdausa.com/m371/webservices/mx/dealerLocateByName/";
                    break;
                case "TEST":
                    retValueStr = "https://portaltest.mazdausa.com/m371/webservices/mx/dealerLocateByName/";
                    break;
                case "PROD":
                    retValueStr = "https://portal.mazdausa.com/m174/webservices/mx/dealerLocateByName/";
                    break;
                default:
                    break;
            }

            Console.WriteLine("\n" + retValueStr + "\n");
            return retValueStr;
        }

        public static string getDealersByRegion()
        {
            string environmentValue = getEnvironmentValue();
            string retValueStr = "";
            switch (environmentValue)
            {
                case "QA":
                    retValueStr = "https://portaltest.mazdausa.com/m371/webservices/mx/dealerLocateByZip/";
                    break;
                case "TEST":
                    retValueStr = "https://portaltest.mazdausa.com/m371/webservices/mx/dealerLocateByZip/";
                    break;
                case "PROD":
                    retValueStr = "https://portal.mazdausa.com/m174/webservices/mx/dealerLocateByZip/";
                    break;
                default:
                    break;
            }

            Console.WriteLine("\n" + retValueStr + "\n");
            return retValueStr;
        }

        public static List<Dealer> GetDealersFromResponse(string lContent)
        {
            List<Dealer> dealerList = new List<Dealer>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(lContent);

            XmlElement root = doc.DocumentElement;

            string resultXML = root.Attributes["status"].Value;
            Console.WriteLine("resultXML" + resultXML);
            if (resultXML.Equals("ok"))
            {
                XmlNodeList allDealer = root.GetElementsByTagName("dealers");
                XmlDocument dealersDoc = new XmlDocument();
                dealersDoc.LoadXml(allDealer[0].OuterXml);

                XmlNodeList dealerNodeList = dealersDoc.GetElementsByTagName("dealer");
                
                foreach (XmlNode dealerNode in dealerNodeList)
                {
                    XmlDocument dealerDoc = new XmlDocument();
                    dealerDoc.LoadXml(dealerNode.OuterXml);
                    XmlElement dealerElement = dealerDoc.DocumentElement;
                    Dealer dealer = new Dealer();
                    dealer.Id = dealerElement.Attributes["id"].Value;
                    dealer.Name = dealerElement.Attributes["name"].Value;
                    dealerList.Add(dealer);
                }
            }
            return dealerList;
        }
    }
}
