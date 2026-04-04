using Pulse_MAUI.Interfaces;

namespace Pulse_MAUI.Extensions
{
    public static class AppInitializer
    {
        public static MauiAppBuilder InitializeApp(this MauiAppBuilder builder)
        {
            builder
                .RegisterAppServices()
                .ViewModelInit();

            IAppConfiguration Configuration = builder.Services.BuildServiceProvider().GetRequiredService<IAppConfiguration>();
            builder.RefitClientInit(Configuration);

            return builder;
        }
    }
}
