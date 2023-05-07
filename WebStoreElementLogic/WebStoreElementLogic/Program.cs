using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebStoreElementLogic.Data;
using WebStoreElementLogic.Hubs;
using WebStoreElementLogic.Interfaces;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using AppContext = WebStoreElementLogic.Data.AppContext;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<EManagerHub>("/EManagerHub");
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });

        app.Run();
    }
}