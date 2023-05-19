using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebStoreElementLogic.Data;
using WebStoreElementLogic.Hubs;
using WebStoreElementLogic.Interfaces;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using WebStoreElementLogic.Account;
using WebStoreElementLogic.Shared;
using AppCtx = WebStoreElementLogic.Data.AppContext;
using Microsoft.JSInterop;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebStoreElementLogic API", Version = "v1" });
});


// Add distributed memory cache
builder.Services.AddDistributedMemoryCache();

// Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppCtx>(options => options.UseSqlServer(connectionString));

// Services
builder.Services.AddTransient<IStatusService, StatusService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IDapperService, DapperService>();
builder.Services.AddTransient<IInboundService, InboundService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<ISimpleCookieManager, SimpleCookieManager>();
builder.Services.AddTransient<IStockService, StockService>();

// Session and authentication
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<WebStoreElementLogic.Data.AppContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(2); // Set cookie expiration to 2 hours

    });

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

// Add session middleware configuration
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
});

// Add Scoped services
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<IEManagerService, EManagerService>();
builder.Services.AddScoped<IDapperService, DapperService>();

// WebHost enviornment for images
builder.Services.AddSingleton<ICustomWebHostEnvironment>(s =>
{
    var httpContextAccessor = s.GetRequiredService<IHttpContextAccessor>();
    var hostingEnvironment = s.GetService<IWebHostEnvironment>();
    return new CustomWebHostEnvironment(httpContextAccessor, hostingEnvironment);
});

// Configure Kestrel
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





// Build app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Add Extentsions
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebStore API v1");
});
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<EManagerHub>("/EManagerHub");
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        string ipAddress = "88.94.88.30"; // Replace with your desired IP address
        int port = 5000; // Replace with your desired port number

        string loginUrl = $"http://{ipAddress}:{port}/login";
        context.Response.Redirect(loginUrl);
        return;
    }
    await next.Invoke();
});

// Run the app
app.Run();


/*
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebStoreElementLogic.Data;
using WebStoreElementLogic.Hubs;
using WebStoreElementLogic.Interfaces;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStoreElementLogic.Account;
using WebStoreElementLogic.Shared;
using AppContext = WebStoreElementLogic.Data.AppContext;

namespace MyProgram
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSignalR();
            services.AddControllers();

            // Add distributed memory cache
            services.AddDistributedMemoryCache();

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDapperService, DapperService>();
            services.AddTransient<IInboundService, InboundService>();
            services.AddTransient<IOrderService, OrderService>();

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
            //services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.example.com") });
            services.AddScoped<HttpClient>();
            services.AddScoped<IEManagerService, EManagerService>();
            services.AddScoped<IDapperService, DapperService>();


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
                endpoints.MapControllers();
                endpoints.MapHub<EManagerHub>("/EManagerHub");
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Configure the request pipeline
            startup.Configure(app, app.Environment);

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    string ipAddress = "193.69.50.119"; // Replace with your desired IP address
                    int port = 90; // Replace with your desired port number

                    string loginUrl = $"http://{ipAddress}:{port}/login";
                    context.Response.Redirect(loginUrl);
                    return;
                }
                await next.Invoke();
            });


            app.Run();
        }
    }
}*/