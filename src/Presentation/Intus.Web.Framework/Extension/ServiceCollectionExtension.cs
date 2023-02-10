using Intus.Data;
using Intus.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Intus.Web.Framework.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
          
            services.AddDbContext<ApplicationDbContext>(options =>
            
                options.UseSqlServer(configuration.GetConnectionString("ApplicationConnection"))
            );


            services.AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>));
        
        }
    }
}
