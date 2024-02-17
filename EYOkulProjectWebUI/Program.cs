using EYOkulProjectWebUI.Hubs;
using EYOkulProjectWebUI.Models;
using EYOkulProjectWebUI.Subscription.Concreate;
using EYOkulProjectWebUI.Subscription.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

// Servislerin konteyn�ra eklenmesi
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Index"; // De�i�tirebilirsiniz
        options.LogoutPath = "/Account/Logout"; // De�i�tirebilirsiniz
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    });






// Servislerin konteyn�ra eklenmesi
builder.Services.AddSingleton<Service_Transaction<TransactionsModel>>();

var app = builder.Build();

// Middleware'lerin kullan�lmas�
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseDatabaseSubsription<Service_Transaction<TransactionsModel>>("TBL_TRANSACTIONS"); // Fixed typo in "Subscription"

// Endpoint'lerin tan�mlanmas�
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TransactionHub>("/TransactionHub");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// HTTP iste�i pipeline'�n�n yap�land�r�lmas�
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Uygulaman�n �al��t�r�lmas�
app.Run();
