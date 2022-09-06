using BPR_WebAPI.Data.Accounts;

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
			builder.Services.AddSingleton<IAccountService, AccountService>();

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
			app.UseAuthorization();

			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.MapControllers();

			app.Run();
		}
	}
}