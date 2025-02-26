using Api.Errors;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Middleware
{
	public class ExceptionMiddleware
	{
        private readonly IHostEnvironment host;
        private readonly RequestDelegate next;
        public ExceptionMiddleware(IHostEnvironment host, RequestDelegate next)
        {
            this.host = host;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex, host);
            }
        }

		private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment host)
		{
			context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = host.IsDevelopment()
                ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new ApiErrorResponse(context.Response.StatusCode, ex.Message, "intenalServerERROR");

            var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            var json = JsonSerializer.Serialize(response, options);

            return context.Response.WriteAsync(json);

		}
	}
}
