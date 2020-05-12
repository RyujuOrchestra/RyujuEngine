// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using RyujuEngine.Build;

namespace Build
{
	public sealed class BuildPlayerHookTest
	{
		[Test]
		public void It_should_be_able_to_get_a_builder()
		{
			Assert.That(BuildPlayerHook.Builder, Is.Not.Null, "Builder must be non null.");
		}
	}
}
