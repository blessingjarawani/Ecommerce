using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_.Net_Core.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                  .ConfigureLogging(logbuilder =>
                  {
                     logbuilder.ClearProviders();
                  })
                .ConfigureWebHostDefaults(webBuilder =>
                 {
                    webBuilder.UseStartup<Startup>();
                 });
    }
}
