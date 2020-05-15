// Copyright (c) 2020 Ryuju Orchestra
using System.Runtime.CompilerServices;

namespace RyujuEngine.Mathematics
{
	/// <summary>
	/// A static class that contains non-standard mathematics functions.
	/// 標準的なクラスに無い数学的な関数をまとめた静的クラスです。
	/// </summary>
	public static class MathEx
	{
		/// <summary>
		/// Find the greatest common divisor (GCD).
		/// 最大公約数を求めます。
		/// </summary>
		/// <param name="a">
		/// First target value.
		/// 対象となる値の 1 つ目です。
		/// </param>
		/// <param name="b">
		/// Second target value.
		/// 対象となる値の 2 つ目です。
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GCD(int a, int b)
		{
			var x = a >= 0 ? a : -a;
			var y = b >= 0 ? b : -b;

			if (a == 0)
			{
				return b != 0 ? y : 1;
			}
			else if (b == 0)
			{
				return x;
			}

			if (a > b)
			{
				var t = x;
				x = y;
				y = t;
			}

			while (y != 0)
			{
				var t = x % y;
				x = y;
				y = t;
			}
			return x;
		}

		/// <summary>
		/// Find the greatest common divisor (GCD).
		/// 最大公約数を求めます。
		/// </summary>
		/// <param name="a">
		/// First target value.
		/// 対象となる値の 1 つ目です。
		/// </param>
		/// <param name="b">
		/// Second target value.
		/// 対象となる値の 2 つ目です。
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long GCD(long a, long b)
		{
			var x = a >= 0 ? a : -a;
			var y = b >= 0 ? b : -b;

			if (a == 0)
			{
				return b != 0 ? y : 1;
			}
			else if (b == 0)
			{
				return x;
			}

			if (x > y)
			{
				var t = x;
				x = y;
				y = t;
			}

			while (y != 0)
			{
				var t = x % y;
				x = y;
				y = t;
			}
			return x;
		}
	}
}
