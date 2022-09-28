using Microsoft.AspNetCore.Components.WebView.Maui;
using BPR_App.Data;
using BPR_RazorLibrary.Models.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using BPR_RazorLibrary.Data.Users;
using BPR_RazorLibrary.Data.Receiver;
using BPR_RazorLibrary.Data.Sensor;

namespace BPR_App;

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

		builder.Services.AddMauiBlazorWebView();
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
        builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IReceiverService, ReceiverService>();
        builder.Services.AddSingleton<ISensorService, SensorService>();
        builder.Services.AddAuthorizationCore();
        return builder.Build();
	}
}
