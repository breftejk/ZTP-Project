using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZTP_Project.Data;
using ZTP_Project.Factories;
using ZTP_Project.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ImportPolicy", policy =>
        policy.RequireRole("Admin", "Importer"));
});

// Configure Identity services
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Register repositories
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IWordRepository, WordRepository>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();

// Register exporter and importer factories
builder.Services.AddSingleton<IExporterFactory, ExporterFactory>();
builder.Services.AddSingleton<IImporterFactory, ImporterFactory>();

// Configure session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Add controllers and views to the application
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Enable session usage
app.UseSession();

// Seed database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedData.Initialize(services).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets(); // Enable serving static files

// Configure the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages(); // Map Razor Pages

app.Run(); // Start the application