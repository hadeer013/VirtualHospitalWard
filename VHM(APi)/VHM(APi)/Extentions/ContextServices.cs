using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VHM.DAL.Data;


namespace VHM_APi_.Extentions
{
    public static class ContextServices
    {
        
        public static void AddContextServices(this IServiceCollection services,IConfiguration Configuration)
        {
            services.AddDbContext<HospitalContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            
        }
    }
}
