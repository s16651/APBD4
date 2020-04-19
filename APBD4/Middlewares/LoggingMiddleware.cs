using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APBD4.Services;
using Microsoft.AspNetCore.Http;

namespace APBD4.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, SDbService service)
        {
            httpContext.Request.EnableBuffering();
            if (httpContext.Request != null)
            {
                string method = httpContext.Request.Method;
                string path = httpContext.Request.Path;
                string bodyStr = "";
                string queryString = httpContext.Request.QueryString.ToString();
                using (StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }
                service.SaveLogData(method, queryString, path, bodyStr);
            }
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            await _next(httpContext);
        }
    }
}
