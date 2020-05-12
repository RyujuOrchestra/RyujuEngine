// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using RyujuEngine.Units;

namespace Units
{
	public sealed class TempoTest
	{
		private static object[] TestCases = new object[] {
			new object[]{ 1.0, 2.0 },
			new object[]{ 3.4, 5.6 },
			new object[]{ 0.7, -0.8 },
			new object[]{ -0.9, 1.0 },
			new object[]{ -1.1, -1.2 },
		};

		[Test]
		public void It_should_be_able_to_get_zero()
		{
			var target = Tempo.Zero;
			Assert.That(target.BeatsPerMinute, Is.EqualTo(0.0).Within(0.001), "Must be zero.");
			Assert.That(target.DurationOfBeat.Seconds, Is.EqualTo(double.PositiveInfinity).Within(0.001), "Must be zero.");
		}

		[Test]
		public void It_should_be_able_to_get_one()
		{
			var target = Tempo.One;
			Assert.That(target.BeatsPerMinute, Is.EqualTo(1.0).Within(0.001), "Must be 1.");
			Assert.That(target.DurationOfBeat.Seconds, Is.EqualTo(60.0).Within(0.001), "Must be 1.0 / 60.0.");
		}

		[Test]
		[TestCase(0.0)]
		[TestCase(1.0)]
		[TestCase(2.0)]
		[TestCase(3.4)]
		[TestCase(-5.1)]
		public void It_should_be_able_to_create_specified_value(double value)
		{
			var actual = Tempo.FromBPM(value);
			Assert.That(actual.BeatsPerMinute, Is.EqualTo(value).Within(0.001), "Must be specified value.");
			Assert.That(
				actual.BeatsPerSecond,
				Is.EqualTo(value / 60.0).Within(0.001),
				"Must be 1/60 of the specified value"
			);
			Assert.That(
				actual.DurationOfBeat.Seconds,
				Is.EqualTo(60.0 / value).Within(0.001),
				"Must be 60 / value."
			);
		}

		[Test]
		[TestCaseSource(nameof(TestCases))]
		public void It_should_be_able_to_multiply_with_a_double_from_right(double x, double y)
		{
			var expected = x * y;
			var actual = Tempo.FromBPM(x) * y;
			Assert.That(
				actual.BeatsPerMinute, Is.EqualTo(expected).Within(0.001),
				"Must be same value."
			);
		}

		[Test]
		[TestCaseSource(nameof(TestCases))]
		public void It_should_be_able_to_multiply_with_a_double_from_left(double x, double y)
		{
			var expected = x * y;
			var actual = x * Tempo.FromBPM(y);
			Assert.That(
				actual.BeatsPerMinute, Is.EqualTo(expected).Within(0.001),
				"Must be same value."
			);
		}

		[Test]
		[TestCaseSource(nameof(TestCases))]
		public void It_should_be_able_to_divide_by_double(double x, double y)
		{
			var expected = x / y;
			var actual = Tempo.FromBPM(x) / y;
			Assert.That(
				actual.BeatsPerMinute, Is.EqualTo(expected).Within(0.001),
				"Must be same value."
			);
		}
	}
}
