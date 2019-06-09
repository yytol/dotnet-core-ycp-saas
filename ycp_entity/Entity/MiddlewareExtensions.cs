using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity {
    public static class MiddlewareExtensions {
        public static IApplicationBuilder UseCrossDomain(this IApplicationBuilder builder) {
            return builder.UseMiddleware<Middlewares.CrossDomainMiddleware>();
        }
    }
}
