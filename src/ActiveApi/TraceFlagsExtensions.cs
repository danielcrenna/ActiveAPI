namespace ActiveApi
{
	public static class TraceFlagsExtensions
	{
		public static bool HasFlagFast(this TraceFlags value, TraceFlags flag)
		{
			return (value & flag) != 0;
		}
	}
}