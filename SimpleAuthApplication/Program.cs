using FluentScheduler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleAuthApplication.Data;
using SimpleAuthApplication.Hubs;
using SimpleAuthApplication.Jobs;
using SimpleAuthApplication.Jwt;
using SimpleAuthApplication.Repositories;
using SimpleAuthApplication.Services;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT
builder.Services.AddScoped<IJwtTokenGenerator>(provider =>
    new JwtTokenGenerator(
        builder.Configuration["Jwt:Key"],
        builder.Configuration["Jwt:Issuer"],
        builder.Configuration["Jwt:Audience"],
        int.Parse(builder.Configuration["Jwt:ExpiryMinutes"])));

// Repositories, Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// SignalR
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Pobieramy IServiceScopeFactory i przekazujemy do FluentScheduler
var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

// Inicjalizacja JobManager z przekazaniem scope factory
JobManager.Initialize(new CurrencyRateRegistry(serviceScopeFactory));
Console.WriteLine("JobManager initialized."); // LOG

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<UserActivityHub>("/userActivityHub");

app.Run();
