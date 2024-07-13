using Amazon.Polly;
using Amazon.Translate;
using Microsoft.EntityFrameworkCore;
using MediCaseAPI.Extensions;
using MediCaseAPI.Middleware;
using MediCaseAPI.Models;
using MediCaseAPI.Options;
using MediCaseAPI.Seeders;

const string FrontendCorsPolicy = "Frontend";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MedicalContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MedicalDatabase")));
builder.Services.AddSingleton<IPublishedContextFactory>(
    new PublishedContextFactory(builder.Configuration.GetConnectionString("PublishedDatabase")!));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.Configure<FileStorageOptions>(builder.Configuration.GetSection(FileStorageOptions.SectionName));
builder.Services.Configure<SpeechOptions>(builder.Configuration.GetSection(SpeechOptions.SectionName));
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonTranslate>();
builder.Services.AddAWSService<IAmazonPolly>();
builder.Services.AddFileStorage(builder.Configuration);
builder.Services.AddCognitoAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddHealthChecks()
    .AddDbContextCheck<MedicalContext>();
builder.Services.AddCors(options =>
    options.AddPolicy(FrontendCorsPolicy, policy => policy
        .WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("Content-Disposition")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithBearerAuth();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<DataSeeder>().SeedAsync();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(FrontendCorsPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
