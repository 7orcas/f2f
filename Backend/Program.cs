using System.Security.Cryptography;
using System.Text;
using Backend;
using Backend.App.Machines;
using Backend.Base.Token.Ent;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens; // For TokenValidationParameters


var builder = WebApplication.CreateBuilder(args);

// Add Serilog configuration
// Set up Serilog to use appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Reads from appsettings.json
    .CreateLogger();

builder.Host.UseSerilog();

LoadAppSettings(builder);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<InterceptorFilter>();
});
//builder.Services.AddScoped<InterceptorFilter>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true; // Prevent client-side access
    options.Cookie.IsEssential = true; // Mark the session cookie as essential
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = TokenParameters.GetParameters());

//Base Services (start up)
builder.Services.AddSingleton<OrgConfigInitialiseServiceI, OrgConfigInitialiseService>();
builder.Services.AddSingleton<PermissionInitialiseServiceI, PermissionInitialiseService>();

//Base Services
builder.Services.AddScoped<AuditServiceI, AuditService>();
builder.Services.AddScoped<LabelServiceI, LabelService>();
builder.Services.AddScoped<ConfigServiceI, ConfigService>();
builder.Services.AddScoped<LoginServiceI, LoginService>();
builder.Services.AddScoped<TokenServiceI, TokenService>();
builder.Services.AddScoped<OrgServiceI, OrgService>();
builder.Services.AddScoped<SessionServiceI, SessionService>();
builder.Services.AddScoped<PermissionServiceI, PermissionService>();
builder.Services.AddScoped<RoleServiceI, RoleService>();
builder.Services.AddScoped<EntityServiceI, EntityService>();


//App Services
builder.Services.AddScoped<MachineServiceI, MachineService>();

var app = builder.Build();

app.UsePathBase(AppSettings.PathBase);

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Use(async (context, next) =>
{
    if (!context.Request.PathBase.Equals(AppSettings.PathBase))
    {
        context.Response.StatusCode = 404; // Not Found
        await context.Response.WriteAsync("Invalid path base.");
        return;
    }
    await next();
});

RunOnStartup(app);
app.Run();



/// <summary>
/// Call the initialisation service methods
/// </summary>
void RunOnStartup(WebApplication app)
{
    var configService = app.Services.GetRequiredService<OrgConfigInitialiseServiceI>();
    configService.InitialiseOrgConfigs();

    var permissionService = app.Services.GetRequiredService<PermissionInitialiseServiceI>();
    permissionService.InitialisePermissions();
}

void LoadAppSettings(WebApplicationBuilder builder)
{
    AppSettings.DBMainConnection = builder.Configuration["ConnectionStrings:DBMainConnection"];
    AppSettings.MaxGetTokenCalls = int.Parse(builder.Configuration["Token:MaxGetTokenCalls"]);
    AppSettings.CacheExpirationAddSeconds = int.Parse(builder.Configuration["Token:CacheExpirationAddSeconds"]);
    AppSettings.CacheExpirationGetSeconds = int.Parse(builder.Configuration["Token:CacheExpirationGetSeconds"]);
    AppSettings.MainClientUrl = builder.Configuration["Urls:MainClientUrl"];
    AppSettings.PathBase = builder.Configuration["PathBase"];

    var _log = Serilog.Log.Logger;
    _log.Information("---------Backend Startup------------" +
        "\nDBMainConnection=" + AppSettings.DBMainConnection +
        "\nMaxGetTokenCalls=" + AppSettings.MaxGetTokenCalls + 
        "\nCacheExpirationAddSeconds=" + AppSettings.CacheExpirationAddSeconds + 
        "\nCacheExpirationGetSeconds=" + AppSettings.CacheExpirationGetSeconds +
        "\nMainClientUrl=" + AppSettings.MainClientUrl +
        "\nPathBase=" + AppSettings.PathBase
        );

}