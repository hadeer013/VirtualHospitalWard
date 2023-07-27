using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VHM.DAL.Data;
using VHM.DAL.Data.HangFireContext;

namespace VHM_APi_
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = services.GetRequiredService<HospitalContext>();
                await context.Database.MigrateAsync();

                var HangFirecontext = services.GetRequiredService<HangFireDbContext>();
                await HangFirecontext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex.Message, "An Error occured during applying migration");
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
