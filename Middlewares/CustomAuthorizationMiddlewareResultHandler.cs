using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Newtonsoft.Json;

namespace api.Middlewares
{
    public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new AuthorizationMiddlewareResultHandler();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            // Inicia timer para ElapsedMilliseconds
            var stopwatch = Stopwatch.StartNew();

            if (!authorizeResult.Succeeded)
            {
                stopwatch.Stop();
                context.Response.ContentType = "application/json";
                if (!context.User.Identity?.IsAuthenticated ?? true)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    var dataResponse = DataResponseHelper.Error<object>(
                        "You must be authenticated to perform this action.",
                        new[] { "Authentication is required." },
                        stopwatch.Elapsed.TotalMilliseconds
                    );
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(dataResponse));
                    return;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;

                    // Verifica roles necessárias na policy
                    var rolesRequirement = policy.Requirements.OfType<RolesAuthorizationRequirement>().FirstOrDefault();
                    string message = "You do not have permission to perform this action.";
                    if (rolesRequirement != null && rolesRequirement.AllowedRoles.Any())
                    {
                        var requiredRoles = string.Join(", ", rolesRequirement.AllowedRoles);
                        message = $"Only users with the following role(s) can perform this action: {requiredRoles}";
                    }

                    var dataResponse = DataResponseHelper.Error<object>(
                        "You are not authorized to perform this action.",
                        new[] { message },
                        stopwatch.Elapsed.TotalMilliseconds
                    );
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(dataResponse));
                    return;
                }
            }

            stopwatch.Stop();
            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}