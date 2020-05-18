// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

using RyujuEngine.Mathematics;
using RyujuEngine.Units;

namespace Units
{
	public sealed class BeatDurationTest
	{
		private static readonly object[][] TestBeatParams = new object[][]
		{
			new object[] { 0, 0, 1 },
			new object[] { 1, 2, 3 },
			new object[] { 6, 5, 4 },
			new object[] { -2, 0, 1 },
			new object[] { -3, -2, 5 },
			new object[] { 4, -2, 4 },
			new object[] {  5, -6, -3 },
			new object[] { -7, 6, -9 },
			new object[] { -8, -27, -18 },
		};

		public static IEnumerable<object[]> OneBeat()
		{
			foreach (var param in TestBeatParams)
			{
				yield return param;
			}
		}

		public static IEnumerable<object[]> TwoBeatPairs()
		{
			foreach (var param1 in TestBeatParams)
			{
				foreach (var param2 in TestBeatParams)
				{
					yield return param1.Concat(param2).ToArray();
				}
			}
		}

		public static IEnumerable<object[]> BeatAndIntegerPairs()
		{
			for (var i = -4; i <= 4; i++)
			{
				foreach (var beatParam in TestBeatParams)
				{
					yield return beatParam.Append(i).ToArray();
				}
			}
		}

		public static IEnumerable<object[]> BeatAndRatioPairs()
		{
			for (var num = -3; num <= 3; num++)
			{
				for (var den = 1; den <= 6; den++)
				{
					foreach (var beatParam in TestBeatParams)
					{
						yield return beatParam.Append(num).Append(den).ToArray();
					}
				}
			}
		}

		[Test]
		public void It_should_be_able_to_get_zero()
		{
			var actual = BeatDuration.Zero;
			Assert.That(actual.BeatPart, Is.EqualTo(0), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(Rational.Zero), "The instance has an invalid sub-beat value.");
		}

		[Test]
		public void It_should_be_able_to_get_one()
		{
			var actual = BeatDuration.One;
			Assert.That(actual.BeatPart, Is.EqualTo(1), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(Rational.Zero), "The instance has an invalid sub-beat value.");
		}

		[Test]
		[TestCase(0, 1, 2)]
		[TestCase(3, 4, 5)]
		[TestCase(-6, -7, 8)]
		public void It_should_be_able_to_create_with_the_specified_values(int beat, long subBeatPos, int subBeatRes)
		{
			var subBeat = new Rational(subBeatPos, subBeatRes);
			var actual = BeatDuration.Of(beat, subBeatPos, subBeatRes);
			Assert.That(actual.BeatPart, Is.EqualTo(beat), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(subBeat), "The instance has an invalid sub-beat value.");
		}

		[Test]
		[TestCase(0, 2, 4, 0, 1, 2)]
		[TestCase(6, 3, 2, 7, 1, 2)]
		[TestCase(0, 3, 2, 1, 1, 2)]
		[TestCase(0, -2, 3, 0, -2, 3)]
		[TestCase(0, 5, -3, -1, -2, 3)]
		public void It_should_be_able_to_normalize(
			int beat,
			long subBeatPos,
			int subBeatRes,
			int expectedBeat,
			long expectedSubBeatPos,
			int expectedSubBeatRes
		)
		{
			var expectedSubBeat = new Rational(expectedSubBeatPos, expectedSubBeatRes);

			var actual = BeatDuration.Of(beat, subBeatPos, subBeatRes);
			Assert.That(actual.BeatPart, Is.EqualTo(expectedBeat), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(expectedSubBeat), "The instance has an invalid sub-beat value.");
		}

		[Test]
		[TestCaseSource(nameof(TwoBeatPairs))]
		public void It_should_be_able_to_add_with_two_value(
			int xBeat,
			long xSubPos,
			int xSubRes,
			int yBeat,
			long ySubPos,
			int ySubRes
		)
		{
			var x = xBeat + new Rational(xSubPos, xSubRes);
			var y = yBeat + new Rational(ySubPos, ySubRes);
			Evaluate(x + y, out var beat, out var subBeat);

			var actual
				= BeatDuration.Of(xBeat, xSubPos, xSubRes)
				+ BeatDuration.Of(yBeat, ySubPos, ySubRes)
				;
			Assert.That(actual.BeatPart, Is.EqualTo(beat), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(subBeat), "The instance has an invalid sub-beat value.");
		}

		[Test]
		[TestCaseSource(nameof(TwoBeatPairs))]
		public void It_should_be_able_to_substract(
			int xBeat,
			long xSubPos,
			int xSubRes,
			int yBeat,
			long ySubPos,
			int ySubRes
		)
		{
			var x = xBeat + new Rational(xSubPos, xSubRes);
			var y = yBeat + new Rational(ySubPos, ySubRes);
			Evaluate(x - y, out var beat, out var subBeat);

			var actual
				= BeatDuration.Of(xBeat, xSubPos, xSubRes)
				- BeatDuration.Of(yBeat, ySubPos, ySubRes)
				;
			Assert.That(actual.BeatPart, Is.EqualTo(beat), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(subBeat), "The instance has an invalid sub-beat value.");
		}

		[Test]
		[TestCaseSource(nameof(BeatAndRatioPairs))]
		public void It_should_be_able_to_multiply_with_a_ratio_from_right(
			int xBeat,
			long xSubPos,
			int xSubRes,
			long yNumerator,
			int yDenominator
		)
		{
			var x = xBeat + new Rational(xSubPos, xSubRes);
			var y = new Rational(yNumerator, yDenominator);
			Evaluate(x * y, out var beat, out var subBeat);

			var actual = BeatDuration.Of(xBeat, xSubPos, xSubRes) * y;
			Assert.That(actual.BeatPart, Is.EqualTo(beat), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(subBeat), "The instance has an invalid sub-beat value.");
		}

		[Test]
		[TestCaseSource(nameof(BeatAndRatioPairs))]
		public void It_should_be_able_to_multiply_with_a_ratio_from_left(
			int xBeat,
			long xSubPos,
			int xSubRes,
			long yNumerator,
			int yDenominator
		)
		{
			var x = xBeat + new Rational(xSubPos, xSubRes);
			var y = new Rational(yNumerator, yDenominator);
			Evaluate(x * y, out var beat, out var subBeat);

			var actual = y * BeatDuration.Of(xBeat, xSubPos, xSubRes);
			Assert.That(actual.BeatPart, Is.EqualTo(beat), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(subBeat), "The instance has an invalid sub-beat value.");
		}

		[Test]
		[TestCaseSource(nameof(BeatAndIntegerPairs))]
		public void It_should_be_able_to_multiply_with_a_int_from_right(
			int xBeat,
			long xSubPos,
			int xSubRes,
			int y
		)
		{
			var x = xBeat + new Rational(xSubPos, xSubRes);
			Evaluate(x * y, out var beat, out var subBeat);

			var actual = BeatDuration.Of(xBeat, xSubPos, xSubRes) * y;
			Assert.That(actual.BeatPart, Is.EqualTo(beat), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(subBeat), "The instance has an invalid sub-beat value.");
		}

		[Test]
		[TestCaseSource(nameof(BeatAndIntegerPairs))]
		public void It_should_be_able_to_multiply_with_a_int_from_left(
			int xBeat,
			long xSubPos,
			int xSubRes,
			int y
		)
		{
			var x = xBeat + new Rational(xSubPos, xSubRes);
			Evaluate(x * y, out var beat, out var subBeat);

			var actual = y * BeatDuration.Of(xBeat, xSubPos, xSubRes);
			Assert.That(actual.BeatPart, Is.EqualTo(beat), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(subBeat), "The instance has an invalid sub-beat value.");
		}

		[Test]
		[TestCaseSource(nameof(BeatAndIntegerPairs))]
		public void It_should_be_able_to_divide_by_a_int(
			int xBeat,
			long xSubPos,
			int xSubRes,
			int y
		)
		{
			if (y == 0)
			{
				return;
			}

			var x = xBeat + new Rational(xSubPos, xSubRes);
			Evaluate(x / y, out var beat, out var subBeat);

			var actual = BeatDuration.Of(xBeat, xSubPos, xSubRes) / y;
			Assert.That(actual.BeatPart, Is.EqualTo(beat), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(subBeat), "The instance has an invalid sub-beat value.");
		}

		[Test]
		[TestCaseSource(nameof(BeatAndRatioPairs))]
		public void It_should_be_able_to_divide_by_a_ratio(
			int xBeat,
			long xSubPos,
			int xSubRes,
			long yNumerator,
			int yDenominator
		)
		{

			var x = xBeat + new Rational(xSubPos, xSubRes);
			var y = new Rational(yNumerator, yDenominator);
			if (y == 0)
			{
				return;
			}
			Evaluate(x / y, out var beat, out var subBeat);

			var actual = BeatDuration.Of(xBeat, new Rational(xSubPos, xSubRes)) / y;
			Assert.That(actual.BeatPart, Is.EqualTo(beat), "The instance has an invalid beat value.");
			Assert.That(actual.SubBeatPart, Is.EqualTo(subBeat), "The instance has an invalid sub-beat value.");
		}

		private static void Evaluate(in Rational expression, out int beat, out Rational subBeat)
		{
			beat = (int)expression.IntegerPart;
			subBeat = expression.FractionPart;
		}
	}
}
