using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Pulse_MAUI.Extensions;
using System.Reflection;

namespace Pulse_MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
#if DEBUG
        string environment = "DEV";
#else
		string environment = "PROD";
#endif

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .SetupAppConfig(environment)
                .UseMauiCommunityToolkit()
                .ConfigureMopups()
                .UseFFImageLoading()
                .InitializeApp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Signika-Regular.ttf", "SignikaRegular");
                    fonts.AddFont("Signika-Bold.ttf", "SignikaSemibold");

                    //fontawesome
                    fonts.AddFont("fa-solid-900.ttf", "MyFont");
                })
                .UseSentry(options =>
                {
                    // The DSN is the only required setting.
                    options.Dsn = "https://708db823deb7dd72073f5ac2149694b4@o4510259879673856.ingest.de.sentry.io/4510930203574352";

                    // Use debug mode if you want to see what the SDK is doing.
                    // Debug messages are written to stdout with Console.Writeline,
                    // and are viewable in your IDE's debug console or with 'adb logcat', etc.
                    // This option is not recommended when deploying your application.
                    options.Debug = true;

                    // Other Sentry options can be set here.
                }); ;

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static MauiAppBuilder SetupAppConfig(this MauiAppBuilder builder, string environment)
        {
            string appSettingFileName = $"appsettings.{environment}.json";
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                                       .FirstOrDefault(name => name.EndsWith(appSettingFileName, StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
                throw new FileNotFoundException($"Embedded resource '{appSettingFileName}' not found");

            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                builder.Configuration.AddJsonStream(stream);
            }

            return builder;

        }
    }
}
