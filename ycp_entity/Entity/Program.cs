﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Entity {
    public class Program {

        public static dpz.Mvc.Sessions.MemorySessionManager.CacheManager CacheManager;

        public static void Main(string[] args) {
            CacheManager = new dpz.Mvc.Sessions.MemorySessionManager.CacheManager();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            var builder = WebHost.CreateDefaultBuilder(args);

            if (site.Config.Https.Enable) {
                builder.UseKestrel(options => {
                    options.Listen(IPAddress.Any, site.Config.Https.Port, listenOptions => {
                        //填入之前iis中生成的pfx文件路径和指定的密码　　　　　　　　　　　　
                        listenOptions.UseHttps(site.Config.Https.PfxPath, site.Config.Https.PfxPassword);
                    });
                });
            }

            if (site.Config.Http.Enable) {
                builder.UseKestrel(options => {
                    options.Listen(IPAddress.Any, site.Config.Http.Port);
                });
            }

            return builder.UseStartup<Startup>();
        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();
    }
}
