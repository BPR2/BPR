using BPR_RazorLibrary.Services.Receivers;
using BPR_RazorLibrary.Services.Sensor;
using Blazored.LocalStorage;
using BPR_RazorLibrary.Services.Users;
using BPR_RazorLibrary.Services.Fields;
using BPR_RazorLibrary.Models.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Smart.Blazor;
using BPR_RazorLibrary.Services.Charts;

namespace BPR_WebApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();
			builder.Services.AddServerSideBlazor();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddSingleton<IUserService, UserService>();
			      builder.Services.AddSingleton<IReceiverService, ReceiverService>();
            builder.Services.AddSingleton<ISensorService, SensorService>();
			builder.Services.AddSingleton<IFieldService, FieldService>();
            builder.Services.AddSingleton<IChartService, ChartService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddSmart();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.RequireAuthenticatedUser().RequireClaim("Username", "admin"));

                options.AddPolicy("User", policy =>
                policy.RequireAuthenticatedUser().RequireAssertion(context => {
                    Claim levelClaim = context.User.FindFirst(claim => claim.Type.Equals("Id"));
                    if (levelClaim == null) return false;
                    return int.Parse(levelClaim.Value) > 1;
                }));
            });

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

			app.MapBlazorHub();
			app.MapFallbackToPage("/_Host");

			app.Run();
		}
	}
}