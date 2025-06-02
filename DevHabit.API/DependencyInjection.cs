namespace DevHabit.API;
public static class DependencyInjection
{
    public static WebApplicationBuilder AddControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(option =>
        {
            option.ReturnHttpNotAcceptable = true;
        })
            .AddNewtonsoftJson()   // To Apply Patch request
         .AddXmlSerializerFormatters();
          
        builder.Services.AddOpenApi();
        return builder;
    }

    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseSqlServer(builder.Configuration.GetConnectionString("Database"),
                    sqlOptions => sqlOptions
                            .MigrationsHistoryTable(HistoryRepository.DefaultTableName , Schemas.Application))
                .UseCamelCaseNamingConvention()
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        });

        return builder;
    }

    public static WebApplicationBuilder AddValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions
                .TryAdd("requestId", context.HttpContext.TraceIdentifier);
            };
        });


        return builder;
    }
    public static WebApplicationBuilder AddException(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddExceptionHandler<ValidationExceptionHandler>();

        return builder;
    }
    public static WebApplicationBuilder AddObservability(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(rescource => rescource.AddService(builder.Environment.ApplicationName))
            .WithTracing(tracing => tracing
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation())
            .WithMetrics(metrics => metrics
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation())
            .UseOtlpExporter();


        return builder;
    }
}
