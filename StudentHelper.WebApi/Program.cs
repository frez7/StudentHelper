using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using StudentHelper.Model.Models.Configs;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Entities.RoleEntities;
using StudentHelper.WebApi.Controllers;
using StudentHelper.WebApi.Data;
using StudentHelper.WebApi.Service;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(provider =>
    builder.Configuration.GetSection("SMTPConfig").Get<SMTPConfig>());


builder.Services.AddDbContext<IdentityContext>();

builder.Services.AddTransient<EmailService>();



builder.Services.AddIdentity<User, AdminRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();



builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/api/account/login";
    options.LogoutPath = "/api/account/logout";
    options.AccessDeniedPath = "/api/account/accessdenied";
});

builder.Services.AddControllers();

builder.Services.AddScoped<AuthController>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0.0",
        Title = "StudentHelper API",
        Description = "We don't have description))",
    });
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

Config? BindConfiguration(IServiceProvider provider)
{
    var envName = builder.Environment.EnvironmentName;

    var config = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.json")
        .Build();

    var configService = config.Get<Config>();
    return configService;
}


