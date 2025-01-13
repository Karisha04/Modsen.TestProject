

using Modsen.TestProject.Application.Validators;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.ConfigureSwagger();

builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();

var app = builder.Build();
app.UseRouting();
app.UseCustomMiddleware(app.Environment);

app.Run();

