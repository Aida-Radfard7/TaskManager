using System.Reflection.PortableExecutable;
using TaskManager.Core.Domain.Entities;
using TaskManager.Infra.Data.EF.SqlServer.Repositories;

namespace TaskManager.Endpoints.Rest.Middlewares
{
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context , IRequestLogRepository _repository)
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);

            var log = new RequestLog
            {
                Id = Guid.NewGuid(),
                Method = context.Request.Method,
                Path = context.Request.Path,
                Body = await reader.ReadToEndAsync(),
            };

            await _repository.SaveLogAsync(log);

            await _next(context);

        }
    }
}
