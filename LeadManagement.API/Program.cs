using LeadManagement.API.Middleware;
using LeadManagement.Application.Interfaces;
using LeadManagement.Application.Interfaces.Repositories;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Application.Services;
using LeadManagement.Infrastructure.Repositories;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/leadmanagement-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddScoped<ITrainingCourseRepository, TrainingCourseRepository>();
builder.Services.AddScoped<IEnquiryFollowupRepository, EnquiryFollowupRepository>();

builder.Services.AddScoped<ILeadService, LeadService>();
builder.Services.AddScoped<ITrainingCourseService, TrainingCourseService>();
builder.Services.AddScoped<IEnquiryFollowupService, EnquiryFollowupService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();