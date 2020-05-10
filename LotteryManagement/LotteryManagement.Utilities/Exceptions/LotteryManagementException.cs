using System;
using System.Runtime.Serialization;

namespace LotteryManagement.Utilities.Exceptions
{
    public class LotteryManagementException: Exception
    {

        public LotteryManagementException()
        {
        }

        public LotteryManagementException(string message) : base(message)
        {
        }

        public LotteryManagementException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LotteryManagementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
