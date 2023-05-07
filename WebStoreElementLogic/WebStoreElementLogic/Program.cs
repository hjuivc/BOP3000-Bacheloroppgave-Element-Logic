using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStoreElementLogic.Data;
using WebStoreElementLogic.Hubs;
using WebStoreElementLogic.Interfaces;
using WebStoreElementLogic.Account;
using WebStoreElementLogic.Shared;

using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using AppContext = WebStoreElementLogic.Data.AppContext;

namespace MyNamespace
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSignalR();
        builder.Services.AddControllers();

        builder.Services.AddScoped<HttpClient>();
        builder.Services.AddScoped<IEManagerService, EManagerService>();

        builder.Services.AddTransient<IProductService, ProductService>();
        builder.Services.AddTransient<DapperService>();

        builder.Services.AddTransient<IInboundService, InboundService>();
        builder.Services.AddTransient<IOrderService, OrderService>();

        builder.Services.AddTransient<IDbConnection>(x => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IDapperService, DapperService>();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<AppContext>(options => options.UseSqlServer(connectionString));
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 5000, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            });
            options.Listen(IPAddress.Any, 7001, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                listenOptions.UseHttps();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add distributed memory cache
            services.AddDistributedMemoryCache();
        //app.UseHttpsRedirection();
        app.UseStaticFiles();

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDapperService, DapperService>();

            services.AddDbContext<WebStoreElementLogic.Data.AppContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<WebStoreElementLogic.Data.AppContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/logout";
                });

            services.AddScoped<CustomAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

            // Add session middleware configuration
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
            });

            // Add HttpClient service
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.example.com") });
        }





        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<EManagerHub>("/EManagerHub");
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add the Startup class
            builder.Services.AddTransient<Startup>();

            // Add the ConfigureServices method from the Startup class
            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            // Configure the request pipeline
            using (var scope = app.Services.CreateScope())
            {
                var env = app.Environment;
                startup.Configure(app, env);
            }

            app.Run();
        }


    }
}
