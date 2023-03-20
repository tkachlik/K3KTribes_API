using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Middleware
{
    public class GlobalErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try 
            {
                await next(context);
            }
            catch (Exception) 
            {
                context.Response.StatusCode = 500;
                context.Response.WriteAsJsonAsync(new ErrorResponse(500, "", "Something went wrong."));
            }
        }
    }

    public static class GlobalErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorHandlingMiddleware>();
        }
    }
}