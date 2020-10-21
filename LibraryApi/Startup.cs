using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Confluent.Kafka;
using LibraryApi.Domain;
using LibraryApi.Profiles;
using LibraryApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LibraryApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        } 
           
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddTransient<ISystemTime, SystemTime>(); // create a brand new instance any time it's needed.
            // services.AddScoped<ISystemTime, SystemTime>(); // create exactly one of these PER REQUEST.
            // services.AddSingleton<ISystemTime, SystemTime>(); // Create exactly one of these and share it like a cheap bottle of wine with anybody that needs it.

            services.AddDbContext<LibraryDataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("library"));
            });

            var mapperConfig = new MapperConfiguration(opt =>
            {
                opt.AddProfile<BooksProfile>();
                opt.AddProfile<ReservationsProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton<IMapper>(mapper);
            services.AddSingleton<MapperConfiguration>(mapperConfig);

            services.AddScoped<IQueryForBooks, EfLibraryData>();
            services.AddScoped<IDoBookCommands, EfLibraryData>();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("redis");
            });
            services.AddTransient<ICacheTheCatalog, CatalogService>();

            var producerConfig = new ProducerConfig();
            Configuration.Bind("kafka", producerConfig);
            //services.AddOptions<ProducerConfig>(producerConfig); // TODO: an options example.
            services.AddSingleton<ProducerConfig>(producerConfig);
            services.AddScoped<ILogReservations, KafkaReservationProducer>();


            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Library API For BES 100",
                    Version = "1.0",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Jeff Gonzalez",
                        Email = "jeff@hypertheory.com"
                    },
                    Description = "For the BES 100 Class."
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API");
                c.RoutePrefix = "";
            });
        }
    }
}
