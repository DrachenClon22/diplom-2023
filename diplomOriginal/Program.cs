using diplomOriginal.Controllers;
using diplomOriginal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySqlConnector;
using System;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<MySqlConnection>(_ =>
	new MySqlConnection(builder.Configuration.GetConnectionString("Default")));

Database.InitConnection(builder.Configuration.GetConnectionString("Default")!);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

#region ROUTING
app.MapControllerRoute(
    name: "catalog",
    pattern: "catalog/{catalog?}/{id?}",
    defaults: new { controller = "Catalog", action = "Index" });

app.MapControllerRoute(
    name: "cabinet",
    pattern: "cabinet",
    defaults: new { controller = "UserCabinet", action = "Index" });

app.MapControllerRoute(
    name: "login",
    pattern: "login/{action}",
    defaults: new { controller = "Login", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Map("/hello", [Authorize] () => "Hello World!");
#endregion

DevMode.IsInDevMode = app.Environment.IsDevelopment();

app.Run();

public static class DevMode
{
    public static bool IsInDevMode = false;
}