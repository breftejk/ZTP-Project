using API.Data;
using API.Decorators;
using API.Factories;
using API.Filters;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Register database context with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    // Define Swagger document metadata
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ZTP Project API",
        Version = "v1",
        Description = "This Swagger documentation provides a detailed overview of the API endpoints for the ZTP-Project, a cross-platform application developed with .NET, MAUI, and ASP.NET Core Web API as part of the Advanced Programming Techniques course at Bialystok University of Technology."
    });

    // Define Bearer token authentication for Swagger
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOi...\"",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    };

    options.AddSecurityRequirement(securityRequirement);
});

// Add controllers for handling API requests
builder.Services.AddControllers();

// Register the WordService
builder.Services.AddScoped<IWordService, WordService>();

// Register the WordService
builder.Services.AddScoped<UserFactory>();
builder.Services.AddScoped<IWordSetService, WordSetService>();

// Register the authorization service
builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();

// Register the decorator for logging additional information in the authorization process
builder.Services.Decorate<IAuthorizationService, AuthorizationServiceDecorator>();

// Register the authorization filter for securing API endpoints
builder.Services.AddScoped<AuthorizationFilter>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Ensure the database is created and seeded with initial data
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    DatabaseSeeder.SeedDatabase(context);
}

// Configure Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // Configure the Swagger UI endpoint and appearance
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = string.Empty; // Serve Swagger UI at the root of the app
});

// Configure middleware for development environment
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Configure routing middleware
app.UseRouting();

// Add authorization middleware if using [Authorize] attributes
app.UseAuthorization();

// Map controllers to handle incoming API requests
app.MapControllers();

// Run the application
app.Run();