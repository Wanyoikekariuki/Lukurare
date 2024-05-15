using Authentication.Jwt.Config;
using Authentication.Jwt.Custom;
using Authentication.Jwt.Service;
using CommunicationLibrary.Gateways.Config;
using EFDatabaseModel.Contexts;
using LukurareBackend.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectBase.Database.Connection;
using ProjectBaseWeb.JSONConverters;
using ProjectBaseWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lukurare
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //    public static readonly ILoggerFactory MyLoggerFactory
        //= LoggerFactory.Create(builder => { builder.AddFile("logs/myapp-{Date}.txt"); });

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //add access the content to able to set the time offset
            //https: //docs.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-5.0
            services.AddHttpContextAccessor();
            services.AddTransient<IDateTimeZoneManger, DateTimeZoneManger>();
            //https://www.ryadel.com/en/use-json-net-instead-of-system-text-json-in-asp-net-core-3-mvc-projects/
            //above covers AddNewtonsoftJson

            //Chat services(SignalR)
            services.AddSignalR()
                    .AddJsonProtocol(options => {
                        options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                    });
            services.AddSignalR(e =>
            {
                e.MaximumReceiveMessageSize = 102400000; // Set an appropriate value
            });
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });
            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .WithOrigins("https://localhost:44324")
                       .AllowCredentials();
            }));


            var defaultContractResolver =
                new Newtonsoft.Json.Serialization.DefaultContractResolver();
            services
                .AddRazorPages()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ContractResolver = defaultContractResolver;
                });

            services
                .AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.UseMemberCasing();
                    opt.SerializerSettings.NullValueHandling = Newtonsoft
                        .Json
                        .NullValueHandling
                        .Include;
                    opt.SerializerSettings.DefaultValueHandling = Newtonsoft
                        .Json
                        .DefaultValueHandling
                        .Include;
                    opt.SerializerSettings.MissingMemberHandling = Newtonsoft
                        .Json
                        .MissingMemberHandling
                        .Ignore;
                    opt.SerializerSettings.DateTimeZoneHandling = Newtonsoft
                        .Json
                        .DateTimeZoneHandling
                        .Utc;
                    opt.SerializerSettings.DateParseHandling = Newtonsoft
                        .Json
                        .DateParseHandling
                        .None;
                    opt.SerializerSettings.Converters.Add(new DateTimeJSONConverter(services));
                    opt.SerializerSettings.Converters.Add(
                        new NullableDateTimeJSONConverter(services)
                    );
                    opt.SerializerSettings.Error = (sender, ev) => {
                        //int y = 0;
                    };
                    //opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
                });

            //https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio
            services.AddSwaggerGen();

            #region jwt authentication
            //// configure strongly typed settings object
            services.Configure<AppJwtSettings>(Configuration.GetSection("AppJwtSettings"));
            services.Configure<SMSGatewayDefination>(
                Configuration.GetSection("SMSGatewayDefination")
            );
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<AppConstants>(Configuration.GetSection("AppConstants"));

            services.AddTransient<IMailService, MailService>();
            //// configure DI for application services
            services.AddScoped<IUserJwtService, UserJwtService>();
            //services.AddTokenAuthentication(Configuration);

            services
                .AddAuthentication("Basic")
                .AddScheme<BasicAuthenticationOptions, CustomAuthenticationHandler>("Basic", null);
            var config = new ConfigurationBuilder()
                 .AddJsonFile(
                 $"appsettings.json",
                 true,
                 true
             ).Build();
            ContextConnectionService._configuration = config;
            ContextConnectionService.SetDefaultConnectionStringName(
                "BulkSMSGatwayConnectionString"
            );
            ContextConnectionService.AddConnectionString(
                ContextConnectionService.DefaultConnectionStringName,
                Configuration.GetConnectionString(
                    ContextConnectionService.DefaultConnectionStringName
                )
            );

            var connectionSting = Configuration.GetConnectionString(
                ContextConnectionService.DefaultConnectionStringName
            );
            //    services.AddDbContext<EFDatabaseModelDatabaseContext>(options =>
            //options
            //    .UseLoggerFactory(MyLoggerFactory) // Add this line to enable logging
            //    .UseMySQL(Configuration.GetConnectionString(ContextConnectionService.DefaultConnectionStringName)));
            services.AddDbContext<EFDatabaseModelDatabaseContext>(o => o.UseMySQL(connectionSting));

            //services.AddLogging(builder => builder.AddDebug());


            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(
                async (context, next) =>
                {
                    await TimeZoneManagerService.Process(context, next);
                    await next();
                }
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //consider moving swagger to development environment only
            //Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwaggerUI();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bulk SMS Gateway Web");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseWebSockets(new Microsoft.AspNetCore.Builder.WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
            });

            #region authentication for Jwt in our case
            app.UseAuthentication();
            app.UseAuthorization();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/ChatHub", options =>
                {
                    //options.Transports = HttpTransportType.WebSockets;
                    //options.WebSockets.SubProtocolSelector = protocols => protocols.FirstOrDefault();
                });
            });
        }
    }
}
