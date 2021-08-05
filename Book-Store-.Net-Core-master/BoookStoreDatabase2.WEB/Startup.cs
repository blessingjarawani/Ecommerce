using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BoookStoreDatabase2.DAL.Context;
using BoookStoreDatabase2.DAL.Entities;
using BoookStoreDatabase2.DAL.SeedData;
using Microsoft.AspNetCore.Http;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.DAL.Repositories;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Services;
using AutoMapper;
using BoookStoreDatabase2.DAL.Mappers;

namespace BoookStoreDatabase2.WEB
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
            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IApplicationUsersRepository, ApplicationUsersRepository>();
            services.AddTransient<IApplicationUsersService, ApplicationUsersService>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<ICartService, CartService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(),
                             AppDomain.CurrentDomain.GetAssemblies());
        }



        public void ConfigureDatabase(IServiceCollection services)
        {

            services.AddDbContextPool<StoreContext>(options =>
             options.UseNpgsql(Configuration.GetConnectionString("StoreContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager,
                 RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            DatabaseInitiliaser.SeedData(userManager, roleManager);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
