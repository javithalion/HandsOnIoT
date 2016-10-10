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

namespace Javithalion.IoT.DeviceEvents.Service
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DependencyInjectionConfiguration(services);

            // Add framework services.
            services.AddMvc();
        }
       
        private void DependencyInjectionConfiguration(IServiceCollection services)
        {
            services.AddTransient<IDeviceEventReadService, DeviceEventReadService>();
            services.AddSingleton<IServiceBus, ServiceBus>();

            services.AddTransient<IDeviceEventDao, DeviceEventDao>();
            services.AddTransient<IMongoDatabase>(MongoDatabaseFactory);            
        }

        private IMongoDatabase MongoDatabaseFactory(IServiceProvider arg)
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress("javithalion-iot-deviceevent.documents.azure.com", 10250);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity("DeviceEvents", "javithalion-iot-deviceevent");
            MongoIdentityEvidence evidence = new PasswordEvidence("LLzU3NDePowdrVopNKcHruc3EKwWm9WQ3e87yXDqMM5MWEK6jwKQyzxqtSH0m2hrwmvVfkydtF4aOzrVcKMf4A");

            settings.Credentials = new List<MongoCredential>() { new MongoCredential("SCRAM-SHA-1", identity, evidence) };

            MongoClient client = new MongoClient(settings);
            return client.GetDatabase("DeviceEvents");

            //MongoClient client = new MongoClient(Configuration.GetConnectionString("DefaultConnection"));
            //return client.GetDatabase(Configuration["DeviceEventDatabase:Name"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
