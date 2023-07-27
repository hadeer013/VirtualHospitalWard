namespace VHM_APi_.Errors
{
    public class ApiExceptionErrorResponse:ApiErrorResponse
    {
        public string Details { get; set; }
        public ApiExceptionErrorResponse(int StatusCode, string Message = null, string Details = null):base(StatusCode,Message)
        {
            this.Details= Details;
        }

    }
}
