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
using Serilog.Sinks.Elasticsearch;

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

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Async(m =>
                                m.RollingFile(Path.Combine(env.ContentRootPath, "./Logs/log-{Date}.txt"),
                                outputTemplate: "{Timestamp} :: {RequestId} :: [{Level}] - {Message}{NewLine}{Exception}"))
                .CreateLogger();

            //Log.Logger = new LoggerConfiguration()
            //   .MinimumLevel.Debug()
            //   .Enrich.FromLogContext()
            //   .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(@"https://119e1893f12a35c1b2a0ebb1a7243151.us-east-1.aws.found.io:9243"))
            //   {
            //       IndexDecider = (@event, offset) =>
            //     {
            //         return ".testIndex";
            //     }
            //   })
            //   .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DependencyInjectionConfiguration(services);

            // Add framework services.
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                            .AllowAnyMethod()
                                                                            .AllowAnyHeader()));
            services.AddMvc();
        }

        private void DependencyInjectionConfiguration(IServiceCollection services)
        {
            services.AddTransient<IDeviceEventWriteService, DeviceEventWriteService>();
            services.AddTransient<IDeviceEventReadService, DeviceEventReadService>();

            services.AddTransient<IDeviceEventDao, DeviceEventDao>();

            services.AddSingleton(MongoDatabaseFactory);
            services.AddSingleton<IMapper>(factory => _mapperConfiguration.CreateMapper());
        }

        private IMongoDatabase MongoDatabaseFactory(IServiceProvider serviceProvider)
        {
            return null; //TODO :: REMOVE
            MongoClient client = new MongoClient(Configuration.GetConnectionString("DefaultConnection"));
            return client.GetDatabase(Configuration["DeviceEventDatabase:Name"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, new SwaggerUiOwinSettings
                {
                    DefaultPropertyNameHandling = PropertyNameHandling.CamelCase
                });
                app.UseCors("AllowAll");
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();
        }
    }
}
