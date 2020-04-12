using System;
using ActiveApi.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveApi
{
	partial class Add
	{
		public static IServiceCollection AddMethodOverrides(this IServiceCollection services, IConfiguration config) => services.AddMethodOverrides(config.Bind);
		public static IServiceCollection AddMethodOverrides(this IServiceCollection services, Action<MethodOverrideOptions> configureAction = null)
		{
			if(configureAction != null)
				services.Configure(configureAction);

			return services;
		}
	}
}
