using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QLTH.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public  Dictionary<string, string[]> Errors { get; set; }

        public ValidationException(Dictionary<string, string[]> errors)
        {
            Errors = errors;
        }
    }
}