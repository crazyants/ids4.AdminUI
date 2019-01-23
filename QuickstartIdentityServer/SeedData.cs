using System;
using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuickstartIdentityServer.DBManager;
using QuickstartIdentityServer.Filters;

namespace QuickstartIdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding database...");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var pcontext = scope.ServiceProvider.GetService<PermissionConext>();
                pcontext.Database.Migrate();
                EnsureSeedData(pcontext);

                scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();
                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                EnsureSeedData(context);
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }
        private static void EnsureSeedData(PermissionConext context)
        {
            if (!context.User.Any())
            {
                Console.WriteLine("User being populated");
                var adminuser = new UserEntity
                {
                    Account = "superadmin",
                    Name = "superadmin",
                    Pwd = EncryptUtil.GetMd5("666666"),
                    IsSystemAdmin = true
                };
                context.User.Add(adminuser);
                context.SaveChanges();
            }
            if(!context.App.Any(a=>a.Code == "base"))
            {
                context.App.Add(new AppEntity
                {
                    Code = "base",
                    Name = "权限系统"
                });
                context.Module.Add(new ModuleEntity
                {
                    Code = "base",
                    Name = "人员管理"
                });
                context.Module.Add(new ModuleEntity
                {
                    Code = "base",
                    Name = "系统管理"
                });
                context.Module.Add(new ModuleEntity
                {
                    Code = "base",
                    Name = "角色管理"
                });
                context.SaveChanges();
            }
        }
        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Console.WriteLine("Clients being populated");
                foreach (var client in Config.GetClients().ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources().ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }

            if (!context.ApiResources.Any())
            {
                Console.WriteLine("ApiResources being populated");
                foreach (var resource in Config.GetApiResources().ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }
        }
    }
}
