// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using RyujuEngine.Mathematics;

namespace Mathematics
{
	public sealed class RationalTest
	{
		private static readonly object[] OneRationalAndOneIntegerPairs = new object[]
		{
			new object[] { 0, 1, 2 },
			new object[] { 2, 1, 0 },
			new object[] { 3, 4, 5 },
			new object[] { 5, 4, 1 },
			new object[] { 5, 4, 3 },
			new object[] { 5, 4, -1 },
			new object[] { -6, 7, 8 },
			new object[] { 8, 7, -6 },
		};

		private static readonly object[] TwoRationalPairs = new object[]
		{
			new object[] { 0, 1, 2, 3 },
			new object[] { 4, 5, 6, 7 },
			new object[] { 7, 6, 5, 4 },
			new object[] { -8, 9, 10, 11 },
			new object[] { -12, 13, -14, 15 },
		};

		[Test]
		public void It_should_be_able_to_get_zero()
		{
			var actual = Rational.Zero;
			Assert.That(actual.Numerator, Is.EqualTo(0), "The numerator must be 0.");
			Assert.That(actual.Denominator, Is.EqualTo(1), "The denominator must be 1.");
			Assert.That(actual.Float, Is.EqualTo(0.0f).Within(0.001f), "The float must be 0.");
			Assert.That(actual.Double, Is.EqualTo(0.0).Within(0.001), "The double must be 0.");
			Assert.That(actual.IntegerPart, Is.EqualTo(0), "The integer part must be 0.");
			Assert.That(actual.FractionPart, Is.EqualTo(Rational.Zero), "The fraction part must be 0.");
			Assert.That(() => actual.Reciprocal, Throws.Exception, "The reciprocal must not be defined.");
		}

		[Test]
		public void It_should_be_able_to_get_one()
		{
			var actual = Rational.One;
			Assert.That(actual.Numerator, Is.EqualTo(1), "The numerator must be 1.");
			Assert.That(actual.Denominator, Is.EqualTo(1), "The denominator must be 1.");
			Assert.That(actual.Float, Is.EqualTo(1.0f).Within(0.001f), "The float must be 1.");
			Assert.That(actual.Double, Is.EqualTo(1.0).Within(0.001), "The double must be 1.");
			Assert.That(actual.IntegerPart, Is.EqualTo(1), "The integer part must be 1.");
			Assert.That(actual.FractionPart, Is.EqualTo(Rational.Zero), "The fraction part must be 0.");
			Assert.That(actual.Reciprocal, Is.EqualTo(Rational.One), "The reciprocal must be 1.");
		}

		[Test]
		[TestCase(1, 2)]
		[TestCase(3, 4)]
		[TestCase(3, 4)]
		[TestCase(-5, 6)]
		[TestCase(6, -5)]
		[TestCase(255, 127)]
		public void It_should_be_able_to_create_with_specified_relatively_prime_values(long numerator, int denominator)
		{
			var num = denominator > 0 ? numerator : -numerator;
			var den = denominator > 0 ? denominator : -denominator;
			var actual = new Rational(num, den);
			var integerPart = (int)(num / den);
			var fractionalPart = new Rational(num - integerPart * den, den);
			var reciprocal = new Rational(den, (int)num);

			Assert.That(actual.Numerator, Is.EqualTo(num), "Invalid numerator.");
			Assert.That(actual.Denominator, Is.EqualTo(den), "Invalid denominator.");
			Assert.That(actual.Float, Is.EqualTo((float)numerator / denominator).Within(0.001f), "Invalid float.");
			Assert.That(actual.Double, Is.EqualTo((double)numerator / denominator).Within(0.001), "Invalid double.");
			Assert.That(actual.IntegerPart, Is.EqualTo(integerPart), "Invalid integer part.");
			Assert.That(actual.FractionPart, Is.EqualTo(fractionalPart), "Invalid fraction part.");
			Assert.That(actual.Reciprocal, Is.EqualTo(reciprocal), "Invalid reciprocal number.");
		}

		[Test]
		[TestCase(0, 9, 0, 1)]
		[TestCase(2, 4, 1, 2)]
		[TestCase(9, 3, 3, 1)]
		[TestCase(8, 6, 4, 3)]
		[TestCase(-3, 6, -1, 2)]
		[TestCase(-6, 3, -2, 1)]
		[TestCase(3, -6, -1, 2)]
		[TestCase(-6, -3, 2, 1)]
		public void It_should_be_able_to_reduce_numbers(
			long numerator,
			int denominator,
			long reducedNumerator,
			long reducedDenominator
		)
		{
			var actual = new Rational(numerator, denominator);
			Assert.That(actual.Numerator, Is.EqualTo(reducedNumerator), "Invalid numerator.");
			Assert.That(actual.Denominator, Is.EqualTo(reducedDenominator), "Invalid denominator.");
		}

		[Test]
		[TestCaseSource(nameof(TwoRationalPairs))]
		public void It_should_be_able_to_add_a_rational(long xn, int xd, long yn, int yd)
		{
			var x = new Rational(xn, xd);
			var y = new Rational(yn, yd);
			var actualXY = x + y;
			var actualYX = y + x;
			var expected = new Rational(xn * yd + yn * xd, xd * yd);
			Assert.That(actualXY.Numerator, Is.EqualTo(expected.Numerator), "`x + y` has invalid numerator.");
			Assert.That(actualXY.Denominator, Is.EqualTo(expected.Denominator), "`x + y` has invalid denominator.");
			Assert.That(actualYX.Numerator, Is.EqualTo(expected.Numerator), "`y + x` has invalid numerator.");
			Assert.That(actualYX.Denominator, Is.EqualTo(expected.Denominator), "`y + x` has invalid denominator.");
		}

		[Test]
		[TestCaseSource(nameof(OneRationalAndOneIntegerPairs))]
		public void It_should_be_able_to_add_a_int(long xn, int xd, int y)
		{
			var x = new Rational(xn, xd);
			var actualXY = x + y;
			var actualYX = y + x;
			var expected = new Rational(xn + y * xd, xd);
			Assert.That(actualXY.Numerator, Is.EqualTo(expected.Numerator), "`x + y` has invalid numerator.");
			Assert.That(actualXY.Denominator, Is.EqualTo(expected.Denominator), "`x + y` has invalid denominator.");
			Assert.That(actualYX.Numerator, Is.EqualTo(expected.Numerator), "`y + x` has invalid numerator.");
			Assert.That(actualYX.Denominator, Is.EqualTo(expected.Denominator), "`y + x` has invalid denominator.");
		}

		[Test]
		[TestCaseSource(nameof(TwoRationalPairs))]
		public void It_should_be_able_to_be_substracted_by_a_rational(long xn, int xd, long yn, int yd)
		{
			var x = new Rational(xn, xd);
			var y = new Rational(yn, yd);
			var actual = x - y;
			var expected = new Rational(xn * yd - yn * xd, xd * yd);
			Assert.That(actual.Numerator, Is.EqualTo(expected.Numerator), "`x - y` has invalid numerator.");
			Assert.That(actual.Denominator, Is.EqualTo(expected.Denominator), "`x - y` has invalid denominator.");
		}

		[Test]
		[TestCaseSource(nameof(OneRationalAndOneIntegerPairs))]
		public void It_should_be_able_to_be_substracted_by_a_int(long xn, int xd, int y)
		{
			var x = new Rational(xn, xd);
			var actualXY = x - y;
			var actualYX = y - x;
			var expectedXY = new Rational(xn - y * xd, xd);
			var expectedYX = new Rational(y * xd - xn, xd);
			Assert.That(actualXY.Numerator, Is.EqualTo(expectedXY.Numerator), "`x - y` has invalid numerator.");
			Assert.That(actualXY.Denominator, Is.EqualTo(expectedXY.Denominator), "`x - y` has invalid denominator.");
			Assert.That(actualYX.Numerator, Is.EqualTo(expectedYX.Numerator), "`y - x` has invalid numerator.");
			Assert.That(actualYX.Denominator, Is.EqualTo(expectedYX.Denominator), "`y - x` has invalid denominator.");
		}

		[Test]
		[TestCaseSource(nameof(TwoRationalPairs))]
		public void It_should_be_able_to_be_multipled_by_a_rational(long xn, int xd, long yn, int yd)
		{
			var x = new Rational(xn, xd);
			var y = new Rational(yn, yd);
			var actualXY = x * y;
			var actualYX = y * x;
			var expected = new Rational(xn * yn, xd * yd);
			Assert.That(actualXY.Numerator, Is.EqualTo(expected.Numerator), "`x * y` has invalid numerator.");
			Assert.That(actualXY.Denominator, Is.EqualTo(expected.Denominator), "`x * y` has invalid denominator.");
			Assert.That(actualYX.Numerator, Is.EqualTo(expected.Numerator), "`y * x` has invalid numerator.");
			Assert.That(actualYX.Denominator, Is.EqualTo(expected.Denominator), "`y * x` has invalid denominator.");
		}

		[Test]
		[TestCaseSource(nameof(OneRationalAndOneIntegerPairs))]
		public void It_should_be_able_to_be_multipled_by_a_int(long xn, int xd, int y)
		{
			var x = new Rational(xn, xd);
			var actualXY = x * y;
			var actualYX = y * x;
			var expected = new Rational(xn * y, xd);
			Assert.That(actualXY.Numerator, Is.EqualTo(expected.Numerator), "`x * y` has invalid numerator.");
			Assert.That(actualXY.Denominator, Is.EqualTo(expected.Denominator), "`x * y` has invalid denominator.");
			Assert.That(actualYX.Numerator, Is.EqualTo(expected.Numerator), "`y * x` has invalid numerator.");
			Assert.That(actualYX.Denominator, Is.EqualTo(expected.Denominator), "`y * x` has invalid denominator.");
		}

		[Test]
		[TestCaseSource(nameof(TwoRationalPairs))]
		public void It_should_be_able_to_be_divided_by_a_rational(long xn, int xd, long yn, int yd)
		{
			var x = new Rational(xn, xd);
			var y = new Rational(yn, yd);
			var actual = x / y;
			var expected = new Rational(xn * yd, xd * (int)yn);
			Assert.That(actual.Numerator, Is.EqualTo(expected.Numerator), "`x / y` has invalid numerator.");
			Assert.That(actual.Denominator, Is.EqualTo(expected.Denominator), "`x / y` has invalid denominator.");
		}

		[Test]
		[TestCaseSource(nameof(OneRationalAndOneIntegerPairs))]
		public void It_should_be_able_to_divide_with_a_int(long xn, int xd, int y)
		{
			if (y == 0)
			{
				return;
			}

			var x = new Rational(xn, xd);
			var actualXY = x / y;
			var actualYX = y / x;
			var expectedXY = new Rational(xn, xd * y);
			var expectedYX = new Rational(y * xd, (int)xn);
			Assert.That(actualXY.Numerator, Is.EqualTo(expectedXY.Numerator), "`x / y` has invalid numerator.");
			Assert.That(actualXY.Denominator, Is.EqualTo(expectedXY.Denominator), "`x / y` has invalid denominator.");
			Assert.That(actualYX.Numerator, Is.EqualTo(expectedYX.Numerator), "`y / x` has invalid numerator.");
			Assert.That(actualYX.Denominator, Is.EqualTo(expectedYX.Denominator), "`y / x` has invalid denominator.");
		}

		[Test]
		[TestCaseSource(nameof(TwoRationalPairs))]
		public void It_should_be_able_to_compare_with_another(long xn, int xd, long yn, int yd)
		{
			var x = (double)xn / xd;
			var y = (double)yn / yd;
			var less = x < y;
			var greater = x > y;
			var le = x <= y;
			var ge = x >= y;

			var rationalX = new Rational(xn, xd);
			var rationalY = new Rational(yn, yd);
			Assert.That(rationalX < rationalY, Is.EqualTo(less), "Failed to compare when x < y");
			Assert.That(rationalX > rationalY, Is.EqualTo(greater), "Failed to compare when x > y");
			Assert.That(rationalX <= rationalY, Is.EqualTo(le), "Failed to compare when x <= y");
			Assert.That(rationalX >= rationalY, Is.EqualTo(ge), "Failed to compare when x >= y");
		}

		[Test]
		[TestCaseSource(nameof(OneRationalAndOneIntegerPairs))]
		public void It_should_be_able_to_compare_with_right_integer(long xn, int xd, int y)
		{
			var x = (double)xn / xd;
			var less = x < y;
			var greater = x > y;
			var le = x <= y;
			var ge = x >= y;

			var rationalX = new Rational(xn, xd);
			Assert.That(rationalX < y, Is.EqualTo(less), "Failed to compare when x < y");
			Assert.That(rationalX > y, Is.EqualTo(greater), "Failed to compare when x > y");
			Assert.That(rationalX <= y, Is.EqualTo(le), "Failed to compare when x <= y");
			Assert.That(rationalX >= y, Is.EqualTo(ge), "Failed to compare when x >= y");
		}

		[Test]
		[TestCaseSource(nameof(OneRationalAndOneIntegerPairs))]
		public void It_should_be_able_to_compare_with_left_integer(long xn, int xd, int y)
		{
			var x = (double)xn / xd;
			var less = y < x;
			var greater = y > x;
			var le = y <= x;
			var ge = y >= x;

			var rationalX = new Rational(xn, xd);
			Assert.That(y < rationalX, Is.EqualTo(less), "Failed to compare when y < x");
			Assert.That(y > rationalX, Is.EqualTo(greater), "Failed to compare when y > x");
			Assert.That(y <= rationalX, Is.EqualTo(le), "Failed to compare when y <= x");
			Assert.That(y >= rationalX, Is.EqualTo(ge), "Failed to compare when y >= x");
		}
	}
}
