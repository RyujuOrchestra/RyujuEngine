// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using System.Collections.Generic;

using RyujuEngine.Collections.Extension;

namespace Collections.Extension
{
	public sealed class IListExtensionsTest
	{
		private struct Entry
		{
			public int Order;
			public bool Overrode;
		}

		private class EntryComparer : IComparer<Entry>
		{
			public int Compare(Entry left, Entry right) => left.Order - right.Order;
		}

		private static readonly IReadOnlyList<Entry> TestList = new List<Entry>()
		{
			new Entry() { Order = 1, Overrode = false },
			new Entry() { Order = 3, Overrode = false },
			new Entry() { Order = 3, Overrode = false },
			new Entry() { Order = 3, Overrode = false },
			new Entry() { Order = 5, Overrode = false },
			new Entry() { Order = 5, Overrode = false },
			new Entry() { Order = 7, Overrode = false },
			new Entry() { Order = 7, Overrode = false },
			new Entry() { Order = 7, Overrode = false },
			new Entry() { Order = 9, Overrode = false },
			new Entry() { Order = 9, Overrode = false },
		};

		[Test]
		[TestCase(0, 0)]
		[TestCase(1, 1)]
		[TestCase(2, 1)]
		[TestCase(3, 4)]
		[TestCase(4, 4)]
		[TestCase(5, 6)]
		[TestCase(6, 6)]
		[TestCase(7, 9)]
		[TestCase(8, 9)]
		[TestCase(9, 11)]
		[TestCase(10, 11)]
		public void It_should_be_able_to_insert(int order, int index)
		{
			var list = new List<Entry>(TestList);
			list.InsertWithBinarySearch(new Entry() { Order = order, Overrode = true }, new EntryComparer());
			for (int i = 0; i < list.Count; i++)
			{
				Assert.That(list[i].Overrode, Is.EqualTo(i == index), $"The entry[{i}].Overrode must be {i == index}.");
			}
		}

		[Test]
		[TestCase(0, false, 0)]
		[TestCase(1, true, 0)]
		[TestCase(2, false, 1)]
		[TestCase(3, true, 3)]
		[TestCase(4, false, 4)]
		[TestCase(5, true, 5)]
		[TestCase(6, false, 6)]
		[TestCase(7, true, 8)]
		[TestCase(8, false, 9)]
		[TestCase(9, true, 10)]
		[TestCase(10, false, 11)]
		public void It_should_be_able_to_insert_or_replace(int order, bool replaced, int index)
		{
			var list = new List<Entry>(TestList);
			Assert.That(
				list.ReplaceWithBinarySearch(new Entry() { Order = order, Overrode = true }, new EntryComparer()),
				Is.EqualTo(replaced),
				"It must correctly detect that it must do is insert or replace it."
			);
			Assert.That(list.Count, Is.EqualTo(replaced ? TestList.Count : TestList.Count + 1), "Invalid array length.");
			for (int i = 0; i < list.Count; i++)
			{
				Assert.That(list[i].Overrode, Is.EqualTo(i == index), $"The entry[{i}].Overrode must be {i == index}.");
			}
		}
	}
}
