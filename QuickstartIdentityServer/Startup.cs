// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using QuickstartIdentityServer.DBManager;
using QuickstartIdentityServer.Filters;
using QuickstartIdentityServer.IdsAuthorization;
using Swashbuckle.AspNetCore.Swagger;

namespace QuickstartIdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            Env = env;
            //var logger = LoggerFactory.CreateLogger("Startup");
        }

        public IConfiguration Configuration { get; }
        public ILoggerFactory LoggerFactory { get; }
        public IHostingEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                config.Filters.Add(new WebApiDoResultAttribute());
                config.Filters.Add(new WebApiExceptionFilterAttribute());
                //config.Filters.Add(new ModelValidationAttribute());
            }).AddJsonOptions(options => //全局配置Json序列化处理
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            //const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;database=IdentityServer4.Quickstart.EntityFramework-2.0.0;trusted_connection=yes;";
            const string connectionString = @"Data Source=192.168.103.251;port=3306;Initial Catalog=IdentityServer4.Quickstart;user id=root;password=123456;Character Set=utf8;";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer(options => options.Authentication.CookieAuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme)
                .AddDeveloperSigningCredential()
                .AddResourceOwnerValidator<PasswordValidator>()
                //.AddTestUsers(Config.GetUsers())
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseMySql(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly)
                                        .CharSetBehavior(CharSetBehavior.AppendToAllColumns)
                                        .AnsiCharSet(CharSet.Utf8mb4)
                                        .UnicodeCharSet(CharSet.Utf8mb4));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseMySql(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly)
                                        .CharSetBehavior(CharSetBehavior.AppendToAllColumns)
                                        .AnsiCharSet(CharSet.Utf8mb4)
                                        .UnicodeCharSet(CharSet.Utf8mb4));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                });

            string url= Configuration["ASPNETCORE_URLS"];
            if (string.IsNullOrEmpty(url)) url = "http://127.0.0.1";
            Console.WriteLine($"url;{url}");
            //添加身份认证服务
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters();
                     options.RequireHttpsMetadata = false;
                     options.Audience = "api1";//api范围
                     options.Authority = url;//IdentityServer地址
                 });

            //添加自定义的权限验证
            services.AddAuthorization(options =>
            {
                options.AddPolicy(IdsAuthAttribute.policy, policy => policy.AddRequirements(new IdsAuthPermissionHandler("base")));
            });

            #region context
            services.AddDbContext<PermissionConext>(options =>
            {
                //Configuration.GetConnectionString("sql4")
                options.UseMySql(connectionString, b => b.MigrationsAssembly(migrationsAssembly).UseRelationalNulls()
                                                        .CharSetBehavior(CharSetBehavior.AppendToAllColumns)
                                                        .AnsiCharSet(CharSet.Utf8mb4)
                                                        .UnicodeCharSet(CharSet.Utf8mb4));
                //options.UseMySQL(Configuration.GetConnectionString("MysqlConnection"), b => b.MigrationsAssembly(this.GetType().GetTypeInfo().Assembly.FullName));
                //if (Env.IsDevelopment())
                //{
                options.UseLoggerFactory(LoggerFactory).EnableSensitiveDataLogging();
                //options.ConfigureWarnings(warnings => 
                //    warnings.Throw(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.QueryClientEvaluationWarning));
                //}
            }, ServiceLifetime.Scoped, ServiceLifetime.Singleton);
            #endregion
            //services.AddAuthentication()
            //    .AddGoogle("Google", options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //        options.ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com";
            //        options.ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo";
            //    })
            //    .AddOpenIdConnect("oidc", "OpenID Connect", options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //        options.SignOutScheme = IdentityServerConstants.SignoutScheme;

            //        options.Authority = "https://demo.identityserver.io/";
            //        options.ClientId = "implicit";

            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            NameClaimType = "name",
            //            RoleClaimType = "role"
            //        };
            //    });

            #region swagger
            //if (Env.IsDevelopment())
            //{
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                    var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                    c.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"", Name = "Authorization", Type = "apiKey" });
                    GetXmlCommentsPath(c, $"{Assembly.GetEntryAssembly().GetName().Name}");
                });
            //}
            #endregion
        }
        /// <summary>
        /// 获取api文档xml文件的路径
        /// </summary>
        /// <param name="c"></param>
        /// <param name="name"></param>
        private static void GetXmlCommentsPath(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions c, string name)
        {
            var fileName = string.Format(@"{0}.xml", name);
            var file = Path.Combine(AppContext.BaseDirectory, fileName);
            if (File.Exists(file))
            {
                c.IncludeXmlComments(file);
            }
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region swagger
            //if (env.IsDevelopment())
            //{
                //At this point, you can spin up your application and view the generated Swagger JSON at "/swagger/v1/swagger.json."
                app.UseSwagger();
                //Now you can restart your application and check out the auto-generated, interactive docs at "/swagger".
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            //}
            #endregion

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}