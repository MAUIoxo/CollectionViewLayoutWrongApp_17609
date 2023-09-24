using CollectionViewLayoutWrongApp.Pages;
using CollectionViewLayoutWrongApp.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Sharpnado.CollectionView;

namespace CollectionViewLayoutWrongApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSharpnadoCollectionView(loggerEnable: false)
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainPageViewModel>();

        return builder.Build();
	}
}
