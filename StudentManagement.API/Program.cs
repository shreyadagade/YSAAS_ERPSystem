using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Middleware;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Application.Interfaces.Services;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Application.Services;
using StudentManagement.Application.Services.Registration;
using StudentManagement.Infrastructure.Data;
using StudentManagement.Infrastructure.Email;
using StudentManagement.Infrastructure.Repositories;
using StudentManagement.Infrastructure.Repositories.Registration;
using StudentManagement.Infrastructure.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>  options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
//Repository
builder.Services.AddScoped<IStudentDetailsRepository, StudentDetailsRepository>();
builder.Services.AddScoped<IStudentPaymentRepository, StudentPaymentRepository>();
builder.Services.AddScoped<IStudentQualificationRepository, StudentQualificationRepository>();
builder.Services.AddScoped<IStudentRegistrationRepository, StudentRegistrationRepository>();
//Service
builder.Services.AddScoped<IStudentPaymentService, StudentPaymentService>();
builder.Services.AddScoped<IStudentDetailsService, StudentDetailsService>();
builder.Services.AddScoped<IStudentQualificationService, StudentQualificationService>();
builder.Services.AddScoped<IStudentRegistrationService, StudentRegistrationService>();

// Email

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService>(serviceProvider =>
    new EmailService(
        "smtp.gmail.com",
        587,
        "minakshigaike@gmail.com",
        "yudr utnl mkbq tyhn"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
