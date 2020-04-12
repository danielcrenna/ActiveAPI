// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ActiveRoutes;

namespace ActiveApi.Configuration
{
	public class MethodOverrideOptions : IFeatureToggle
	{
		public bool Enabled { get; set; } = true;
		public string MethodOverrideHeader { get; set; } = HttpHeaders.MethodOverride;

		public string[] AllowedMethodOverrides { get; set; } =
		{
			HttpMethods.Delete, 
			HttpMethods.Head, 
			HttpMethods.Put
		};
	}
}