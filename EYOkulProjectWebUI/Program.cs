using EYOkulProjectWebUI.DAL;
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

// Servislerin konteynýra eklenmesi
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EYOkulDbContext>();
builder.Services.AddSignalR();
builder.Services.AddSession();
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Index"; 
        options.LogoutPath = "/Home/Logout";
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
    });






// Servislerin konteynýra eklenmesi
builder.Services.AddSingleton<Service_Transaction<TransactionsModel>>();

var app = builder.Build();

// Middleware'lerin kullanýlmasý
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseDatabaseSubsription<Service_Transaction<TransactionsModel>>("TBL_TRANSACTIONS"); // Fixed typo in "Subscription"

// Endpoint'lerin tanýmlanmasý
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TransactionHub>("/TransactionHub");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// HTTP isteði pipeline'ýnýn yapýlandýrýlmasý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Uygulamanýn çalýþtýrýlmasý
app.Run();
