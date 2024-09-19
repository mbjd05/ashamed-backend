using ContactsApplication.Application.Repositories;
using ContactsApplication.Application.Services;
using ContactsApplication.Infrastructure.Repositories;
using ContactsApplication.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IContactManagerService, ContactManagerService>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.Run();
