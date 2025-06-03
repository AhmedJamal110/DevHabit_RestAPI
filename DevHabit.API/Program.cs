using DevHabit.API.Extensions;
using DevHabit.API.Mapping;
using DevHabit.API.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .AddControllers()
    .AddDatabase()
    .AddValidation()
    .AddException()
    .AddObservability();



builder.Services.AddTransient<SortMapiingProvider>();
builder.Services.AddSingleton<ISortMappingDefination, SortMappingDefination<HabitDto, Habit>>(_ =>
HabitMappings.SortMapping);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.ApplyMigrationsAsync();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapControllers();
await app.RunAsync();
