using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VHM.DAL;
using VHM.DAL.Data.HangFireContext;
using VHM_APi_.Errors;
using VHM_APi_.Extentions;
using VHM_APi_.Helper;
using VHM_APi_.Hubs;
using VHM_APi_.Middlewares;
using VHW.BLL.Interfaces;
using VHW.BLL.Interfaces.BackupAndRestore;
using VHW.BLL.Repositories;
using VHW.BLL.Repositories.AmbulanceRepository;
using VHW.BLL.Repositories.HangFireRepository;
using VHW.BLL.Token;

namespace VHM_APi_
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VHM_APi_", Version = "v1" });
            });
            services.AddContextServices(Configuration);
            services.AddIdentityServices(Configuration);
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScopedServices();
            services.AddDbContext<HangFireDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("HangFireAndBackupConnection")));

            services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var errors = actionContext.ModelState.Where(M => M.Value.Errors.Count > 0)
                                                .SelectMany(M => M.Value.Errors)
                                                .Select(M => M.ErrorMessage).ToArray();
                        var Response = new ApiValidationErrorResponse()
                        {
                            Errors = errors
                        };
                        return new BadRequestObjectResult(Response);
                        
                    };
                });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            services.AddSignalR();
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VHM_APi_ v1"));
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHangfireDashboard("/dashboard");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/ChatHub");
                endpoints.MapHub<NotificationHub>("/NotificationHub");
                endpoints.MapHub<NotificationHub>("/ReadingHub");

            });
        }
    }
}
