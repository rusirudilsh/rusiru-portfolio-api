using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;

namespace Rusiru.Portfolio.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public const string CorsPolicyName = "PortfolioCorsPolicy";


        // AddSwaggerDocumentation
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Rusiru Portfolio API",
                    Version = "v1",
                    Description = "ASP.NET Core Web API for managing and serving my portfolio content."
                });
            });

            return services;
        }

         // AddCorsPolicy
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, policy =>
                {
                    policy
                        .WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }


        // AddRateLimiting
        public static IServiceCollection AddRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";

                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        error = "Too many requests",
                        message = "Please try again shortly.",
                        traceId = context.HttpContext.TraceIdentifier
                    }, cancellationToken);
                };

                options.AddPolicy("public-read", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        }));

                options.AddPolicy("admin", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity?.Name
                            ?? httpContext.Connection.RemoteIpAddress?.ToString()
                            ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 30,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        }));
            });

            return services;
        }
    }
}
