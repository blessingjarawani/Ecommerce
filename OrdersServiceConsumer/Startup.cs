using AutoMapper;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Services;
using BoookStoreDatabase2.DAL.Context;
using BoookStoreDatabase2.DAL.Mappers;
using BoookStoreDatabase2.DAL.Repositories;
using Ecommerce.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using Ecommerce.BLL.Infrastructure.Shared.Services;
using Ecommerce.DAL.Repositories;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersServiceConsumer
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
            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {

                    cfg.Host("rabbitmq://localhost");

                    cfg.ReceiveEndpoint("orders_queue", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(t => t.Interval(2, 100));

                        ep.ConfigureConsumer<OrderConsumer>(provider);
                    });
                }));
            });

            ConfigureDatabase(services);
            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddTransient<ICustomerOrderService, CustomerOrderService>();
            services.AddTransient<ICustomerOrderRepository, CustomerOrderRepository>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<ICartService, CartService>();
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(),
                            AppDomain.CurrentDomain.GetAssemblies());

            services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrdersServiceConsumer", Version = "v1" });
            });
        }
        public void ConfigureDatabase(IServiceCollection services)
        {

            services.AddDbContextPool<StoreContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("StoreContext")));
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrdersServiceConsumer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
