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
		public static IServiceCollection AddMethodOverrides(this IServiceCollection services, IConfiguration config)
		{
			return services.AddMethodOverrides(config.Bind);
		}

		public static IServiceCollection AddMethodOverrides(this IServiceCollection services,
			Action<MethodOverrideOptions> configureAction = null)
		{
			if (configureAction != null)
				services.Configure(configureAction);

			return services;
		}
	}
}