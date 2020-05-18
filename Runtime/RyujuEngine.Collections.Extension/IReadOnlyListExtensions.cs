// Copyright (cf) 2020 Ryuju Orchestra
using System.Collections.Generic;

namespace RyujuEngine.Collections.Extension
{
	/// <summary>
	/// The extensions for the <see cref="IReadOnlyList"/>.
	/// <see cref="IReadOnlyList"/>向けの拡張メソッドです。
	/// </summary>
	public static class IReadOnlyListExtensions
	{
		/// <summary>
		/// Do a binary search and find a left-closed and right-open interval using default comparer.
		/// 二分探索を行い、指定した要素が存在しうる左閉右開区間を求めます。
		/// </summary>
		/// <param name="list">
		/// A target list.
		/// 探索対象のリストです。
		/// </param>
		/// <param name="element">
		/// A target value.
		/// 比較する基準値です。
		/// </param>
		/// <param name="lower">
		/// A variable that will be contained a left-closed interval index.
		/// 基準値が存在し得る一番最小のインデックスです。
		/// </param>
		/// <param name="upper">
		/// A variable that will be contained a right-opened interval index.
		/// 基準値が挿入されるべき一番最小のインデックスです。
		/// </param>
		/// <returns>
		/// <see cref="true"/> if found.
		/// <see cref="false"/> otherwise.
		/// 基準値が見つかった場合は<see cref="true"/>、
		/// そうでなければ<see cref="false"/>です。
		/// </returns>
		public static bool BinarySearchBounds<T>(this IReadOnlyList<T> list, in T element, out int lower, out int upper)
			=> list.BinarySearchBounds(element, out lower, out upper, Comparer<T>.Default);

		/// <summary>
		/// Do a binary search and find a left-closed and right-open interval using with the specified comparer.
		/// 指定した比較器を用いて二分探索を行い、指定した要素が存在しうる左閉区間右開区間を求めます。
		/// </summary>
		/// <param name="list">
		/// A target list.
		/// 探索対象のリストです。
		/// </param>
		/// <param name="element">
		/// A target value.
		/// 比較する基準値です。
		/// </param>
		/// <param name="lower">
		/// A variable that will be contained a left-closed interval index.
		/// 基準値が存在し得る一番最小のインデックスです。
		/// </param>
		/// <param name="upper">
		/// A variable that will be contained a right-opened interval index.
		/// 基準値が挿入されるべき一番最小のインデックスです。
		/// </param>
		/// <param name="comparer">
		/// A comparer.
		/// 基準値と要素を比較するときに使用する比較器です。
		/// </param>
		/// <returns>
		/// <see cref="true"/> if found.
		/// <see cref="false"/> otherwise.
		/// 基準値が見つかった場合は<see cref="true"/>、そうでなければ<see cref="false"/>です。
		/// </returns>
		public static bool BinarySearchBounds<T>(
			this IReadOnlyList<T> list,
			in T element,
			out int lower,
			out int upper,
			IComparer<T> comparer
		)
		{
			var length = list.Count;
			if (length == 0)
			{
				lower = 0;
				upper = 0;
				return false;
			}

			if (comparer.Compare(element, list[0]) < 0)
			{
				lower = 0;
				upper = 0;
				return false;
			}
			if (comparer.Compare(element, list[length - 1]) > 0)
			{
				lower = length;
				upper = length;
				return false;
			}

			var found = false;
			var lowerLeft = 0;
			var upperLeft = 0;
			var lowerLength = length;
			var upperLength = length;

			// Find the left-closed interval of lower and shrink the upper interval.
			// lower の左閉区間を求め、ついでに upper の予想区間も小さくしておく。
			while (lowerLength > 0)
			{
				var half = lowerLength >> 1;
				var middle = lowerLeft + half;
				var comp = comparer.Compare(element, list[middle]);
				if (comp > 0)
				{
					lowerLeft = middle + 1;
					lowerLength -= half + 1;
					if (upperLeft < middle + 1)
					{
						upperLength -= middle + 1 - upperLeft;
						upperLeft = middle + 1;
					}
				}
				else
				{
					lowerLength = half;
					if (comp == 0)
					{
						found = true;
						if (upperLeft < middle + 1)
						{
							upperLength -= middle + 1 - upperLeft;
							upperLeft = middle + 1;
						}
					}
					else
					{
						var candidateLength = middle + 1 - upperLeft;
						if (upperLength > candidateLength)
						{
							upperLength = candidateLength;
						}
					}
				}
			}

			// Find the left-closed interval of upper.
			// upper の左閉区間を求める。
			while (upperLength > 0)
			{
				var half = upperLength >> 1;
				var middle = upperLeft + half;
				var comp = comparer.Compare(element, list[middle]);
				if (comp > 0)
				{
					upperLeft = middle + 1;
					upperLength -= half + 1;
				}
				else if (comp < 0)
				{
					upperLength = half;
				}
				else
				{
					found = true;
					upperLength = half - 1;
					upperLeft = middle + 1;
				}
			}

			lower = lowerLeft;
			upper = upperLeft;
			return found;
		}
	}
}
