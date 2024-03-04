using Archz.Application.Core.Behaviors;
using Asp.Versioning;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Archz.Application.Core.Startup;

public static partial class Startup
{
    public static IServiceCollection AddCore<T>(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services
            .AddRouting(options => options.LowercaseUrls = true)
            .AddEndpointsApiExplorer()
            .AddCustomSwagger<T>()
            .AddProblemDetails();
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.AddValidatorsFromAssembly(typeof(T).Assembly);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(T).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        return services;
    }

    public static IServiceCollection AddCustomSwagger<T>(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = typeof(T).Assembly.FullName, Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type=ReferenceType.SecurityScheme,
                             Id="Bearer"
                         }
                     },
                     Array.Empty<string>()
                 }
             });
        });
        return services;
    }
}
