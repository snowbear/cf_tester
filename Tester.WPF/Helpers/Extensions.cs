using System;

namespace Tester.Helpers
{
	public static class Extensions
	{
		public static TOut Try<TIn, TOut>(this TIn input, Func<TIn,TOut> f)
			where TIn : class
			where TOut : class
		{
			if (input == null) return null;
			return f(input);
		}
	}
}