using LeadManagement.API.Middleware;
using LeadManagement.Application.Interfaces.Repositories;
using LeadManagement.Application.Interfaces.Repositories.Enquiry;
using LeadManagement.Application.Interfaces.Repositories.EnquiryFollowup;
using LeadManagement.Application.Interfaces.Repositories.Lead;
using LeadManagement.Application.Interfaces.Repositories.TrainingCourse;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Application.Interfaces.Services.Enquiry;
using LeadManagement.Application.Services;
using LeadManagement.Application.Services.Enquiry;
using LeadManagement.Application.Settings;
using LeadManagement.Infrastructure.Repositories;
using LeadManagement.Infrastructure.Repositories.Enquiry;
using LeadManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// =========================
// Serilog
// =========================

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/leadmanagement-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


// =========================
// Controllers
// =========================

builder.Services.AddControllers();

// =========================
// Swagger
// =========================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
       
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] =
                new List<string>()
        });
});

// =========================
// Repository Registration
// =========================

builder.Services.AddScoped<ILeadRepository, LeadRepository>();

builder.Services.AddScoped<
    ITrainingCourseRepository,
    TrainingCourseRepository>();

builder.Services.AddScoped<
    IEnquiryFollowupRepository,
    EnquiryFollowupRepository>();



// =========================
// Service Registration
// =========================

builder.Services.AddScoped<ILeadService, LeadService>();

builder.Services.AddScoped<
    ITrainingCourseService,
    TrainingCourseService>();

builder.Services.AddScoped<
    IEnquiryFollowupService,
    EnquiryFollowupService>();




// =========================
// JWT Settings
// =========================

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

if (jwtSettings == null ||
    string.IsNullOrWhiteSpace(jwtSettings.Key))
{
    throw new InvalidOperationException(
        "JWT settings are not configured.");
}


// =========================
// JWT Service
// =========================

builder.Services.AddScoped<IJwtService, JwtService>();


// =========================
// Authentication
// =========================

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.Key)),

                ValidateIssuer = true,

                ValidIssuer =
                    jwtSettings.Issuer,

                ValidateAudience = true,

                ValidAudience =
                    jwtSettings.Audience,

                ValidateLifetime = true,

                ClockSkew =
                    TimeSpan.Zero
            };
    });


// =========================
// Authorization
// =========================

builder.Services.AddAuthorization();


// =========================
// Database
// =========================

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString(
//            "DefaultConnection")
//    ));


// =========================
// Build Application
// =========================

var app = builder.Build();


// =========================
// Swagger
// =========================

app.UseSwagger();

app.UseSwaggerUI();


// =========================
// Exception Middleware
// =========================

app.UseMiddleware<ExceptionHandlingMiddleware>();


// =========================
// HTTPS
// =========================

app.UseHttpsRedirection();


// =========================
// Authentication
// =========================

app.UseAuthentication();


// =========================
// Authorization
// =========================

app.UseAuthorization();


// =========================
// Controllers
// =========================

app.MapControllers();


// =========================
// Run
// =========================

app.Run();