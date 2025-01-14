using AshamedApp.Application.Repositories;
using AshamedApp.Application.Services;
using AshamedApp.Application.Services.Implementations;
using AshamedApp.Application.Validators;
using AshamedApp.Infrastructure.Database;
using AshamedApp.Infrastructure.Repositories;
using AshamedApp.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMqttMessageRepository, MqttMessageRepository>();
builder.Services.AddScoped<IMqttMessageManagerService, MqttMessageManagerService>();
builder.Services.AddScoped<ISnapshotRepository, SnapshotRepository>();
builder.Services.AddScoped<ISnapshotManagerService, SnapshotManagerService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<MqttClientService>();
builder.Services.AddControllers();

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

// Register validators
builder.Services.AddValidatorsFromAssemblyContaining<TimeRangeRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SnapshotDtoValidator>();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Development configuration
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
}
else
{
    // Production configuration
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation V1");
        options.RoutePrefix = "docs";
    });
    builder.Configuration.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
}

app.UseHttpsRedirection();

var mqttClientService = app.Services.GetRequiredService<MqttClientService>();
_ = Task.Run(async () => await mqttClientService.ConnectAsync());

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Use CORS
app.UseCors("AllowFrontend");

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.Urls.Add("https://+:443");

app.Run();