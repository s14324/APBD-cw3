using Cw3.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IDbService service)
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                string method = httpContext.Request.Method.ToString();
                string path = httpContext.Request.Path;
                string queryString = httpContext.Request?.QueryString.ToString();
                var bodyStream = string.Empty;

                using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStream = await reader.ReadToEndAsync();
                }

                StringBuilder sb = new StringBuilder();

                sb.Append("\nMetoda: " + method);
                sb.Append("\nSciezka: " + path);
                sb.Append("\nQuery string: " + queryString);
                sb.Append("\nBody: " + bodyStream);
                File.AppendAllText("log.txt", sb.ToString());
                sb.Clear();
            }

            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            await _next(httpContext);
        }
    }
}
