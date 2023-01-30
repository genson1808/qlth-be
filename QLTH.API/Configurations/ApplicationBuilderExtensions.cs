using QLTH.API.Middleware;

namespace QLTH.API.Configurations
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}