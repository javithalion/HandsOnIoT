using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.Business.ReadModel;
using MongoDB.Driver;
using AutoMapper;
using Javithalion.IoT.DeviceEvents.Business.ReadModel.Maps;
using NSwag.AspNetCore;
using System.Reflection;
using NJsonSchema;
using Javithalion.IoT.DeviceEvents.Business;
using Javithalion.IoT.DeviceEvents.Business.WriteModel;
using Javithalion.IoT.DeviceEvents.Service.Middlewares;
using Serilog;
using System.IO;
using Mongo2Go;
using Javithalion.IoT.DeviceEvents.Service.Extensions;
using Hangfire;
using Javithalion.IoT.DeviceEvents.Service.JobsScheduler;
using Javithalion.IoT.DeviceEvents.Business.PredictionsModel;

namespace Javithalion.IoT.DeviceEvents.Service
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private MapperConfiguration _mapperConfiguration;
        private readonly IHostingEnvironment _environment; //a bit ugly https://github.com/aspnet/Hosting/issues/429       

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
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Async(m =>
                                m.RollingFile(Path.Combine(env.ContentRootPath, "./Logs/log-{Date}.txt"),
                                outputTemplate: "{Timestamp} :: {RequestId} :: [{Level}] - {Message}{NewLine}{Exception}"))
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DependencyInjectionConfiguration(services);

            // Add framework services.
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                            .AllowAnyMethod()
                                                                            .AllowAnyHeader()));

            var hangFireConnectionString = Configuration.GetConnectionString("HangfireConnection");
            services.AddHangfire(hf => hf.UseSqlServerStorage(hangFireConnectionString));

            services.AddMvc();
        }

        private void DependencyInjectionConfiguration(IServiceCollection services)
        {
            services.AddTransient<IDeviceEventWriteService, DeviceEventWriteService>();
            services.AddTransient<IDeviceEventReadService, DeviceEventReadService>();
            services.AddTransient<IDeviceEventsFeederService, DeviceEventsMachineLearningFeederService>();
            services.AddTransient<IDeviceEventDao, DeviceEventDao>();

            services.AddSingleton(MongoDatabaseFactory);
            services.AddSingleton<IMapper>(factory => _mapperConfiguration.CreateMapper());
        }

        private IMongoDatabase MongoDatabaseFactory(IServiceProvider serviceProvider)
        {
            if (_environment.IsDevelopment())
            {
                return GetDevelopmentDatabase();
            }
            else
            {
                var client = new MongoClient(Configuration.GetConnectionString("DefaultConnection"));
                return client.GetDatabase(Configuration["DeviceEventDatabase:Name"]);
            }
        }

        private IMongoDatabase GetDevelopmentDatabase()
        {
            var runner = MongoDbRunner.StartForDebugging();

            var seedingFilePath = Path.Combine(Directory.GetCurrentDirectory(), Configuration["DeviceEventDatabase:SeedingFile"]);
            if (File.Exists(seedingFilePath))
                runner.Import(Configuration["DeviceEventDatabase:Name"],
                             Configuration["DeviceEventDatabase:CollectionName"],
                             seedingFilePath,
                             true);

            var client = new MongoClient(new MongoClientSettings
            {
                MaxConnectionPoolSize = 800,
                Server = new MongoServerAddress("localhost", 27017)
            });

            return client.GetDatabase(Configuration["DeviceEventDatabase:Name"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddSerilog();
            
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, new SwaggerUiOwinSettings
                {
                    DefaultPropertyNameHandling = PropertyNameHandling.CamelCase
                });
                app.UseCors("AllowAll");

                app.SeedData();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();

            appLifetime.ApplicationStopping.Register(OnApplicationStopping);
            appLifetime.ApplicationStarted.Register(OnApplicationStarted);            
        }

        private void OnApplicationStarted()
        {
            try
            {
                RetrainMachineLearningModelJob.Start();
                Log.Logger.Information("OnApplicationStarted finished properly");
            }
            catch(Exception ex)
            {
                Log.Logger.Fatal($"Problem executing post-started application's task {ex.ToString()}");
            }
        }

        private void OnApplicationStopping()
        {
            try
            {
                RetrainMachineLearningModelJob.Stop();
                Log.Logger.Information("OnApplicationStopping finished properly");
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal($"Problem executing on-stopping application's task {ex.ToString()}");
            }
        }
    }
}
