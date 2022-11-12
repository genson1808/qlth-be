using System;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MISA.QLTH.Common.Entities.DTO;
using MISA.QLTH.Common.Exceptions;
using ValidationException = MISA.QLTH.Common.Exceptions.ValidationException;

namespace MISA.QLTH.API.Configurations
{
    public static class ValidationResponse
    {
        public static IActionResult MakeValidationResponse(ActionContext context)
        {
            var errorList = context.ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            throw new ValidationException(errorList);
        }
    }
}