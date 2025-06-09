using api.Helpers;
using Newtonsoft.Json;

namespace api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var response = DataResponseHelper.Error<object>(
                    "Unexpected error",
                    new[] { ex.Message },
                    stopwatch.Elapsed.TotalMilliseconds
                );
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                return;
            }
            finally
            {
                stopwatch.Stop();
                context.Items["ExecutionElapsed"] = stopwatch.Elapsed.TotalMilliseconds;
            }
        }
    }

}