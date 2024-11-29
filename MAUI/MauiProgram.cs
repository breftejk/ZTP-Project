using Microsoft.Extensions.Logging;
using Auth0.OidcClient;

namespace MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        builder.Services.AddSingleton<MainPage>();

        builder.Services.AddSingleton(new Auth0Client(new()
        {
            Domain = "ztp-project.eu.auth0.com",
            ClientId = "m0QBuy42uHdqw002iYdXAAjwfySRSj1N",
            RedirectUri = "ztp.project.app://callback/",
            PostLogoutRedirectUri = "ztp.project.app://callback/",
            Scope = "openid profile email"
        }));

        return builder.Build();
    }
}