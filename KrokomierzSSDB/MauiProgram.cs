using Microsoft.Extensions.Logging;
#if ANDROID
using KrokomierzSSDB.Platforms.Android; // Dodano prawidłowy import dla CustomShellRenderer
#endif

namespace KrokomierzSSDB
{
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
                .ConfigureMauiHandlers(handlers =>
                {
#if ANDROID
                    handlers.AddHandler(typeof(Shell), typeof(CustomShellRenderer)); // Rejestracja renderera
#endif
                });
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddSingleton<LocalDbService>();
            builder.Services.AddTransient<Historia>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
