using API.Filters;
using API.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    // Define the Swagger document
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ZTP Project API",
        Version = "v1",
        Description = "This Swagger documentation provides a detailed overview of the API endpoints for the ZTP-Project, a cross-platform application developed with .NET, MAUI, and ASP.NET Core Web API as part of the Advanced Programming Techniques course at Bialystok University of Technology."
    });

    // Configure Swagger to use the Bearer token for authorization
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
        {
            securityScheme, new string[] {}
        }
    };

    options.AddSecurityRequirement(securityRequirement);
});

// Add controllers
builder.Services.AddControllers();

// Register the authorization service
builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();

// Register the decorator
builder.Services.Decorate<IAuthorizationService, AuthorizationServiceDecorator>();

// Register the authorization filter
builder.Services.AddScoped<AuthorizationFilter>();

var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();

// Enable middleware to serve Swagger UI (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

// Add UseAuthorization if you are using [Authorize] or other authorization mechanisms
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the application
app.Run();