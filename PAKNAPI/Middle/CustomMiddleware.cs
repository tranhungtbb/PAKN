using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAKNAPI.Middle
{
	public class CustomMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			context.Request.EnableBuffering();

			using (var reader = new StreamReader(context.Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
			{
				var body = await reader.ReadToEndAsync();
				context.Items.Add("body", body);
				context.Request.Body.Position = 0;
			}

			await next(context);
		}
	}
}
