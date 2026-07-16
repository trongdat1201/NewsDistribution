using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace DATNWF_API.Filters
{
    public class AuditLogFilter : IAsyncActionFilter
    {
        private readonly ILogger<AuditLogFilter> _logger;

        public AuditLogFilter(ILogger<AuditLogFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var method = request.Method;

            // Chỉ ghi nhận nhật ký với các hành động thay đổi dữ liệu (POST, PUT, DELETE)
            bool isWriteAction = method == "POST" || method == "PUT" || method == "DELETE";

            var executedContext = await next(); // Thực thi API action

            if (isWriteAction)
            {
                var username = context.HttpContext.User.Identity?.Name ?? "Anonymous";
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
                var path = request.Path;
                var statusCode = executedContext.HttpContext.Response.StatusCode;

                _logger.LogInformation(
                    "AUDIT TRAIL | User: {Username} | IP: {IP} | Method: {Method} | Path: {Path} | Status: {StatusCode}",
                    username, ipAddress, method, path, statusCode
                );
            }
        }
    }
}
