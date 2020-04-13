// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ActiveLogging;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveApi
{
	partial class Add
	{
		public static IServiceCollection AddTraceContext(this IServiceCollection services)
		{
			services.AddHttpContextAccessor();
			services.AddScoped<ISafeLoggerInterceptor, TraceParentSafeLoggerInterceptor>();
			return services;
		}
	}
}