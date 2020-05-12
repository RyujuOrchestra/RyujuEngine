// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using System.Collections.Generic;

using RyujuEngine.Collections.Extension;

namespace Collections.Extension
{
	public sealed class IReadOnlyListExtensionsTest
	{
		private static readonly IReadOnlyList<int> TestList = new List<int>()
		{
			1,
			3,
			3,
			3,
			5,
			5,
			7,
			7,
			7,
			9,
			9,
		};

		[Test]
		[TestCase(0, false, 0, 0)]
		[TestCase(1, true, 0, 1)]
		[TestCase(2, false, 1, 1)]
		[TestCase(3, true, 1, 4)]
		[TestCase(4, false, 4, 4)]
		[TestCase(5, true, 4, 6)]
		[TestCase(6, false, 6, 6)]
		[TestCase(7, true, 6, 9)]
		[TestCase(8, false, 9, 9)]
		[TestCase(9, true, 9, 11)]
		[TestCase(10, false, 11, 11)]
		public void It_should_be_able_to_find_range(int element, bool found, int lower, int upper)
		{
			Assert.That(
				TestList.BinarySearchBounds(element, out var actualLower, out var actualUpper),
				Is.EqualTo(found),
				"Invalid found flag."
			);
			Assert.That(actualLower, Is.EqualTo(lower), "Invalid lower value.");
			Assert.That(actualUpper, Is.EqualTo(upper), "Invalid upper value.");
		}
	}
}
