using System.Collections.Generic;

namespace VHM_APi_.Errors
{
    public class ApiValidationErrorResponse:ApiErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorResponse():base(400)
        {

        }
    }
}
