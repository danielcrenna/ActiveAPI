// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using ActiveLogging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ActiveApi
{
	public class TraceParentSafeLoggerInterceptor : ISafeLoggerInterceptor
	{
		private readonly IHttpContextAccessor _accessor;

		public TraceParentSafeLoggerInterceptor(IHttpContextAccessor accessor) => _accessor = accessor;

		public bool CanIntercept => _accessor?.HttpContext?.Request?.Headers != null &&
		                            _accessor.HttpContext.Request.Headers.TryGetValue(HttpHeaders.TraceParent,
			                            out _);

		public bool TryLog<TState>(ILogger inner, ref int safe, LogLevel logLevel, EventId eventId, TState state,
			Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!_accessor.HttpContext.Request.Headers.TryGetValue(HttpHeaders.TraceParent,
				out var traceContext))
				return false;

			try
			{
				// See: https://messagetemplates.org/
				var data = new Dictionary<string, object> {{$"@{HttpHeaders.TraceParent}", traceContext}};

				using (inner.BeginScope(data))
				{
					inner.Log(logLevel, eventId, state, exception, formatter);
					Interlocked.Exchange(ref safe, 0);
					return true;
				}
			}
			catch (Exception ex) when (LogError(ex))
			{
				throw;
			}

			bool LogError(Exception ex)
			{
				inner.LogError(ex, "Unhandled exception");
				return true;
			}
		}
	}
}