// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using ActiveApi.Configuration;
using ActiveRoutes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace ActiveApi
{
	partial class Use
	{
		public static void UseResourceRewriting(this IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				if (context.FeatureEnabled<ResourceRewritingOptions>(out var options))
				{
					await ExecuteFeature(context, options, next);
				}
				else
				{
					await next();
				}
			});

			static async Task ExecuteFeature(HttpContext c, ResourceRewritingOptions o, Func<Task> next)
			{
				// Use X-Action to disambiguate one vs. many resources in a write call
				// See: http://restlet.com/blog/2015/05/18/implementing-bulk-updates-within-restful-services/
				var action = c.Request.Headers[o.ActionHeader];
				if (action.Count > 0)
				{
					var path = c.Request.Path.ToUriComponent();
					path = $"{path}/{action}";
					c.Request.Path = path;
				}

				// Use 'application/merge-patch+json' header to disambiguate JSON patch strategy:
				// See: https://tools.ietf.org/html/rfc7386
				var contentType = c.Request.Headers[HeaderNames.ContentType];
				if (contentType.Count > 0 && contentType.Contains(MediaTypeNames.Application.JsonMergePatch))
				{
					var path = c.Request.Path.ToUriComponent();
					path = $"{path}/merge";
					c.Request.Path = path;
				}

				await next();
			}
		}
	}
}