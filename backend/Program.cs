using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using backend.Data;
using backend.Repositories;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddControllers();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
