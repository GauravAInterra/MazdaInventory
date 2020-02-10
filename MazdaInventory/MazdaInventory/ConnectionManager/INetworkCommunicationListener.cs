using System;
using System.Collections.Generic;
using System.Text;

namespace MazdaInventory.ConnectionManager
{
    public interface INetworkCommunicationListener
    {
        /// Notifies the HTTP Respons.
        void notifyHTTPRespons(object result, string RequestID);
        /// Notifies the HTTP Error.
        void notifyHTTPError(object result, string RequestID);
    }
}
