using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APBD4.Handlers;
using APBD4.Middlewares;
using APBD4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


namespace APBD4
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
			services.AddTransient<SDbService, SqlDbservice>();
			services.AddControllers();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidIssuer = "Jakub",
						ValidAudience = "Students",
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
					};
			});

			//	services.AddSwaggerGen(c =>
			//{
			//	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student API", Version = "v1" });
			//});
		//	services.AddAuthentication("AuthenticationBasic").AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("AuthenticationBasic", null);
		}
			// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
			public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SDbService service)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
		//	app.UseSwagger();

			//app.UseSwaggerUI(c =>
			//{
		//		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API V1");
	//		});

	//		app.UseMiddleware<LoggingMiddleware>();

		//	app.Use(async (context, next) =>
		//	{
		//		if (!context.Request.Headers.ContainsKey("Index"))
		//		{
		//			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
		//			await context.Response.WriteAsync("Index number required");
		//			return;
		//		}
		//		string index = context.Request.Headers["Index"].ToString();
		//		var st = service.GetStudentByIndex(index);
		//		if (st == null)
		//		{
		//			context.Response.StatusCode = StatusCodes.Status400BadRequest;
		//			await context.Response.WriteAsync("Incorrect Index Number");
			//		return;
		//		}
		//		await next();
		//	});
			//app.UseHttpsRedirection();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
		{
				endpoints.MapControllers();
			});
		}
	}
}
