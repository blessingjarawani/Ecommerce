using AutoMapper;
using Book_Store_.Net_Core.Api.Helpers;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Config;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Services;
using BoookStoreDatabase2.DAL.Context;
using BoookStoreDatabase2.DAL.Entities;
using BoookStoreDatabase2.DAL.Mappers;
using BoookStoreDatabase2.DAL.Repositories;
using BoookStoreDatabase2.DAL.SeedData;
using Ecommerce.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using Ecommerce.BLL.Infrastructure.Shared.Services;
using Ecommerce.BLL.Infrastructure.Shared.Services.Email;
using Ecommerce.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_.Net_Core.Api
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
            ConfigureDatabase(services);
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<StoreContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<ICustomerOrderService, CustomerOrderService>();
            services.AddTransient<IApplicationUsersRepository, ApplicationUsersRepository>();
            services.AddTransient<ICustomerOrderRepository, CustomerOrderRepository>();
            services.AddTransient<IApplicationUsersService, ApplicationUsersService>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<ICartService, CartService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(),
                             AppDomain.CurrentDomain.GetAssemblies());
            var emailConfig = Configuration
               .GetSection("EmailSenderConfig")
               .Get<EmailSenderConfig>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IMailService, MailService>();
            services.AddControllers();
            AddSwagger(services);
            var authConfig = new AppSettings();
            Configuration.GetSection("AppSettings").Bind(authConfig);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authConfig.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        }

        public void ConfigureDatabase(IServiceCollection services)
        {

            services.AddDbContextPool<StoreContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("StoreContext")));
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce.Api", Version = "v1" });
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager,
                 RoleManager<IdentityRole> roleManager, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce.Api v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //DatabaseInitiliaser.SeedData(userManager, roleManager);
            app.UseMiddleware<JwtMiddleWare>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            loggerFactory.AddFile(Directory.GetCurrentDirectory() + "/Logs/log-{Date}.log");
        }
    }
}
