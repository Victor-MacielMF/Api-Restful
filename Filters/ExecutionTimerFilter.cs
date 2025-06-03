using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Filters
{
    public class ExecutionTimeFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            // Executa a action
            var resultContext = await next();

            stopwatch.Stop();

            // Só adiciona o tempo se a resposta for DataResponse<>
            if (resultContext.Result is ObjectResult objectResult && objectResult.Value != null)
            {
                var valueType = objectResult.Value.GetType();
                // Checa se é DataResponse<T> (tipado)
                if (valueType.IsGenericType && valueType.Name.StartsWith("DataResponse"))
                {
                    var elapsedProperty = valueType.GetProperty("ElapsedMilliseconds");
                    if (elapsedProperty != null && elapsedProperty.CanWrite)
                    {
                        elapsedProperty.SetValue(objectResult.Value, stopwatch.Elapsed.TotalMilliseconds);
                    }
                }
            }
        }
    }
}