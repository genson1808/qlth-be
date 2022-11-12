using System.Net;
using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MISA.QLTH.Common.Entities.DTO;
using MISA.QLTH.Common.Enums;
using MISA.QLTH.Common.Exceptions;
using MySqlConnector;
using ValidationException = MISA.QLTH.Common.Exceptions.ValidationException;

namespace MISA.QLTH.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if Unexpected
            var lines = ExceptionModel.Create(ex);
            var res = new ErrorResponse { };


            var exceptionType = ex.GetType();

            if (exceptionType == typeof(MySqlException))
            {
                code = HttpStatusCode.InternalServerError;
                res.ErrorCode = ErrorCode.Uneffected;
                res.UserMsg = "Xảy ra lỗi vui lòng liên hệ MISA để được hỗ trợ.";
                res.DevMsg = ex.Message;
                var lwMes = ex.Message.ToLower();
                if (lwMes.Contains("duplicate entry"))
                {
                    var errors = new Dictionary<string, string>();
                    code = HttpStatusCode.BadRequest;
                    res.ErrorCode = ErrorCode.Validate;
                    res.UserMsg = "Thông tin không hợp lệ.";
                    res.DevMsg = ex.Message;
                    if (lwMes.Contains("employeecode"))
                    {
                        errors.Add("EmployeeCode", "Mã cán bộ giáo viên đã tồn tại.");
                    }
                    else if (lwMes.Contains("email"))
                    {
                        errors.Add("Email", "Email đã tồn tại.");
                    }

                    res.MoreInfo = errors;
                }
                else
                {
                    res.MoreInfo = ex.StackTrace;
                }
            }
            else if (exceptionType == typeof(ValidationException))
            {
                var newEx = ex as ValidationException;
                code = HttpStatusCode.BadRequest;
                res.ErrorCode = ErrorCode.Validate;
                res.UserMsg = "Thông tin không hợp lệ.";
                res.DevMsg = ex.Message;
                res.MoreInfo = newEx.Errors;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                code = HttpStatusCode.NotFound;
                res.ErrorCode = ErrorCode.NotFound;
                res.UserMsg = ex.Message;
                res.DevMsg = ex.Message;
                res.MoreInfo = lines;
            }
            else
            {
                code = HttpStatusCode.InternalServerError;
                res.ErrorCode = ErrorCode.Exception;
                res.UserMsg = "Xảy ra lỗi vui lòng liên hệ MISA để được hỗ trợ.";
                res.DevMsg = ex.Message;
                res.MoreInfo = lines;
            }

            var result = JsonSerializer.Serialize(res);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}