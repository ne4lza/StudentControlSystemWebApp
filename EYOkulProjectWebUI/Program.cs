using EYOkulProjectWebUI.Hubs;
using EYOkulProjectWebUI.Models;
using EYOkulProjectWebUI.Subscription.Concreate;
using EYOkulProjectWebUI.Subscription.Middleware;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        PositionClass = ToastPositions.TopLeft,
        TimeOut = 5000,
        ProgressBar = true,
        CloseButton = true
    });
builder.Services.AddSignalR();
    
builder.Services.AddSingleton<Service_Transaction<TransactionsModel>>();
var app = builder.Build();

app.UseRouting(); // EndpointRoutingMiddleware'yi burada �a��r�n

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TransactionHub>("/TransactionHub"); // SignalR hub'�n� burada yap�land�r�n
                                                         // Di�er son noktalar� burada yap�land�r�n
});


app.UseDatabaseSubsription<Service_Transaction<TransactionsModel>>("TBL_TRANSACTIONS");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseNToastNotify();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
