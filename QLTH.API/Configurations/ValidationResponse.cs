using System;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QLTH.Common.Entities.DTO;
using QLTH.Common.Exceptions;
using ValidationException = QLTH.Common.Exceptions.ValidationException;

namespace QLTH.API.Configurations
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