using ListToDo.Application.Interfaces;
using ListToDo.Application.Services;
using ListToDo.Infrastructure.Data;
using ListToDo.Application.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Mapster;
using Serilog;
using System;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// 👇 Replace default logging with Serilog
builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    })
    .AddNewtonsoftJson(); // NOT System.Text.Json

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddScoped<IToDoItemServices, ToDoItemServices>();
builder.Services.AddScoped<IToDoListServices, ToDoListServices>();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new MediaTypeApiVersionReader("v");
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

TypeAdapterConfig.GlobalSettings.Scan(typeof(MappingConfig).Assembly);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
    db.Database.EnsureDeleted();   // DANGER: drops everything
    db.Database.EnsureCreated();   // Recreates tables from model
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"To-Do List App {description.GroupName.ToUpperInvariant()}");
        }
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler("/error");

app.Map("/error", (HttpContext httpContext, ProblemDetailsFactory factory, ILogger<Program> logger) =>
{
    var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

    var statusCode = exception switch
    {
        ArgumentException => StatusCodes.Status400BadRequest,
        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
        _ => StatusCodes.Status500InternalServerError
    };

    var problem = factory.CreateProblemDetails(
        httpContext,
        statusCode: statusCode,
        detail: exception?.Message
    );

    return Results.Problem(problem.Detail, statusCode: problem.Status, title: problem.Title);
});

app.MapControllers();

app.MapControllers();

app.Run();
