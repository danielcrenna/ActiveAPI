// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ActiveRoutes;

namespace ActiveApi.Configuration
{
	public class ResourceRewritingOptions : IFeatureToggle
	{
		public string ActionHeader { get; set; } = HttpHeaders.Action;
		public bool Enabled { get; set; } = true;
	}
}