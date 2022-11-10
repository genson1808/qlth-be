using MISA.QLTH.API.Middleware;

namespace MISA.QLTH.API.Configurations
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}