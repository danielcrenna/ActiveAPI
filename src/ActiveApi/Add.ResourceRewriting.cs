using System;
using ActiveApi.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveApi
{
	partial class Add
	{
		public static IServiceCollection AddResourceRewriting(this IServiceCollection services, IConfiguration config) => services.AddResourceRewriting(config.Bind);
		public static IServiceCollection AddResourceRewriting(this IServiceCollection services, Action<ResourceRewritingOptions> configureAction = null)
		{
			if(configureAction != null)
				services.Configure(configureAction);

			return services;
		}
	}
}
