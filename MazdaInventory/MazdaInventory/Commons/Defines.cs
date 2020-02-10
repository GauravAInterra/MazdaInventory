using System;
using System.Collections.Generic;
using System.Text;

namespace MazdaInventory.Commons
{
    public class Defines
    {
        /// The key for user preferences.
        public static readonly String KeyUserPreferences = "UserPreferences";
        /// The key for username.
		public static readonly String KeyUserName = "UserName";
        /// The key for password.
		public static readonly String KeyPassword = "Password";
        /// The key for shoukdRememberMe.
		public static readonly String KeyShouldRemember = "ShouldRemember";

        /// key for connection type error.
		public static readonly String KeyConnectionErrorType = "ConnectionErrorType";
        /// key for Connection Response HTTPStatusCode.
        public static readonly String KeyConnectionResponseHTTPStatusCode = "ConnectionResponseHTTPStatusCode";
        /// key for Connection Response Content String.
        public static readonly String KeyConnectionResponseContentString = "ConnectionResponseContentString";
        /// key for Connection Response Content Object.
        public static readonly String KeyConnectionResponseContentObject = "ConnectionResponseContentObject";
        /// key for Connection Response Headers.
        public static readonly String KeyConnectionResponseHeaders = "ConnectionResponseHeaders";
        /// key for KeyConnection Response Message .
        public static readonly String KeyConnectionResponseMessage = "ConnectionResponseMessage";

        /// The alert message for session timeout connection response message.
        public static readonly String ValueSessionTimeoutConnectionResponseMessage = "Your session has expired.";
        /// The alert message for HTTPStatusCode Is Not 200 .
        public static readonly String ValueHTTPStatusCodeIsNot200ConnectionResponseMessage = "There is no response from Server.";
        /// The alert message for Invalid URL.
		public static readonly String ValueInvalidURLConnectionResponseMessage = "Something seems to have gone wrong.";
        /// The alert message for Connection Timeout .
        public static readonly String ValueConnectionTimeoutConnectionResponseMessage = "There is no response from Server.";
        /// The alert message for ServerError.
        public static readonly String ValueServerErrorConnectionResponseMessage = "There is no response from Server.";
        /// The alert message for ServerError.
        public static readonly String ValueForDealerTrueConnectionResponseMessage = "You are trying to login with dealer credentials. Dealer Module is currently under development.";

        /// Timeout duration.
        public static readonly int Timeout = 60000;

        /// The key for loginCookies.
		public static readonly String KeyLoginCookies = "LoginCookies";
        /// The key for LocationDataCookies.
        public static readonly String KeyLocationDataCookies = "LocationDataCookies";
        /// The key for IsError.
        public static readonly String IsError = "IsErrorInConnection";
        /// The key for IsException.
        public static readonly String IsException = "IsExceptionInConnection";

        /// The message for NetworkUnavailable.
        public static readonly String NetworkUnavailable = "Please check your network.";
        /// The message for SessionExpired.
        public static readonly String SessionExpired = "Your session has expired.";
        /// The message for RuntimeError.
        public static readonly String RuntimeError = "Something seems to have gone wrong.";
        /// The message for NoResponse.
        public static readonly String NoResponse = "There is no response from Server.";
        /// The RequestID for login.
        public static readonly String LoginRequestServerHit = "login";
        /// The is static pic enabled.
        public static bool isStaticPicEnabled = true;
    }
}
