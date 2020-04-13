// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Primitives;
using Sodium;

namespace ActiveApi
{
	public struct TraceContext
	{
		public byte Version => 0;
		public byte[] TraceId { get; set; }
		public byte[] ParentId { get; set; }
		public TraceFlags Flags { get; set; }

		public static TraceContext Empty = new TraceContext();

		public static TraceContext New()
		{
			var context = new TraceContext
			{
				TraceId = SodiumCore.GetRandomBytes(16),
				ParentId = SodiumCore.GetRandomBytes(8),
				Flags = TraceFlags.None
			};
			return context;
		}

		public StringValues Header =>
			$"00-{Utilities.BinaryToHex(TraceId)}-{Utilities.BinaryToHex(ParentId)}-{Flags:x}";
	}
}