// Copyright (cf) 2020 Ryuju Orchestra
using System.Collections.Generic;

namespace RyujuEngine.Collections.Extension
{
	/// <summary>
	/// The extensions for the <see cref="IList"/>.
	/// <see cref="IList"/>向けの拡張メソッドです。
	/// </summary>
	public static partial class IListExtensions
	{
		/// <summary>
		/// Insert or replace the value into this list using binary search with default comparer.
		/// Override the last value of the equivalent values if exists.
		/// 二分探索を用いて指定した要素を新しく挿入するか、または一番右の同値を指定した要素で上書きします。
		/// </summary>
		/// <param name="list">
		/// A target list.
		/// 挿入または上書き対象のリストです。
		/// </param>
		/// <param name="element">
		/// An insertion value.
		/// 挿入または上書きをする値です。
		/// </param>
		/// <returns>
		/// <see cref="true"/> if overrode.
		/// <see cref="false"/> if inserted.
		/// 上書きした場合は<see cref="true"/>、新しく挿入した場合は<see cref="false"/>です。
		/// </returns>
		public static bool ReplaceWithBinarySearch<T>(this IList<T> list, in T element)
			=> list.ReplaceWithBinarySearch(element, Comparer<T>.Default);

		/// <summary>
		/// Insert or replace the value into this list using binary search with the specified comparer.
		/// Override the last value of the equivalent values if exists.
		/// 指定した比較器を用いて、指定した要素を新しく挿入するか、または一番右の同値を指定した要素で上書きします。
		/// </summary>
		/// <param name="list">
		/// A target list.
		/// 挿入または上書き対象のリストです。
		/// </param>
		/// <param name="element">
		/// An insertion value.
		/// 挿入または上書きをする値です。
		/// </param>
		/// <param name="comparer">
		/// A comparer.
		/// 基準値と要素を比較するときに使用する比較器です。
		/// </param>
		/// <returns>
		/// <see cref="true"/> if overrode.
		/// <see cref="false"/> if inserted.
		/// 上書きした場合は<see cref="true"/>、新しく挿入した場合は<see cref="false"/>です。
		/// </returns>
		public static bool ReplaceWithBinarySearch<T>(this IList<T> list, in T element, IComparer<T> comparer)
		{
			var found = ((IReadOnlyList<T>)list).BinarySearchBounds(element, out _, out var upper, comparer);
			if (found)
			{
				list[upper - 1] = element;
			}
			else
			{
				list.Insert(upper, element);
			}
			return found;
		}

		/// <summary>
		/// Insert the value into this list at the tail of the equivalent value
		/// using binary search with default comparer.
		/// 二分探索を利用して、指定した要素を末尾に近い方に挿入します。
		/// </summary>
		/// <param name="list">
		/// A target list.
		/// 挿入または上書き対象のリストです。
		/// </param>
		/// <param name="element">
		/// An insertion value.
		/// 挿入する値です。
		/// </param>
		public static void InsertWithBinarySearch<T>(this IList<T> list, in T element)
			=> list.InsertWithBinarySearch(element, Comparer<T>.Default);

		/// <summary>
		/// Insert the value into this list at the tail of the equivalent value
		/// using binary search with the specified comparer.
		/// 指定した比較器を用いて、指定した要素を末尾に近い方に挿入します。
		/// </summary>
		/// <param name="list">
		/// A target list.
		/// 挿入または上書き対象のリストです。
		/// </param>
		/// <param name="element">
		/// An insertion value.
		/// 挿入する値です。
		/// </param>
		/// <param name="comparer">
		/// A comparer.
		/// 値と要素を比較するときに使用する比較器です。
		/// </param>
		public static void InsertWithBinarySearch<T>(this IList<T> list, in T element, IComparer<T> comparer)
		{
			_ = ((IReadOnlyList<T>)list).BinarySearchBounds(element, out _, out var upper, comparer);
			list.Insert(upper, element);
		}
	}
}
