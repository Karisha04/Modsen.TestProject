using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using Modsen.TestProject.Application.Mappings;
using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.Application.Services;
using Modsen.TestProject.DAL;
using Modsen.TestProject.DAL.Repositories;
using Modsen.TestProject.Domain.Abstractions;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Modsen.TestProject.DAL.UnitOfWork;
using Modsen.TestProject.API.Middleware;
using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.UnitOfWork;
using Microsoft.OpenApi.Models;
using Modsen.TestProject.Application.UseCases;
using FluentValidation;
using Modsen.TestProject.Application.Contracts;
using Modsen.TestProject.Application.Validators;
using Modsen.TestProject.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// ���������� ����������� �������� ��� Controllers, Swagger � ������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "������� 'Bearer' [������] � ��� ����� � ���� ����."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

// ����������� DbContext � ������������� �����������
//builder.Services.AddDbContext<ProjectDbContext>(
//    options =>
//    {
//        options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ProjectDbContext)));
//    });
builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectDbContext")));

// ����������� ��������, ������������ � UnitOfWork
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<INewEventsService, NewEventsService>();
builder.Services.AddScoped<INewEventsRepository, NewEventsRepository>();
builder.Services.AddScoped<IParticipantsService, ParticipantsService>();
builder.Services.AddScoped<IParticipantsRepository, ParticipantsRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IValidator<NewEvent>, NewEventValidator>();
builder.Services.AddScoped<IValidator<ParticipantRequest>, ParticipantValidator>();
builder.Services.AddScoped<IImageService, ImageService>();


// ����������� UseCases
builder.Services.AddScoped<CreateNewEventUseCase>();
builder.Services.AddScoped<CreateParticipantUseCase>();
builder.Services.AddScoped<DeleteNewEventUseCase>();
builder.Services.AddScoped<DeleteParticipantUseCase>();
builder.Services.AddScoped<GetAllNewEventsUseCase>();
builder.Services.AddScoped<GetAllParticipantsUseCase>();
builder.Services.AddScoped<GetNewEventByIdUseCase>();
builder.Services.AddScoped<GetNewEventByNameUseCase>();
builder.Services.AddScoped<GetParticipantByIdUseCase>();
builder.Services.AddScoped<UpdateEventImagePathUseCase>();
builder.Services.AddScoped<UpdateNewEventUseCase>();
builder.Services.AddScoped<UpdateParticipantUseCase>();




// ����������� AutoMapper � ��������
builder.Services.AddAutoMapper(typeof(NewEventMappingProfile).Assembly);

// ������������ JWT ��������������
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

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
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ���������� ��������������� �������
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

// Middleware ��� ��������� ������
app.UseMiddleware<ExceptionMiddleware>();

// ������������ HTTP ������� � ������ ����������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // (���� ����������)

// �������������� � �����������
app.UseAuthentication();
app.UseAuthorization();

// ������������� ������������
app.MapControllers();

app.Run();
