using DataAccess.Base;
using DataAccess.Management;
using DataAccess.System;
using Microsoft.Extensions.DependencyInjection;
using ObjectInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Startups
{
    public class DAStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped(typeof(IDataAccess<>), typeof(DataAccessBase<>));
            services.AddScoped<IDataAccess<ProductInfo>, ProductDA>();
            services.AddScoped<IDataAccess<AllCodeInfo>, AllCodeDA>();

        }
    }
}
