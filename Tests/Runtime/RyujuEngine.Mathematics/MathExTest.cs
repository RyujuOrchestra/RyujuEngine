// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using RyujuEngine.Mathematics;

namespace Mathematics
{
	public sealed class MathExTest
	{
		private static readonly object[] TestCases = new object[]
		{
			new object[]{ 0, 0, 1 },
			new object[]{ 0, 2, 2 },
			new object[]{ 0, -3, 3 },
			new object[]{ 4, 0, 4 },
			new object[]{ -5, 0, 5 },
			new object[]{ -2, 4, 2 },
			new object[]{ 2, -4, 2 },
			new object[]{ -2, -4, 2 },
			new object[]{ -4, 2, 2 },
			new object[]{ 4, -2, 2 },
			new object[]{ -4, -2, 2 },
			new object[]{ 1, 2, 1 },
			new object[]{ 3, 4, 1 },
			new object[]{ 6, 9, 3 },
			new object[]{ 14, 42, 14 },
			new object[]{ 30, 42, 6 },
		};

		[Test]
		[TestCaseSource(nameof(TestCases))]
		public void It_should_be_able_to_calculate_gcd_in_int(int x, int y, int expected)
		{
			var actual = MathEx.GCD(x, y);
			Assert.That(actual, Is.EqualTo(expected), "Must be same value.");
		}

		[Test]
		[TestCaseSource(nameof(TestCases))]
		public void It_should_be_able_to_calculate_gcd_in_long(long x, long y, long expected)
		{
			var actual = MathEx.GCD(x, y);
			Assert.That(actual, Is.EqualTo(expected), "Must be same value.");
		}
	}
}
