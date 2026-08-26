using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using StudentManagement.API.Middleware;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Application.Interfaces.Repositories.Password;
using StudentManagement.Application.Interfaces.Repositories.Payment;
using StudentManagement.Application.Interfaces.Repositories.Profile;
using StudentManagement.Application.Interfaces.Repositories.Qualification;
using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Application.Interfaces.Repositories.Student;
using StudentManagement.Application.Interfaces.Services;
using StudentManagement.Application.Interfaces.Services.Login;
using StudentManagement.Application.Interfaces.Services.Password;
using StudentManagement.Application.Interfaces.Services.Payment;
using StudentManagement.Application.Interfaces.Services.Qualification;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Application.Interfaces.Services.Student;
using StudentManagement.Application.Interfaces.Services.StudentProfile;
using StudentManagement.Application.Services;
using StudentManagement.Application.Services.Login;
using StudentManagement.Application.Services.Payment;
using StudentManagement.Application.Services.Profile;
using StudentManagement.Application.Services.Qualification;
using StudentManagement.Application.Services.Registration;
using StudentManagement.Application.Services.Student;
using StudentManagement.Infrastructure.Data;
using StudentManagement.Infrastructure.Email;
using StudentManagement.Infrastructure.Repositories;
using StudentManagement.Infrastructure.Repositories.Payment;
using StudentManagement.Infrastructure.Repositories.Profile;
using StudentManagement.Infrastructure.Repositories.Qualification;
using StudentManagement.Infrastructure.Repositories.Registration;
using StudentManagement.Infrastructure.Repositories.Student;
using System.Text;
using StudentManagement.Application.Services.Password;
using StudentManagement.Infrastructure.Repositories.Password;



var builder = WebApplication.CreateBuilder(args);

var jwtSettings =
    builder.Configuration.GetSection("Jwt");

var jwtKey =
    jwtSettings["Key"]
    ?? throw new Exception("JWT Key is not configured.");

var jwtIssuer =
    jwtSettings["Issuer"];

var jwtAudience =
    jwtSettings["Audience"];

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,

                ValidAudience = jwtAudience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
            };
    });

builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>  options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
//Repository
builder.Services.AddScoped<IStudentDetailsRepository, StudentDetailsRepository>();
builder.Services.AddScoped<IStudentRegistrationRepository, StudentRegistrationRepository>();
builder.Services.AddScoped<IStudentPaymentRepository, StudentPaymentRepository>();
builder.Services.AddScoped<IStudentQualificationRepository, StudentQualificationRepository>();
builder.Services.AddScoped< IStudentDetailsRepository,StudentDetailsRepository>();
builder.Services.AddScoped<IStudentProfileRepository, StudentProfileRepository>();
builder.Services.AddScoped<IStudentPasswordRepository, StudentPasswordRepository>();


//Service
builder.Services.AddScoped<IStudentDetailsService, StudentDetailsService>();
builder.Services.AddScoped<IStudentRegistrationService,StudentRegistrationService>();
builder.Services.AddScoped<IStudentPaymentService,StudentPaymentService>();
builder.Services.AddScoped<IStudentQualificationService, StudentQualificationService>();
builder.Services.AddScoped<IStudentLoginService, StudentLoginService>();
builder.Services.AddScoped< IStudentProfileService,StudentProfileService>();
builder.Services.AddScoped<IStudentPasswordService,StudentPasswordService>();

builder.Services.AddScoped<IJwtService, JwtService>();

// Email

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

//builder.Services.AddScoped<IEmailService>(serviceProvider =>
//    new EmailService(
//        "smtp.gmail.com",
//        587,
//        "minakshigaike@gmail.com",
//        "mvkh bzig aqej fibu"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT Bearer token"
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] =
                []
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
