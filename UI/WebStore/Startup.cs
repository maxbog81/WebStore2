using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebStore.Clients.Employees;
using WebStore.Clients.Identity;
using WebStore.Clients.Orders;
using WebStore.Clients.Products;
using WebStore.Clients.Values;
using WebStore.Domain.Entities.Identity;
using WebStore.Hubs;
using WebStore.Infrastructure.AutoMapper;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Api;
using WebStore.Interfaces.Services;
using WebStore.Logger;
using WebStore.Services.Product;

namespace WebStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration Config) => Configuration = Config;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            services.AddAutoMapper(opt =>
            {
                opt.AddProfile<ViewModelMapping>();
                opt.AddProfile<DTOMapping>();
            }, typeof(Startup)/*.Assembly*/);

            services.AddSingleton<IEmployeesData, EmployeesClient>();
            services.AddScoped<IProductData, ProductsClient>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartStore, CookiesCartStore>();
            services.AddScoped<IOrderService, OrdersClient>();

            services.AddScoped<IValuesService, ValuesClient>();

            services.AddIdentity<User, Role>()
               .AddDefaultTokenProviders();

            #region Custom implementation identity storages

            services.AddTransient<IUserStore<User>, UsersClient>();
            services.AddTransient<IUserPasswordStore<User>, UsersClient>();
            services.AddTransient<IUserEmailStore<User>, UsersClient>();
            services.AddTransient<IUserPhoneNumberStore<User>, UsersClient>();
            services.AddTransient<IUserTwoFactorStore<User>, UsersClient>();
            services.AddTransient<IUserLockoutStore<User>, UsersClient>();
            services.AddTransient<IUserClaimStore<User>, UsersClient>();
            services.AddTransient<IUserLoginStore<User>, UsersClient>();

            services.AddTransient<IRoleStore<Role>, RolesClient>();

            #endregion

            services.Configure<IdentityOptions>(
                opt =>
                {
                    opt.Password.RequiredLength = 3;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequiredUniqueChars = 3;

                    opt.Lockout.AllowedForNewUsers = true;
                    opt.Lockout.MaxFailedAccessAttempts = 10;
                    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);

                    //opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABC123";
                    opt.User.RequireUniqueEmail = false; // Грабли - на этапе отладки при попытке регистрации двух пользователей без email
                });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore-Identity";
                opt.Cookie.HttpOnly = true;
                opt.Cookie.Expiration = TimeSpan.FromDays(150);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            //services.AddSession();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            log.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            //app.UseSession();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseSignalR(route =>
            {
                route.MapHub<InformationHub>("/info");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
