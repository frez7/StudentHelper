using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentHelper.BL.Logging;
using StudentHelper.BL.Services;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.BL.Services.OtherServices;
using StudentHelper.BL.Services.SellerServices;
using StudentHelper.Model.Data;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Extensions;
using StudentHelper.Model.Models.Common.Other;
using StudentHelper.Model.Models.Configs;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.WebApi.Data;
using StudentHelper.WebApi.Managers;
using StudentHelper.WebApi.Service;
using System.Text;
using MediatR;
using StudentHelper.BL.Handlers.CourseHandlers.CourseHandling;
using StudentHelper.BL.Handlers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(provider =>
    builder.Configuration.GetSection("SMTPConfig").Get<SMTPConfig>());


builder.Services.AddDbContext<IdentityContext>();
builder.Services.AddDbContext<CourseContext>();


builder.Services.AddSingleton<DbLogger>();
builder.Services.AddSingleton<DbLoggerProvider>();
builder.Services.AddLogging();
builder.Services.AddSingleton<ILoggerProvider>(provider => provider.GetService<DbLoggerProvider>());
builder.Logging.AddDbLogger(options =>
{
    builder.Configuration.GetSection("Logging")
    .GetSection("Database").GetSection("Options").Bind(options);
});

builder.Services.AddTransient<EmailService>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<IRepository<Log>, Repository<Log>>();
builder.Services.AddTransient<IRepository<Student>, Repository<Student>>();
builder.Services.AddTransient<IRepository<Course>, Repository<Course>>();
builder.Services.AddTransient<IRepository<SellerApplication>, Repository<SellerApplication>>();
builder.Services.AddTransient<IRepository<Seller>, Repository<Seller>>();
builder.Services.AddTransient<IRepository<StudentCourse>, Repository<StudentCourse>>();
builder.Services.AddTransient<IRepository<Page>, Repository<Page>>();
builder.Services.AddTransient<IRepository<VideoLesson>,  Repository<VideoLesson>>();
builder.Services.AddTransient<IRepository<Enrollment>, Repository<Enrollment>>();
builder.Services.AddTransient<IRepository<Test>, Repository<Test>>();
builder.Services.AddTransient<IRepository<Question>, Repository<Question>>();
builder.Services.AddTransient<IRepository<Answer>, Repository<Answer>>();
builder.Services.AddTransient<PageService>();
builder.Services.AddTransient<CourseService>();
builder.Services.AddTransient<VideoService>();
builder.Services.AddTransient<StudentService>();
builder.Services.AddTransient<SellerService>();
builder.Services.AddTransient<ProfileService>();
builder.Services.AddTransient<SellerApplicationService>();
builder.Services.AddTransient<EnrollmentService>();
builder.Services.AddTransient<AuthManager>();
builder.Services.AddTransient<AdminManager>();
builder.Services.AddTransient<TestService>();
builder.Services.AddTransient<QuestionService>();
builder.Services.AddTransient<AnswerService>();
builder.Services.AddTransient<ValidationService>();
builder.Services.AddTransient<GetService>();

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();


builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<RoleManager<IdentityRole<int>>>();

    

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0.0",
        Title = "StudentHelper API",
        Description = "We don't have description))",
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", 
        builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"]!,
            ValidAudience = builder.Configuration["Jwt:Audience"]!,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy =
    new AuthorizationPolicyBuilder
            (JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentHelper");
});

app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();


app.Run();

var host = Host.CreateDefaultBuilder(args).Build();
var serviceProvider = host.Services;

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        UserRoleInitializer.InitializeAsync(services).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while attempting to seed the database");
    }
}
Config? BindConfiguration(IServiceProvider provider)
{
    var envName = builder.Environment.EnvironmentName;

    var config = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.json")
        .Build();

    var configService = config.Get<Config>();
    return configService;
}


