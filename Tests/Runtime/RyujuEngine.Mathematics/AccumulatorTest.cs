// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using RyujuEngine.Mathematics;

namespace Mathematics
{
	public sealed class AccumulatorTest
	{
		[Test]
		public void It_should_be_able_to_accumulate()
		{
			var accumulator = new Accumulator();
			accumulator.Add(1.0f);
			accumulator.Add(0.5f);
			accumulator.Add(0.0f);
			accumulator.Add(0.1f);
			accumulator.Add(0.3f);
			accumulator.Add(-0.7f);
			accumulator.Add(0.9f);
			accumulator.Add(0.7f);
			accumulator.Add(0.2f);
			accumulator.Add(0.4f);
			accumulator.Add(0.6f);
			accumulator.Add(-0.6f);
			accumulator.Add(0.8f);
			var expect = ((1 + 10) * 10 / 2 - 13) / 10.0f;
			var actual = accumulator.Get();
			Assert.That(actual, Is.EqualTo(expect).Within(0.000001f));
		}
	}
}
