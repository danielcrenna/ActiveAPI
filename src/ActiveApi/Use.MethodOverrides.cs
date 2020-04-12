// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using ActiveApi.Configuration;
using ActiveRoutes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ActiveApi
{
	partial class Use
	{
		public static void UseMethodOverrides(this IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				if (context.FeatureEnabled<MethodOverrideOptions>(out var options))
				{
					await ExecuteFeature(context, options, next);
				}
				else
				{
					await next();
				}
			});

			static async Task ExecuteFeature(HttpContext c, MethodOverrideOptions o, Func<Task> next)
			{
				if (c.Request.Method.Equals(HttpMethods.Post, StringComparison.OrdinalIgnoreCase) &&
				    c.Request.Headers.TryGetValue(o.MethodOverrideHeader, out var header))
				{
					var value = header.ToString();

					if (o.AllowedMethodOverrides.Contains(value, StringComparer.OrdinalIgnoreCase))
					{
						c.Request.Method = value;
					}
				}

				await next();
			}
		}
	}
}