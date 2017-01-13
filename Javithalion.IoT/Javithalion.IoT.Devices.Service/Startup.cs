using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Javithalion.IoT.Devices.Business.ReadModel.Maps;
using Javithalion.IoT.Devices.Business;
using Javithalion.IoT.Devices.Business.ReadModel;
using Javithalion.IoT.Devices.DataAccess.Write;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;
using NJsonSchema;
using System.Reflection;
using Javithalion.IoT.Devices.DataAccess.Read;
using Javithalion.IoT.Devices.DataAccess.Write.Extensions;
using Serilog;
using System.IO;
using Javithalion.IoT.Devices.Business.PredictionsModel.Options;

namespace Javithalion.IoT.Devices.Service
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private MapperConfiguration _mapperConfiguration;
        private IHostingEnvironment _environment;

        public Startup(IHostingEnvironment env)
        {
            _environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
                builder.AddUserSecrets();

            Configuration = builder.Build();

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DeviceMapsInstaller());
            });

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Async(m =>
                                    m.RollingFile(Path.Combine(env.ContentRootPath, "./Logs/log-{Date}.txt"),
                                    outputTemplate: "{Timestamp} :: {RequestId} :: [{Level}] - {Message}{NewLine}{Exception}"))
               .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DependencyInjectionConfiguration(services);            

            if (_environment.IsDevelopment())
            {
                services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                                .AllowAnyMethod()
                                                                            .AllowAnyHeader()));
            }

            services.Configure<PredictionsApiOptions>(options =>
            {
                options.WithApiUrl(Configuration["PredictionsConfiguration:WebServiceUrl"].ToString())
                       .WithApiKey(Configuration["PredictionsConfiguration:PrimaryApiKey"].ToString());
            });

            services.AddMemoryCache();
            services.AddMvc();
        }

        private void DependencyInjectionConfiguration(IServiceCollection services)
        {
            services.AddTransient<IDeviceWriteService, DeviceWriteService>();
            services.AddTransient<IDeviceReadService, DeviceReadService>();
            services.AddTransient<IPredictionsService, PredictionsService>();
            services.AddTransient<IDeviceDao>(DeviceDaoFactory);
            services.AddSingleton<IMapper>(factory => _mapperConfiguration.CreateMapper());

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DevicesContext>(options => options.UseSqlServer(connectionString));
        }

        private IDeviceDao DeviceDaoFactory(IServiceProvider arg)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            return new DeviceDao(connectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            if (_environment.IsDevelopment())
            {
                SeedDatabase(app);
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, new SwaggerUiOwinSettings
                {
                    DefaultPropertyNameHandling = PropertyNameHandling.CamelCase
                });
                app.UseCors("AllowAll");
            }
            
            loggerFactory.AddSerilog();
            app.UseMvc();
        }

        private void SeedDatabase(IApplicationBuilder app)
        {
            using (var context = app.ApplicationServices.GetRequiredService<DevicesContext>())
            {
                context.EnsureSeeding();
            }
        }
    }
}
