// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ActiveApi.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ActiveApi
{
	public static class Add
	{
		internal static IServiceCollection AddCanonicalRoutes(this IServiceCollection services)
		{
			// inbound
			services.AddSingleton(r => new CanonicalRoutesResourceFilter(r.GetRequiredService<IOptionsSnapshot<CanonicalRoutesOptions>>()));

			// outbound
			services.AddOptions<RouteOptions>().Configure<IOptionsSnapshot<CanonicalRoutesOptions>>((o, x) =>
			{
				o.AppendTrailingSlash = x.Value.AppendTrailingSlash;
				o.LowercaseUrls = x.Value.LowercaseUrls;
				o.LowercaseQueryStrings = x.Value.LowercaseQueryStrings;
			});

			return services;
		}
	}
}