using API.Data;
using API.Extensions;
using API.Middelware;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Custom services
builder.Services.AddApplicationSecurityServices(builder);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddelware>(); // middelware exceptions hadler

app.UseHttpsRedirection();

app.UseCors("corsapp"); //<-- CROS

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

//Seed data
using var scope = app.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;
try
{
    DataContext context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    ILogger logger = services.GetRequiredService<ILogger>();
    logger.LogError(ex.Message);
}

app.Run();
