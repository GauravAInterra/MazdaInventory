using System;
using System.Collections.Generic;
using System.Text;

namespace MazdaInventory.ConnectionManager
{
    public enum ConnectionErrorType
    {
        /// Connection error type:- none.
        EConnectionError_None,
        /// Connection error type:- NoNetwork.
        EConnectionError_NoNetwork,
        /// Connection error type:- LocalValidationFailed_NilOrBlankUserName.
        EConnectionError_LocalValidationFailed_NilOrBlankUserName,
        /// Connection error type:- LocalValidationFailed_NilOrBlankPassword.
        EConnectionError_LocalValidationFailed_NilOrBlankPassword,
        /// Connection error type:- HTTPStatusCodeIsNot200.
        EConnectionError_ServerSide_HTTPStatusCodeIsNot200,
        /// Connection error type:- EmptyResponse.
        EConnectionError_ServerSide_EmptyResponse,
        /// Connection error type:- AuthenticationFailed.
        EConnectionError_ServerSide_AuthenticationFailed,
        /// Connection error type:- SessionTimeout.
        EConnectionError_ServerSide_SessionTimeout,
        /// Connection error type:- CouldNotGetAuthenticationToken.
        EConnectionError_ServerSide_CouldNotGetAuthenticationToken,
        /// Connection error type:- Others.
		EConnectionError_ServerSide_Others,
        /// Connection error type:- BadRequest.
        EConnectionError_URL_BadRequest,
        /// Connection error type:- Timeout.
        EConnectionError_URL_Timeout,
        ///For UNIdentified Errors
        /// Connection error type:- Type1.
		EConnectionErrorType1,
        /// Connection error type:- Type2.
		EConnectionErrorType2,
        /// Connection error type:- TypeN.
		EConnectionErrorTypeN
    }
}
