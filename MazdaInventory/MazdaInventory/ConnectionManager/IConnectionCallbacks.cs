using System;
using System.Collections.Generic;
using System.Text;

namespace MazdaInventory.ConnectionManager
{
    public interface IConnectionCallbacks
    {
        /// Connections the was successfull with result.
        void ConnectionWasSuccessFullWithResult(Object result, string RequestID);
        /// Connections the failed with error.
        void ConnectionFailedWithError(Object error, string RequestID);
    }
}
