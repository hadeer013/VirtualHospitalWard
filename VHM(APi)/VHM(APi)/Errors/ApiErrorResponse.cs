using System;

namespace VHM_APi_.Errors
{
    public class ApiErrorResponse
    {
        
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiErrorResponse(int StatusCode,string Message=null)
        {
            this.StatusCode = StatusCode;
            this.Message = Message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string GetDefaultMessageForStatusCode(int StatusCode)
        => StatusCode switch
        {
            400 => "A bad Request ,You have made",
            401 => "Authorized, You are not!",
            404 => "Resource was not found",
            500 => "Errors are the path to the dark side,Errors lead to Anger." +
                   "Anger leads to hate.Hate leads to career change!!",
            _ => null
        };
    }
}
