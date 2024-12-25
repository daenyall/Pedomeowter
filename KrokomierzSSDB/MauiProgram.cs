using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using SQLite;
using CommunityToolkit.Maui;
using KrokomierzSSDB.Resources.Databases;

namespace KrokomierzSSDB;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Konfiguracja podstawowa aplikacji
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureSyncfusionCore();

        // Rejestracja SQLiteAsyncConnection
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "demo_local_db.db3");
        builder.Services.AddSingleton(new SQLiteAsyncConnection(dbPath)); // Rejestrujemy instancję SQLiteAsyncConnection

        // Rejestracja usług aplikacji
        builder.Services.AddSingleton<LocalDbService>(); // Rejestrujemy serwis LocalDbService, który używa SQLiteAsyncConnection

        // Rejestracja stron aplikacji
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<Historia>();
        builder.Services.AddTransient<Ustawienia>();
        builder.Services.AddTransient<Statystyki>();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
