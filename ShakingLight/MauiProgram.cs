using Microsoft.Extensions.Logging;
using ShakingLight.Services;
using ShakingLight.ViewModels;

namespace ShakingLight
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
                });
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<FlashlightService>();
#if ANDROID || IOS
            //builder.Services.AddSingleton<Platforms.Android.Services.ForegroundShakingLightService>();
            builder.Services.AddSingleton<Services.ShakingDetectorService>();
#endif
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
