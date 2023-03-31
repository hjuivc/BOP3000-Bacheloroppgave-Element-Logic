using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebStoreElementLogic.Data;
using WebStoreElementLogic.Interfaces;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<WebStoreElementLogic.Data.AppContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddTransient<IProductService, ProductService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<DapperService>();

        builder.Services.AddTransient<IDbConnection>(x => new SqlConnection(connectionString));

        builder.Services.AddScoped<IDapperService, DapperService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}