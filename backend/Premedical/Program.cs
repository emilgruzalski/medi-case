using Microsoft.EntityFrameworkCore;
using Premedical.Extensions;
using Premedical.Middleware;
using Premedical.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MedicalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MedicalDatabase")));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

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
