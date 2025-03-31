using Microsoft.Extensions.Logging;
using System.Resources;
using Microsoft.Extensions.Localization;
using CountMyDartMaui.Resources.Localization;
namespace CountMyDartMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<AppResources>();

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

		return builder.Build();
	}
}
