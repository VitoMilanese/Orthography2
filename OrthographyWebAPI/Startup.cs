﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OAPI.Models;

namespace OAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				//options.AddPolicy("Policy1",
				//	builder =>
				//	{
				//		builder.WithOrigins("http://192.168.1.111",
				//							"http://192.168.1.5")
				//							.AllowAnyHeader()
				//							.AllowAnyMethod();
				//	});

				options.AddPolicy("AllowAnyOriginPolicy",
					builder =>
					{
						builder.AllowAnyOrigin()
								.AllowAnyHeader()
								.AllowAnyMethod();
					});
			});

			services.AddControllers();
			services.AddSingleton<IItemRepository, ItemRepository>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseCors();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}