using System.Globalization;
using System.Threading.RateLimiting;
using GylleneDroppen.Api.Configurations;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using GylleneDroppen.Api.Data;
using GylleneDroppen.Api.Extensions;
using GylleneDroppen.Api.Infrastructure;
using GylleneDroppen.Api.Services;
using GylleneDroppen.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppConfigurations(builder.Configuration);

builder.Services.AddScopedServices();
builder.Services.AddScopedRepositories();

builder.Services.AddSingleton<IPasswordService, PasswordService>();

builder.Services.AddAuthentication();
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddRedis(builder.Configuration);

builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (OnRejectedContext context, CancellationToken cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        var retryAfter = TimeSpan.FromMinutes(1).TotalSeconds; // Set retry time based on your policy

        // Optionally set Retry-After header
        context.HttpContext.Response.Headers.RetryAfter = retryAfter.ToString(CultureInfo.InvariantCulture);

        var response = new
        {
            error = "Too many requests",
            retryAfter = retryAfter,
            message = "Please wait and try again later."
        };

        await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken);
    };


    options.AddFixedWindowLimiter("FixedWindowPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2;
    });

    options.AddSlidingWindowLimiter("SlidingWindowPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.SegmentsPerWindow = 4;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2;
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseRateLimiter();

app.MapControllers().RequireRateLimiting("FixedWindowPolicy");

app.UseCors("AllowNextApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
