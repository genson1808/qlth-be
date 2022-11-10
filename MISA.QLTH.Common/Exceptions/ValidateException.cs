
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MISA.QLTH.Common.Exceptions
{
    public abstract class ValidateException: Exception
    {
        ModelStateDictionary Errors {get; set; }

        public ValidateException(ModelStateDictionary errors) {
            Errors = errors;
        }
    }
}