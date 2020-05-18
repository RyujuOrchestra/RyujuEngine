// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using RyujuEngine.Units;

namespace Units
{
	public sealed class TempoTest
	{
		private static object[] TestCases = new object[] {
			new object[]{ 1.0f, 2.0f },
			new object[]{ 3.4f, 5.6f },
			new object[]{ 0.7f, -0.8f },
			new object[]{ -0.9f, 1.0f },
			new object[]{ -1.1f, -1.2f },
		};

		[Test]
		public void It_should_be_able_to_get_zero()
		{
			var target = Tempo.Zero;
			Assert.That(target.BeatsPerMinute, Is.EqualTo(0.0).Within(0.001), "Must be zero.");
			Assert.That(target.DurationOfBeat.Seconds, Is.EqualTo(float.PositiveInfinity).Within(0.001), "Must be zero.");
		}

		[Test]
		public void It_should_be_able_to_get_one()
		{
			var target = Tempo.One;
			Assert.That(target.BeatsPerMinute, Is.EqualTo(1.0).Within(0.001), "Must be 1.");
			Assert.That(target.DurationOfBeat.Seconds, Is.EqualTo(60.0).Within(0.001), "Must be 1.0 / 60.0.");
		}

		[Test]
		[TestCase(0.0f)]
		[TestCase(1.0f)]
		[TestCase(2.0f)]
		[TestCase(3.4f)]
		[TestCase(-5.1f)]
		public void It_should_be_able_to_create_specified_value(float value)
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
		public void It_should_be_able_to_multiply_with_a_float_from_right(float x, float y)
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
		public void It_should_be_able_to_multiply_with_a_float_from_left(float x, float y)
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
		public void It_should_be_able_to_divide_by_float(float x, float y)
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
