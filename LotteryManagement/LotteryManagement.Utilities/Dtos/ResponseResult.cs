using System.Collections.Generic;

namespace LotteryManagement.Utilities.Dtos
{


    public class ResponseResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }


        public ResponseResult(bool status, string message)
        {
            IsSuccess = status;
            Message = message;
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        public ResponseResult(string message)
        {
            IsSuccess = false;
            Message = message;

        }

    }
}
