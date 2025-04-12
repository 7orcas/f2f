using System.Security.Cryptography;
using System.Text;
using Backend.App.Machines;
using Backend.Base.Token.Ent;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens; // For TokenValidationParameters


var builder = WebApplication.CreateBuilder(args);

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

//Base Services
builder.Services.AddScoped<LoginServiceI, LoginService>();
builder.Services.AddScoped<TokenServiceI, TokenService>();
builder.Services.AddScoped<OrgServiceI, OrgService>();
builder.Services.AddScoped<SessionServiceI, SessionService>();
builder.Services.AddScoped<PermissionServiceI, PermissionService>();

//App Services
builder.Services.AddScoped<MachineServiceI, MachineService>();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseSession();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
