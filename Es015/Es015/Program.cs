
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using System;
using Es016.BLL.Services;
using Es016.DAL.Stores;
using Es016.API;

namespace Es016
{
	/// <summary>
	/// Configurazione dell'applicazione
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Punto di ingresso dell'applicazione
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Table Reservation Management app",
					Description = "An ASP.NET Core Web API for managing table reservations",
				});

				// using System.Reflection;
				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});

			builder.Services.AddSingleton<ClienteStore>();
			builder.Services.AddSingleton<PrenotazioneStore>();
			builder.Services.AddSingleton<TavoloStore>();

			builder.Services.AddScoped<ClienteService>();
			builder.Services.AddScoped<PrenotazioneService>();
			builder.Services.AddScoped<TavoloService>();

			//Non è necessario aggiungere i servizi di controller poiché
			//sono già stati aggiunti con AddControllers

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
