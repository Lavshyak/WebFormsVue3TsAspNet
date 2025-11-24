using System.Text.Json;
using DomainApp.Repositories.FormsRepository;
using DomainApp.Services;
using PersistenceRam.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IJsonFormsRepository, JsonFormsRepositoryRam>();
builder.Services.AddScoped<IStoreJsonFormService, StoreJsonFormService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var originsJson = builder.Configuration["Cors:DefaultPolicy:OriginsJsonArray"] ?? throw new InvalidOperationException();
        var origins = JsonSerializer.Deserialize<string[]>(originsJson) ?? throw new InvalidOperationException();
        policy.WithOrigins(origins);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
