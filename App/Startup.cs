using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using App.Areas.Auth.Models;
using App.Areas.Chc.Utils;
using App.Areas.Crm.Utils;
using App.Areas.Doc.Utils;
using App.Areas.Events.Utils;
using App.Areas.External;
using App.Areas.Groups.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Data;
using App.Hubs;
using AspNetCore.Identity.Mongo;
using AutoMapper;
using Core.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace App
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ApplicationDbContext>();

            services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireDigit = false;
            }, mongoIdentityOptions => { mongoIdentityOptions.ConnectionString = Configuration.GetMongoConnection(); });

            services.AddAutoMapper(config =>
            {
                ChcMapper.MapModels(config);
                CrmMapper.MapModels(config);
                EventsMapper.MapModels(config);
                DocumentsMapper.MapModels(config);
                EventsMapper.MapModels(config);
                GroupsMapper.MapModels(config);
                ExternalMapper.MapModels(config);
            });

            // Add application services.
            //services.AddTransient<IEmailSender, EmailSender>();
            ConfigureDepenedencyInjection(services);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            services.AddMvc(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                    options.Filters.Add(new ValidateModelAttribute());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Demo API",
                    Version = "0.0.1",
                    Description = "This is a restful web api",
                    Contact = new Contact
                    {
                        Name = "Timothy Kasasa",
                        Email = "timothyk@laboremus.no"
                    }
                });

                var basePath = Environment.WebRootPath;
                var xmlPath = Path.Combine(basePath, "Documentation", "App.xml");
                c.IncludeXmlComments(xmlPath);
                c.DescribeAllEnumsAsStrings();

            });
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("default");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseCustomErrorHandling();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1");
                c.InjectStylesheet("/swagger-ui/custom.css");
                c.DocumentTitle = "CRM Service";
                c.RoutePrefix = "docs";
            });
            app.UseFileServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }

        private void ConfigureDepenedencyInjection(IServiceCollection services)
        {
            var skip = new HashSet<string>
            {
                "GenericRepository`1",
                "GenericService`3"
            };
            var assemblies = new List<string>
            {
                GetType().Assembly.GetName().Name
            };
            foreach (var item in assemblies)
            {
                var assembly = Assembly.Load(item);
                var types = assembly
                    .GetExportedTypes()
                    .Where(it => it.IsClass && !skip.Contains(it.Name))
                    .Select(it =>
                    {
                        // get interface class, with similar name
                        var inter = it.GetInterfaces()
                            ?.Where(ot => ot.Name.Contains(it.Name))
                            .FirstOrDefault();
                        // Return a pair
                        return (it, inter);
                    })
                    //Remove types without interfaces
                    .Where(it => it.Item2 != null);

                foreach (var (cls, intf) in types)
                {
                    Console.WriteLine($"Service: {cls.FullName} impl:{intf.FullName}");
                    services.AddScoped(intf, cls);
                }
            }
        }
    }
}