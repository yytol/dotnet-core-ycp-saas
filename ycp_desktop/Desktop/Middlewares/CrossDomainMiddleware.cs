using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BlueDesktop.Middlewares {
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CrossDomainMiddleware {
        private readonly RequestDelegate _next;

        public CrossDomainMiddleware(RequestDelegate next) {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext) {
            if (!httpContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin")) httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            if (!httpContext.Response.Headers.ContainsKey("Access-Control-Allow-Methods"))
                httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "OPTIONS,POST,GET");
            if (!httpContext.Response.Headers.ContainsKey("Access-Control-Allow-Headers"))
                httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "x-requested-with,content-type");
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CrossDomainMiddlewareExtensions {
        public static IApplicationBuilder UseCrossDomainMiddleware(this IApplicationBuilder builder) {
            return builder.UseMiddleware<CrossDomainMiddleware>();
        }
    }
}
