using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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
using ECommerce.WEB.EcommerceHttpClient;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using System.IO;

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
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddTransient<IECommerceHttpClient, ECommerceHttpClient>();
            services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(20));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(t=>t.LoginPath="/account/Login");
        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            CookieAuthenticationOptions options = new
            CookieAuthenticationOptions();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            loggerFactory.AddFile(Directory.GetCurrentDirectory() + "/Logs/log-{Date}.log");
        }
    }
}
