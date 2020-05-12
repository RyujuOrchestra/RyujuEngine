// Copyright (cf) 2020 Ryuju Orchestra
using System.Collections.Generic;

namespace RyujuEngine.Collections.Extension
{
	/// <summary>
	/// <see cref="IList"/>向けの拡張メソッドです。
	/// </summary>
	public static partial class IListExtensions
	{
		/// <summary>
		/// 指定した要素を新しく挿入するか、または一番右の同値を指定した要素で上書きします。
		/// </summary>
		/// <param name="list">挿入または上書き対象のリストです。</param>
		/// <param name="element">挿入または上書きをする値です。</param>
		/// <returns>上書きした場合は<see cref="true"/>、新しく挿入した場合は<see cref="false"/>です。</returns>
		public static bool ReplaceWithBinarySearch<T>(this IList<T> list, in T element)
			=> list.ReplaceWithBinarySearch(element, Comparer<T>.Default);

		/// <summary>
		/// 指定した比較器を用いて、指定した要素を新しく挿入するか、または一番右の同値を指定した要素で上書きします。
		/// </summary>
		/// <param name="list">挿入または上書き対象のリストです。</param>
		/// <param name="element">挿入または上書きをする値です。</param>
		/// <param name="comparer">基準値と要素を比較するときに使用する比較器です。</param>
		/// <returns>上書きした場合は<see cref="true"/>、新しく挿入した場合は<see cref="false"/>です。</returns>
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
		/// 指定した要素を新しく挿入します。
		/// </summary>
		/// <param name="list">挿入対象のリストです。</param>
		/// <param name="element">挿入したい値です。</param>
		public static void InsertWithBinarySearch<T>(this IList<T> list, in T element)
			=> list.InsertWithBinarySearch(element, Comparer<T>.Default);

		/// <summary>
		/// 指定した比較器を用いて、指定した要素を新しく挿入します。
		/// </summary>
		/// <param name="list">挿入対象のリストです。</param>
		/// <param name="element">挿入したい値です。</param>
		/// <param name="comparer">値と要素を比較するときに使用する比較器です。</param>
		public static void InsertWithBinarySearch<T>(this IList<T> list, in T element, IComparer<T> comparer)
		{
			_ = ((IReadOnlyList<T>)list).BinarySearchBounds(element, out _, out var upper, comparer);
			list.Insert(upper, element);
		}
	}
}
