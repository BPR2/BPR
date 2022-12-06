using BPR_RazorLibrary.Models.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using BPR_RazorLibrary.Services.Users;
using BPR_RazorLibrary.Services.Receivers;
using BPR_RazorLibrary.Services.Sensor;
using Blazored.LocalStorage;
using System.Security.Claims;
using BPR_RazorLibrary.Services.Fields;
using Smart.Blazor;
using BPR_RazorLibrary.Services.Charts;

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

        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IReceiverService, ReceiverService>();
        builder.Services.AddSingleton<ISensorService, SensorService>();
        builder.Services.AddSingleton<IFieldService, FieldService>();
        builder.Services.AddSingleton<IChartService, ChartService>();
        builder.Services.AddSmart();

        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddAuthorizationCore(options =>
        {
            options.AddPolicy("Admin", policy =>
                policy.RequireAuthenticatedUser().RequireClaim("Username", "admin"));
            options.AddPolicy("User", policy =>
                policy.RequireAuthenticatedUser().RequireAssertion(context =>
                {
                    Claim levelClaim = context.User.FindFirst(claim => claim.Type.Equals("Id"));
                    if (levelClaim == null) return false;
                    return int.Parse(levelClaim.Value) > 1;
                }));
        });

        return builder.Build();
    }
}
