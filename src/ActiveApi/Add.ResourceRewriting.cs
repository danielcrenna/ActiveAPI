// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ActiveApi.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveApi
{
	partial class Add
	{
		public static IServiceCollection AddResourceRewriting(this IServiceCollection services, IConfiguration config)
		{
			return services.AddResourceRewriting(config.Bind);
		}

		public static IServiceCollection AddResourceRewriting(this IServiceCollection services,
			Action<ResourceRewritingOptions> configureAction = null)
		{
			if (configureAction != null)
				services.Configure(configureAction);

			return services;
		}
	}
}