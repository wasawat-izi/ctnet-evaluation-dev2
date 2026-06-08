using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using backend.Data;
using backend.Repositories;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using backend.Services;
using System.Threading.RateLimiting;

var envPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", ".env"));

if (File.Exists(envPath))
{
    Env.Load(envPath);
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("LoginPolicy", context =>
    {
       var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
       return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ => new FixedWindowRateLimiterOptions
       {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(5),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0 
       });
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "Too many login attempts. Try again later.",
            retryAfter = "5 minutes"
        });
    };
});

builder.Services.AddControllers();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

var server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "database";
var uid = Environment.GetEnvironmentVariable("DB_UID") ?? "root";
var pwd = Environment.GetEnvironmentVariable("DB_PWD") ?? "1234";
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";

var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "http://localhost:5246";
var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "http://localhost:5246";
var signingKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "password";

var connectionString = $"Server={server};port={port};Database={database};Uid={uid};Pwd={pwd}";

builder.Services.AddIdentity<AppUser, IdentityRole>( options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(options =>{
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme = 
    options.DefaultForbidScheme = 
    options.DefaultScheme = 
    options.DefaultSignInScheme = 
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(signingKey)
        )
    };
});

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseMySQL(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
