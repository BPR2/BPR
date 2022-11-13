using BPR_WebAPI.Services.Users;
using BPR_WebAPI.Services.Receiver;
using BPR_WebAPI.Services.Sensor;
using BPR_WebAPI.Services.Field;
using BPR_WebAPI.Services.Charts;

namespace BPR_WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddSingleton<IUserService, UserService>();
			builder.Services.AddSingleton<IReceiverService, ReceiverService>();
            builder.Services.AddSingleton<ISensorService, SensorService>();
			builder.Services.AddSingleton<IFieldService, FieldService>();
            builder.Services.AddSingleton<IChartService, ChartService>();

            builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
			{
				builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
			}));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseCors("corsapp");

			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.MapControllers();

			app.Run();
		}
	}
}