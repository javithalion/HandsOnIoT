using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.Business.ReadModel;
using MongoDB.Driver;
using System.Security.Authentication;
using Javithalion.IoT.Infraestructure.ModelBus;
using Javithalion.IoT.DeviceEvents.Business.WriteModel;
using AutoMapper;
using Javithalion.IoT.DeviceEvents.Business.ReadModel.Maps;
using Javithalion.IoT.DeviceEvents.Service.Infraestructure;
using NSwag.AspNetCore;
using System.Reflection;
using NJsonSchema;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands;

namespace Javithalion.IoT.DeviceEvents.Service
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private MapperConfiguration _mapperConfiguration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DependencyInjectionConfiguration(services);
            // Add framework services.
            services.AddMvc(conf => conf.Filters.Add(typeof(UnhandledExceptionFilter)));
        }

        private void DependencyInjectionConfiguration(IServiceCollection services)
        {
            services.AddTransient<IDeviceEventReadService, DeviceEventReadService>();
            services.AddTransient<IDeviceEventDao, DeviceEventDao>();

            services.AddSingleton(MongoDatabaseFactory);
            services.AddSingleton<IMapper>(factory => _mapperConfiguration.CreateMapper());
            //services.AddSingleton<IServiceBus>(ServiceBusInstaller);
        }

        private IMongoDatabase MongoDatabaseFactory(IServiceProvider serviceProvider)
        {
            MongoClient client = new MongoClient(Configuration.GetConnectionString("DefaultConnection"));
            return client.GetDatabase(Configuration["DeviceEventDatabase:Name"]);
        }

        private IServiceBus ServiceBusInstaller(IServiceProvider serviceProvider)
        {
            //TODO
            return new ServiceBus();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            //app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, new SwaggerUiOwinSettings
            //{
            //    DefaultPropertyNameHandling = PropertyNameHandling.CamelCase
            //});
        }
    }
}
