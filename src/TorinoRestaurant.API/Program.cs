using Autofac;
using TorinoRestaurant.API.Infrastructure.Filters;
using TorinoRestaurant.Application.AutofacModules;
using TorinoRestaurant.Infrastructure.AutofacModules;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using TorinoRestaurant.Hosting;
using TorinoRestaurant.Application.Commons.Extensions;
using TorinoRestaurant.Application.Commons;
using Minio;
using TorinoRestaurant.Application.Abstractions.Services;
using TorinoRestaurant.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.RegisterDefaults();

builder.Services
    .AddCustomAuthentication(configuration)
    .AddCustomApiVersioning(configuration)
    .AddCustomCors(configuration)
    .AddControllers(options =>
    {
        options.Filters.Add(typeof(HttpGlobalExceptionFilter));
    });

builder.Services.AddCustomOption(configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCustomSwagger(configuration, typeof(Program).Name);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "TorinoRestaurant API",
                        Version = "v1",
                        Description = "HTTP API for accessing TorinoRestaurant data"
                    });
    options.DescribeAllParametersInCamelCase();
});
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy("Application is running"))
    .AddSqlServer(builder.Configuration["Database:SqlConnectionString"]!);

//Add HSTS
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.AddSingleton(sp =>
{
    var endpoint = configuration.GetValue<string>("BlobStorage:Endpoint");
    var accessKey = configuration.GetValue<string>("BlobStorage:AccessKey");
    var secretKey = configuration.GetValue<string>("BlobStorage:SecretKey");

    return new MinioClient().WithEndpoint(endpoint).WithCredentials(accessKey, secretKey).WithSSL().Build();
});

builder.Services.AddScoped<IFileStorageService, MinIOFileStorageService>(sp =>
{
    var bucketName = configuration.GetValue<string>("BlobStorage:BucketName") ?? "pixor";
    var minioClient = sp.GetRequiredService<IMinioClient>();

    return new MinIOFileStorageService(minioClient, bucketName, configuration);
});

builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new ApplicationModule());
    container.RegisterModule(new InfrastructureModule(builder.Configuration));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsProduction())
{
    // Required to forward headers from load balancers and reverse proxies
    app.UseForwardedHeaders();
    app.UseHttpsRedirection();

    //Add security response headers
    app.UseHsts();
    app.Use((context, next) =>
    {
        context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
        return next.Invoke();
    });
}

app.UseCors(options =>
{
    options.AllowAnyMethod()
           .AllowAnyHeader()
           .AllowAnyOrigin()
           .WithExposedHeaders("Content-Disposition");
});

app.UseAuthorization();

app.MapHealthChecks("healthz");
app.MapHealthChecks("liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

app.MapControllers();

app.Run();