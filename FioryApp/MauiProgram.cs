using Microsoft.Extensions.Logging;
using FioryApp.src.Service;

namespace FioryApp;

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
			});
		
        builder.Services.AddSingleton<SessionService>(); //Manage Session Service
        builder.Services.AddSingleton<ContapymeService>(); //Manage Session Service

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}

