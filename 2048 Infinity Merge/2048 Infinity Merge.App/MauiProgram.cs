using _2048_Infinity_Merge.Domain.Interfaces;
using _2048_Infinity_Merge.Domain.Rules;
using Microsoft.Extensions.Logging;

namespace _2048_Infinity_Merge.App;

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
				fonts.AddFont("PressStart2P-Regular.ttf", "PressStart2P");
			});

		builder.Services.AddMauiBlazorWebView();

		// Dependencies
		builder.Services.AddSingleton<ISystemRandom, SystemRandom>();
		builder.Services.AddSingleton<IMoving, Moving>();
		builder.Services.AddSingleton<IGameEngine, GameEngine>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
