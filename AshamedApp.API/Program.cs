using AshamedApp.Application.Repositories;
using AshamedApp.Application.Services;
using AshamedApp.Application.Services.Implementations;
using AshamedApp.Infrastructure.Database;
using AshamedApp.Infrastructure.Repositories;
using AshamedApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using MQTTnet.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IContactManagerService, ContactManagerService>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IMqttMessageRepository, MqttMessageRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<MqttClientService>();
builder.Services.AddControllers();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation V1");
        options.RoutePrefix = "docs";
    });
}

app.UseHttpsRedirection();

var mqttClientService = app.Services.GetRequiredService<MqttClientService>();
mqttClientService.ConnectAsync();

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

