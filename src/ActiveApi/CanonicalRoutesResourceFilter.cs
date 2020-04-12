// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using ActiveApi.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using TypeKitchen;

namespace ActiveApi
{
	/// <summary>
	///     Compares externally source routing based on existing <see cref="RouteOptions" /> and permanently redirects when
	///     they differ.
	///     <see href="https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.2" />
	/// </summary>
	public class CanonicalRoutesResourceFilter : IResourceFilter
	{
		private const string SchemeDelimiter = "://";
		private const char ForwardSlash = '/';

		private readonly IOptionsSnapshot<CanonicalRoutesOptions> _options;

		public CanonicalRoutesResourceFilter(IOptionsSnapshot<CanonicalRoutesOptions> options) => _options = options;

		public void OnResourceExecuting(ResourceExecutingContext context)
		{
			if (!string.Equals(context.HttpContext.Request.Method, HttpMethods.Get,
				StringComparison.OrdinalIgnoreCase))
				return;

			if (!TryGetCanonicalRoute(context.HttpContext.Request, _options.Value,
				out var redirectToUrl))
				context.Result = new RedirectResult(redirectToUrl, true);
		}

		public void OnResourceExecuted(ResourceExecutedContext context) { }

		internal static bool TryGetCanonicalRoute(HttpRequest request, CanonicalRoutesOptions options,
			out string redirectToUrl)
		{
			if (!options.Enabled)
			{
				redirectToUrl = null;
				return true;
			}

			var canonical = true;

			var appendTrailingSlash = options.AppendTrailingSlash;
			var lowercaseUrls = options.LowercaseUrls;
			var lowercaseQueryStrings = options.LowercaseQueryStrings;

			var sb = Pooling.StringBuilderPool.Get();
			try
			{
				if (lowercaseUrls)
				{
					AppendLowercase(sb, request.Scheme, ref canonical);
				}
				else
				{
					sb.Append(request.Scheme);
				}

				sb.Append(SchemeDelimiter);

				if (request.Host.HasValue)
				{
					if (lowercaseUrls)
					{
						AppendLowercase(sb, request.Host.Value, ref canonical);
					}
					else
					{
						sb.Append(request.Host);
					}
				}

				if (request.PathBase.HasValue)
				{
					if (lowercaseUrls)
					{
						AppendLowercase(sb, request.PathBase.Value, ref canonical);
					}
					else
					{
						sb.Append(request.PathBase);
					}

					if (appendTrailingSlash && !request.Path.HasValue)
					{
						if (request.PathBase.Value[^1] != ForwardSlash)
						{
							sb.Append(ForwardSlash);
							canonical = false;
						}
					}
				}

				if (request.Path.HasValue)
				{
					if (lowercaseUrls)
					{
						AppendLowercase(sb, request.Path.Value, ref canonical);
					}
					else
					{
						sb.Append(request.Path);
					}

					if (appendTrailingSlash)
					{
						if (request.Path.Value[^1] != ForwardSlash)
						{
							sb.Append(ForwardSlash);
							canonical = false;
						}
					}
				}

				if (request.QueryString.HasValue)
				{
					if (lowercaseUrls && lowercaseQueryStrings)
					{
						AppendLowercase(sb, request.QueryString.Value, ref canonical);
					}
					else
					{
						sb.Append(request.QueryString);
					}
				}

				redirectToUrl = canonical ? null : sb.ToString();
			}
			finally
			{
				Pooling.StringBuilderPool.Return(sb);
			}

			return canonical;
		}

		private static void AppendLowercase(StringBuilder sb, string value, ref bool valid)
		{
			for (var i = 0; i < value.Length; i++)
			{
				if (char.IsUpper(value, i))
				{
					valid = false;
					sb.Append(char.ToLowerInvariant(value[i]));
				}
				else
				{
					sb.Append(value[i]);
				}
			}
		}
	}
}