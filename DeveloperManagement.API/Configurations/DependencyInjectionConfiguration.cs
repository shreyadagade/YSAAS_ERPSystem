using DeveloperManagement.Application.Interfaces;
using DeveloperManagement.Application.Services;
using DeveloperManagement.Infrastructure.Persistence.Context;
using DeveloperManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeveloperManagement.API.Configurations;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DeveloperDbContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IGenericRepository, GenericRepository>();

        services.AddScoped<ITrainingTopicService, TrainingTopicService>();

        services.AddScoped<ITrainingCourseService, TrainingCourseService>();

        services.AddScoped<ITrainingTopicContentService, TrainingTopicContentService>();

        services.AddScoped<ITrainingCourseTopicService,TrainingCourseTopicService>();

        services.AddScoped<ITrainingContentProgramQuestionService,TrainingContentProgramQuestionService>();

        services.AddScoped<ITrainingContentProgramAnswerService,
            TrainingContentProgramAnswerService>();

        services.AddScoped<ITrainingContentQuestionService,TrainingContentQuestionService>();

        services.AddScoped<ITrainingContentInterviewQuestionService,TrainingContentInterviewQuestionService>();

        return services;
    }
}