using Ecom.API.Errors;
using System.Net;
using System.Text.Json;

namespace Ecom.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString());
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);

            }
        }
    }
}
