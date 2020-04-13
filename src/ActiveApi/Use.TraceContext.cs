// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ActiveApi
{
	partial class Use
	{
		// 
		// See: https://www.w3.org/TR/trace-context/#problem-statement
		public static IApplicationBuilder UseTraceContext(this IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				if (!context.Request.Headers.TryGetValue(HttpHeaders.TraceParent, out var traceContext))
				{
					context.Request.Headers.Add(HttpHeaders.TraceParent,
						traceContext = TraceContext.New().Header);
				}

				context.TraceIdentifier = traceContext;

				if (app.ApplicationServices.GetService(typeof(IHttpContextAccessor)) is IHttpContextAccessor accessor)
				{
					accessor.HttpContext = context;
				}

				await next();
			});

			return app;
		}
	}
}