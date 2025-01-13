using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.API.Utils;
using Modsen.TestProject.Application.Contracts;
using Modsen.TestProject.Application.Mappings;
using Modsen.TestProject.Application.UseCases;
using Modsen.TestProject.Application.Validators;
using Modsen.TestProject.DAL;
using Modsen.TestProject.DAL.Repositories;
using Modsen.TestProject.DAL.UnitOfWork;
using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;
using Modsen.TestProject.Domain.UnitOfWork;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddDbContext<ProjectDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ProjectDbContext")));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<INewEventsRepository, NewEventsRepository>();
        services.AddScoped<IParticipantsRepository, ParticipantsRepository>();
        services.AddScoped<IImageService, ImageService>();

        services.AddScoped<IValidator<NewEvent>, NewEventValidator>();
        services.AddScoped<IValidator<ParticipantRequest>, ParticipantValidator>();

        services.AddScoped<CreateNewEventUseCase>();
        services.AddScoped<CreateParticipantUseCase>();
        services.AddScoped<DeleteNewEventUseCase>();
        services.AddScoped<DeleteParticipantUseCase>();
        services.AddScoped<GetAllNewEventsUseCase>();
        services.AddScoped<GetAllParticipantsUseCase>();
        services.AddScoped<GetNewEventByIdUseCase>();
        services.AddScoped<GetNewEventByNameUseCase>();
        services.AddScoped<GetParticipantByIdUseCase>();
        services.AddScoped<UpdateEventImagePathUseCase>();
        services.AddScoped<UpdateNewEventUseCase>();
        services.AddScoped<UpdateParticipantUseCase>();

        services.AddAutoMapper(typeof(NewEventMappingProfile).Assembly);

        return services;
    }
}
