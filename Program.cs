using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ZTP_Project.Data;
using ZTP_Project.Data.Export;
using ZTP_Project.Data.Import;
using ZTP_Project.Data.Repositories;
using ZTP_Project.Learning.Strategies;
using ZTP_Project.Learning.Activities;
using ZTP_Project.Learning.Challenges;
using ZTP_Project.Learning.RepeatableWords;

var builder = WebApplication.CreateBuilder(args);

//
// Memory Cache
//
builder.Services.AddMemoryCache();

//
// Database Configuration
//
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//
// Authorization Policies
//
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ImportPolicy", policy =>
        policy.RequireRole("Admin", "Importer"));
});

//
// Identity Configuration
//
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

//
// Observers and Notifiers
//
builder.Services.AddScoped<ActivityNotifier>();
builder.Services.AddScoped<IActivityObserver, ActivityLogger>();

//
// Repositories
//
builder.Services.AddScoped<WordRepository>();
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddScoped<IDailyChallengeRepository, DailyChallengeRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<ActivityLogger>();

//
// Cache
//
builder.Services.AddScoped<IWordRepository>(provider =>
{
    var repo = provider.GetRequiredService<WordRepository>();
    var cache = provider.GetRequiredService<IMemoryCache>();
    var logger = provider.GetRequiredService<ILogger<CachedWordRepositoryProxy>>();
    return new CachedWordRepositoryProxy(repo, cache, logger);
});

//
// Services
//
builder.Services.AddScoped<DailyChallengeService>();
builder.Services.AddScoped<RepeatableWordsService>();


//
// Learning Strategies and Export
//
builder.Services.AddScoped<FlashcardsStrategy>();
builder.Services.AddScoped<MultipleChoiceStrategy>();
builder.Services.AddScoped<FillInTheBlankStrategy>();
builder.Services.AddScoped<ILearningStrategies, LearningStrategies>();

//
// Exporters and Importers
//
builder.Services.AddSingleton<IDataExporters, DataExporters>();
builder.Services.AddSingleton<IDataImporters, DataImporters>();

//
// Session Configuration
//
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//
// MVC and Razor Pages
//
builder.Services.AddControllersWithViews();

var app = builder.Build();

//
// Session Middleware
//
app.UseSession();

//
// Database Seeding
//
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var notifier = scope.ServiceProvider.GetRequiredService<ActivityNotifier>();
    var logger = scope.ServiceProvider.GetRequiredService<IActivityObserver>();
    notifier.Attach(logger);

    try
    {
        SeedData.Initialize(services).Wait();
    }
    catch (Exception ex)
    {
        var logger2 = services.GetRequiredService<ILogger<Program>>();
        logger2.LogError(ex, "An error occurred while seeding the database.");
    }
}

//
// Error Handling and HTTPS
//
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();

//
// Routing, Authentication, and Authorization
//
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//
// Static Assets
//
app.MapStaticAssets();

//
// Default Route
//
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

//
// Razor Pages
//
app.MapRazorPages();

//
// Run the Application
//
app.Run();