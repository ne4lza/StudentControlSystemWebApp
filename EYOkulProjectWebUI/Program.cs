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

app.UseRouting(); // EndpointRoutingMiddleware'yi burada çaðýrýn

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TransactionHub>("/TransactionHub"); // SignalR hub'ýný burada yapýlandýrýn
                                                         // Diðer son noktalarý burada yapýlandýrýn
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
