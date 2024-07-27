using Amazon.Polly;
using Amazon.Translate;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using MediCaseAPI.Extensions;
using MediCaseAPI.Middleware;
using MediCaseAPI.Models;
using MediCaseAPI.Options;
using MediCaseAPI.Seeders;

const string FrontendCorsPolicy = "Frontend";
const string MigrateOnlyArgument = "--migrate-only";
const string MigrateOnStartupKey = "Database:MigrateOnStartup";

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    // One JSON object per line is easy to search in CloudWatch Logs.
    builder.Logging.ClearProviders();
    builder.Logging.AddJsonConsole();
}

// Add services to the container.
builder.Services.AddDbContext<MedicalContext>(options =>
    options.UseNpgsql(DatabaseConnection.Get(builder.Configuration, "MedicalDatabase")));
builder.Services.AddSingleton<IPublishedContextFactory>(
    new PublishedContextFactory(DatabaseConnection.Get(builder.Configuration, "PublishedDatabase")));
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
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    // The API runs behind CloudFront and an Application Load Balancer.
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
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

var migrateOnly = args.Contains(MigrateOnlyArgument);
if (migrateOnly || app.Configuration.GetValue(MigrateOnStartupKey, true))
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<DataSeeder>().SeedAsync();
}

if (migrateOnly)
    return;

app.UseForwardedHeaders();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(FrontendCorsPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
