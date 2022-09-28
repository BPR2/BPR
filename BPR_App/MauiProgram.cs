using BPR_App.Data;
using BPR_RazorLibrary.Models.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using BPR_RazorLibrary.Services.Users;
using BPR_RazorLibrary.Services.Receivers;
using BPR_RazorLibrary.Services.Sensor;
using Blazored.LocalStorage;

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
        builder.Services.AddBlazoredLocalStorage();
        return builder.Build();
	}
}
