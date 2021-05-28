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
using Bugsnag.AspNet.Core;
using PAKNAPI.Services.FileUpload;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;
using DevExpress.AspNetCore.Reporting;
using DevExpress.AspNetCore;
using PAKNAPI;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;
using System.Globalization;
using System.Collections.Generic;

namespace PAKNAPI
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
			services.Configure<FormOptions>(x =>
			{
				x.ValueLengthLimit = int.MaxValue;
				x.MultipartBodyLengthLimit = int.MaxValue;
				x.MemoryBufferThreshold = int.MaxValue;
			});

			services.Configure<RequestLocalizationOptions>(options =>
			{
				options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-GB");
				options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-GB"), new CultureInfo("en-GB") };
			});
			services.AddCors();
			services.AddMvc().AddDefaultReportingControllers();

			services.AddMvc().ConfigureApplicationPartManager(x =>
			{
				var parts = x.ApplicationParts;
				var aspNetCoreReportingAssemblyName = typeof(DevExpress.AspNetCore.Reporting.WebDocumentViewer.WebDocumentViewerController).Assembly.GetName().Name;
				var reportingPart = parts.FirstOrDefault(part => part.Name == aspNetCoreReportingAssemblyName);
				if (reportingPart != null)
				{
					parts.Remove(reportingPart);
				}
			});
			services.AddMvc().AddNewtonsoftJson();
			services.AddMvc(options =>
			{
				options.Filters.Add(new AuthorizeFilter("ThePolicy"));
			});

			services.AddMvc().AddNewtonsoftJson(); ;

			services.AddMvc().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
				options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
				options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
			});
			services.AddTransient<CustomMiddleware>();
			services.AddHttpContextAccessor();
			services.AddTransient<IAppSetting, AppSetting>();
			services.AddTransient<IFileService, FileService>();

			services.AddDevExpressControls();

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

			services.AddBugsnag(configuration =>
			{
				configuration.ApiKey = Configuration["BugsnagKey"];
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//app.UseMvc();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseMiddleware<CustomMiddleware>();

			app.UseCors(
				options => options.WithOrigins("http://localhost:8081", "http://localhost:51046", "http://14.177.236.88:6160/", "http://localhost:8080/")
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
			);
			app.UseOpenApi();
			app.UseSwaggerUi3();

			app.UseHttpsRedirection();
			//// note
			app.UseStaticFiles();
			app.UseStaticFiles(new StaticFileOptions()
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Upload/Remind")),
				RequestPath = new PathString("/Upload/Remind")
			});
			app.UseStaticFiles(new StaticFileOptions()
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Upload/BannerIntroduce")),
				RequestPath = new PathString("/Upload/BannerIntroduce")
			});
			app.UseStaticFiles(new StaticFileOptions()
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Upload/News")),
				RequestPath = new PathString("/Upload/News")
			});
			app.UseStaticFiles(new StaticFileOptions()
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Upload/User")),
				RequestPath = new PathString("/Upload/User")
			});

			// Dev
			DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new ReportStorageWebExtension1());
			DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode = DevExpress.XtraReports.UI.DataBindingMode.Bindings;
			app.UseDevExpressControls();
			app.UseStaticFiles();
			// end

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
