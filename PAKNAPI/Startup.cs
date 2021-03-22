using PAKNAPI.Common;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
using System.Linq;
using System.Text;
using PAKNAPI.Authorize;
using Microsoft.AspNetCore.Mvc.Authorization;
using PAKNAPI.Middle;

namespace BookLibAPI
{

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors();
			services.AddMvc().AddNewtonsoftJson();
			services.AddMvc(options =>
			{
				options.Filters.Add(new AuthorizeFilter("ThePolicy"));
			});

			services.AddControllers().AddNewtonsoftJson(); ;
			services.AddTransient<CustomMiddleware>();
			services.AddHttpContextAccessor();
			services.AddTransient<IAppSetting, AppSetting>();

			//services.Configure<CsSetting>(Configuration.GetSection("Default"));

			//services.AddCors(options =>
			//{
			//	options.AddPolicy("Policy",
			//		builder =>
			//		{
			//			builder.WithOrigins("http://example.com",
			//				"http://www.contoso.com");
			//		});

			//	options.AddPolicy("AnotherPolicy",
			//		builder =>
			//		{
			//			builder.WithOrigins("*")
			//				.AllowAnyHeader()
			//				.AllowAnyMethod();
			//		});
			//});

			//services.AddMvc(options => {
			//	options.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter());
			//}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.Audience = Configuration["Jwt:Issuer"];
				options.ClaimsIssuer = Configuration["Jwt:Issuer"];
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = Configuration["Jwt:Issuer"],
					ValidAudience = Configuration["Jwt:Issuer"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
				};
			});

			services.AddAuthorization(options =>
			{
				var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
				defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
				options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
				options.AddPolicy("ThePolicy", policy => policy.Requirements.Add(new ThePolicyRequirement()));
			});
			services.AddTransient<IAuthorizationHandler, ThePolicyAuthorizationHandler>();

			//services.AddSwaggerGen(c =>
			//{
			//	c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
			//	c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
			//});
			services.AddSwaggerDocument();

			// If using IIS:
			services.Configure<IISServerOptions>(options =>
			{
				options.AllowSynchronousIO = true;
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//app.UseMvc();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				//app.UseSwagger();
				//app.UseSwaggerUI(c =>
				//{
				//	c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
				//	c.RoutePrefix = string.Empty;
				//});
			}

			app.UseMiddleware<CustomMiddleware>();

			app.UseCors(
				options => options.WithOrigins("http://localhost:8081", "http://tvsach.com", "http://localhost:51046", "http://localhost:8080/")
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
			);
			app.UseOpenApi();
			app.UseSwaggerUi3();

			app.UseHttpsRedirection();

			app.UseRouting();

			//app.UseCors("AnotherPolicy");

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
