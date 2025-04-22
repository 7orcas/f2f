using FrontendServer.Data;
using FrontendServer.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

//ToDo is the ConfigurePrimaryHttpMessageHandler required?
builder.Services.AddHttpClient("BackendApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:6001/"); // Adjust base URL to your backend
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.UseCookies = true; // Enable cookies
    handler.CookieContainer = new CookieContainer(); // Cookie container for managing cookies
    return handler;
});

builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:6001/"); // Adjust base URL to your backend
}).AddHttpMessageHandler<AuthorizationMessageHandler>();


// Add session services
//builder.Services.AddRazorComponents();
builder.Services.AddSession(); 
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddScoped<SessionService>();


builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<SessionTokenStorageService>();
builder.Services.AddScoped<AuthorizationMessageHandler>();
builder.Services.AddScoped<PermissionService>();

builder.Services.AddScoped<MachineService>();


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
//app.UseSession(); // Add session services


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
