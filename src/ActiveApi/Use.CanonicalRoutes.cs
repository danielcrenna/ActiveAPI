// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using ActiveApi.Configuration;
using ActiveRoutes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ActiveApi
{
	public static partial class Use
	{
		public static void UseCanonicalRoutes(this IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				if (context.FeatureEnabled<CanonicalRoutesOptions>(out var options))
				{
					await ExecuteFeature(context, options, next);
				}
				else
				{
					await next();
				}
			});

			static async Task ExecuteFeature(HttpContext c, CanonicalRoutesOptions o, Func<Task> next)
			{
				if (string.Equals(c.Request.Method, HttpMethods.Get, StringComparison.OrdinalIgnoreCase))
				{
					if (!CanonicalRoutesResourceFilter.TryGetCanonicalRoute(c.Request, o, out var redirectToUrl))
					{
						c.Response.Redirect(redirectToUrl, true);
						return;
					}
				}

				await next();
			}
		}
	}
}