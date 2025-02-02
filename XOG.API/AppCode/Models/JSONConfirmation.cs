﻿namespace XOG.Models
{
    public class JSONConfirmation
    {
        public JSONConfirmation()
        {
        }

        public JSONConfirmation(bool isSuccess, string message)
        {
            Message = message;
            IsSuccess = isSuccess;
        }

        public JSONConfirmation(bool isSuccess, bool isError, string message)
        {
            Message = message;
            IsSuccess = isSuccess;
            IsError = isError;
        }

        public bool IsError { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Object { get; set; }
    }
}
