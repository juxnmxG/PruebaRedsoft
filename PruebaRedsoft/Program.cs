using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PruebaRedsoft.Models;
using PruebaRedsoft.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<PolicyStoreDatabaseSettings>(
    builder.Configuration.GetSection(nameof(PolicyStoreDatabaseSettings)));

builder.Services.AddSingleton<IPolicyStoreDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<PolicyStoreDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(sp => 
    new MongoClient(builder.Configuration.GetValue<string>("PolicyStoreDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IPolicyService, PolicyService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
