using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace KrokomierzSSDB;

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
            })
            .ConfigureSyncfusionCore();

        // Rejestracja usług
        builder.Services.AddSingleton<LocalDbService>();

        // Rejestracja stron
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<Historia>(); // Poprawna rejestracja strony Historia
        builder.Services.AddTransient<Statystyki>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
