// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ActiveRoutes;

namespace ActiveApi.Configuration
{
	public class CanonicalRoutesOptions : IFeatureToggle
	{
		public bool LowercaseUrls { get; set; } = true;
		public bool LowercaseQueryStrings { get; set; } = false;
		public bool AppendTrailingSlash { get; set; } = true;
		public bool Enabled { get; set; } = true;
	}
}