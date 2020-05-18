// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using RyujuEngine.Units;

namespace Units
{
	public sealed class BeatPointTest
	{
		private static readonly object[] TwoBeatPairs = new object[]
		{
			new object[] { 2, 0, 1, 3, 0, 1 },
			new object[] { 2, 1, 3, 5, 2, 3 },
			new object[] { 1, 2, 3, 1, 2, 3 },
			new object[] { 1, 2, 3, 4, 5, 6 },
			new object[] { -1, 2, 3, 4, 5, 6 },
			new object[] { 1, 2, 3, -4, 5, 6 },
			new object[] { -1, 2, 3, -4, 5, 6 },
			new object[] { 7, 5, 6, 4, 2, 3, },
			new object[] { -7, 5, 6, 4, 2, 3, },
			new object[] { 7, 5, 6, -4, 2, 3, },
			new object[] { -7, 5, 6, -4, 2, 3, },
		};

		[Test]
		public void It_should_be_able_to_get_zero()
		{
			var actual = BeatPoint.Zero;
			Assert.That(
				actual.DurationFromZero,
				Is.EqualTo(BeatDuration.Zero),
				"The instance has an invalid beat value."
			);
		}

		[Test]
		[TestCase(0, 1, 2)]
		[TestCase(3, 4, 5)]
		[TestCase(-6, -7, 8)]
		[TestCase(0, 2, 4)]
		[TestCase(6, 3, 2)]
		[TestCase(0, 3, 2)]
		[TestCase(0, -2, 3)]
		[TestCase(0, 5, -3)]
		public void It_should_be_able_to_create_with_the_specified_values(int beat, long subBeatPos, int subBeatRes)
		{
			var durationFromZero = BeatDuration.Of(beat, subBeatPos, subBeatRes);

			var actual = BeatPoint.At(beat, subBeatPos, subBeatRes);
			Assert.That(actual.DurationFromZero, Is.EqualTo(durationFromZero), "This instance has an invalid value.");
		}

		[Test]
		[TestCaseSource(nameof(TwoBeatPairs))]
		public void It_should_be_able_to_add_with_a_duration_from_right(
			int xBeat,
			long xSubPos,
			int xSubRes,
			int yBeat,
			long ySubPos,
			int ySubRes
		)
		{
			var x = BeatDuration.Of(xBeat, xSubPos, xSubRes);
			var y = BeatDuration.Of(yBeat, ySubPos, ySubRes);

			var expected = x + y;
			var actual = BeatPoint.At(x) + y;
			Assert.That(actual.DurationFromZero, Is.EqualTo(expected), "The instance has an invalid beat value.");
		}

		[Test]
		[TestCaseSource(nameof(TwoBeatPairs))]
		public void It_should_be_able_to_add_with_a_duration_from_left(
			int xBeat,
			long xSubPos,
			int xSubRes,
			int yBeat,
			long ySubPos,
			int ySubRes
		)
		{
			var x = BeatDuration.Of(xBeat, xSubPos, xSubRes);
			var y = BeatDuration.Of(yBeat, ySubPos, ySubRes);

			var expected = y + x;
			var actual = y + BeatPoint.At(x);
			Assert.That(actual.DurationFromZero, Is.EqualTo(expected), "The instance has an invalid beat value.");
		}

		[Test]
		[TestCaseSource(nameof(TwoBeatPairs))]
		public void It_should_be_able_to_substract_with_duration(
			int xBeat,
			long xSubPos,
			int xSubRes,
			int yBeat,
			long ySubPos,
			int ySubRes
		)
		{
			var x = BeatDuration.Of(xBeat, xSubPos, xSubRes);
			var y = BeatDuration.Of(yBeat, ySubPos, ySubRes);

			var expected = x - y;
			var actual = BeatPoint.At(x) - y;
			Assert.That(actual.DurationFromZero, Is.EqualTo(expected), "The instance has an invalid beat value.");
		}
	}
}
