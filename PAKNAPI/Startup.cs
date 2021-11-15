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
using PAKNAPI.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

using System;
using PAKNAPI.Chat;
using PAKNAPI.Job;

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

            // cái này ảnh hưởng đến controller ko authorize @@
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new AuthorizeFilter("ThePolicy"));
            //});


			services.AddMvc().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
				options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
				options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
			});
			services.AddTransient<CustomMiddleware>();
			services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
			services.AddTransient<IAppSetting, AppSetting>();
			services.AddTransient<IFileService, FileService>();
			services.AddTransient<IMailService, MailService>();
			services.AddTransient<IAuthorizationHandler, ThePolicyAuthorizationHandler>();

			services.AddHttpContextAccessor();
			services.AddDevExpressControls();

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

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
				

				options.AddPolicy("ThePolicy", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser().Build();
                    policy.Requirements.Add(new ThePolicyRequirement());
                });
			});
			
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

			// quart
			var jobKeyTTHC = new JobKey("syncDataTTHC");
			var jobKeyKNCT = new JobKey("syncDataKNCT");
			var jobKeyHVHCC = new JobKey("syncDataDVHCC");
			var jobKeyFeedBack = new JobKey("syncDataFeedBack");
			services.AddQuartz(q =>
			{
				q.UseMicrosoftDependencyInjectionScopedJobFactory();
				q.SchedulerId = "Scheduler-Core";
				q.UseSimpleTypeLoader();
				q.UseInMemoryStore();
				q.UseDefaultThreadPool(tp =>
				{
					tp.MaxConcurrency = 10;
				});
                q.AddJob<MyJobAdministrative>(opts => opts.WithIdentity(jobKeyTTHC));
				q.AddJob<MyJobKienNghiCuTri>(opts => opts.WithIdentity(jobKeyKNCT));
				q.AddJob<MyJobDichVuCongQuocGia>(opts => opts.WithIdentity(jobKeyHVHCC));
				q.AddJob<MyJobFeedBack>(opts => opts.WithIdentity(jobKeyFeedBack));


				// Create a trigger for the job
				q.AddTrigger(opts => opts
					.ForJob(jobKeyTTHC) // link to the syncData
					.WithIdentity("syncDataTTHC-trigger") // give the trigger a unique name
					.WithCalendarIntervalSchedule(s => s.WithIntervalInDays(2))); //time

				q.AddTrigger(opts => opts
					.ForJob(jobKeyKNCT) // link to the syncData
					.WithIdentity("syncDataKNCT-trigger") // give the trigger a unique name
					.WithCalendarIntervalSchedule(s => s.WithIntervalInDays(2))); //time
				q.AddTrigger(opts => opts
					.ForJob(jobKeyHVHCC) // link to the syncData
					.WithIdentity("syncDataHVHCC-trigger") // give the trigger a unique name
					.WithCalendarIntervalSchedule(s => s.WithIntervalInDays(1))); //time
				q.AddTrigger(opts => opts
					.ForJob(jobKeyFeedBack) // link to the syncData
					.WithIdentity("syncDataFeedBack-trigger") // give the trigger a unique name
					.WithCalendarIntervalSchedule(s => s.WithIntervalInDays(5))); //time
			});

			// ASP.NET Core hosting
			services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

			services.AddSignalR(options =>
			{
				options.EnableDetailedErrors = true;
				options.KeepAliveInterval = TimeSpan.FromMinutes(1);
				options.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
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

            // Dev
            DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new ReportStorageWebExtension1(new AppSetting(Configuration)));
			DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode = DevExpress.XtraReports.UI.DataBindingMode.Bindings;
			app.UseDevExpressControls();
			app.UseStaticFiles();
			// end

			app.UseRouting();

			//app.UseCors("AnotherPolicy");
			
			app.UseAuthorization();
			app.UseAuthentication();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHub<ChatHub>("/signalr");
			});
		}
	}



}
