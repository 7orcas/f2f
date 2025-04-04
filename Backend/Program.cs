using System.Text;
using Backend.App.Login;
using Backend.App.Token;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens; // For TokenValidationParameters


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourIssuer",
            ValidAudience = "yourAudience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("S3cureK3yWith128B1tsOrMore!"))
        };
    });


builder.Services.AddScoped<LoginServiceI, LoginService>();
builder.Services.AddScoped<TokenServiceI, TokenService>();

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
