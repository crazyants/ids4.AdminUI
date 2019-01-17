using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.DBManager
{
    public class ContextFactory : IDesignTimeDbContextFactory<PermissionConext>
    {
        public PermissionConext CreateDbContext(string[] args)
        {
            var serviceProvider = WebHost.CreateDefaultBuilder(args)
                 .UseStartup<Startup>()
                 .Build().Services;
            return serviceProvider.CreateScope().ServiceProvider.GetService<PermissionConext>();
        }
    }
}
