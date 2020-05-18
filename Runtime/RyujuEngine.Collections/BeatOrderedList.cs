// Copyright (c) 2020 Ryuju Orchestra
using System.Collections;
using System.Collections.Generic;

using RyujuEngine.Collections.Extension;
using RyujuEngine.Units;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// A class that contains the specified type values, sorted by the <see cref="BeatPoint"/>.
	/// <see cref="BeatPoint"/>でソートされたリストを保持するクラスです。
	/// </summary>
	/// <typeparam name="T">
	/// The value type.
	/// 保持したい値の型です。
	/// </typeparam>
	public sealed class BeatOrderedList<T>
	: IEnumerable
	, IEnumerable<BeatOrderedList<T>.Entry>
	{
		/// <summary>
		/// The number of entries.
		/// 要素の数です。
		/// </summary>
		public int Count => _list.Count;

		/// <summary>
		/// An indexer that supplies direct access by an <see cref="int"/> index.
		/// 要素を直接インデックスで参照します。
		/// </summary>
		public BeatOrderedList<T>.Entry this[int index]
		{
			get => _list[index];
			set => _list[index] = value;
		}

		/// <summary>
		/// Insert or replace an entry.
		/// エントリーを挿入または入れ替えます。
		/// </summary>
		/// <param name="time">
		/// A time.
		/// 時刻です。
		/// </param>
		/// <param name="value">
		/// A value that is contained by an entry.
		/// エントリーで保持する値です。
		/// </param>
		public void Add(in BeatPoint time, T value)
		{
			if (_list.Count <= 0 || _list[_list.Count -1].Time > time)
			{
				_list.Add(new Entry(time, value));
				return;
			}
			_ = _list.ReplaceWithBinarySearch(new Entry(time, value), EntryTimeComparer.Default);
		}

		/// <summary>
		/// Get the value that is contained by entry, its time equals the specified time
		/// or is maximum until the specified time.
		/// 指定した時間の値、もしくは指定した時間より小さくかつ最大の時刻にある値を取得します。
		/// </summary>
		/// <param name="time">
		/// A time.
		/// 取得したい値の時刻です。
		/// </param>
		/// <param name="value">
		/// An out variable that receives result value.
		/// The value that was related to the specified time if exists.
		/// The value that was related to the maximum time until the specified time if exists.
		/// The <see cref="default"/> value if no entity exists until the specified time.
		/// 取得する値を保持する変数です。
		/// 見つかった場合は、それと関連付けられた値です。
		/// 見つからず、指定した時刻よりも小さい中で最大の時刻のエントリーがある場合は、そこに関連付けられた値です。
		/// 見つからず、指定した時刻よりも小さい時刻のエントリーが存在しない場合は、<see cref="default"/>です。
		/// </param>
		/// <returns>
		/// <see cref="true"/> if the value that was related to the specified time exists.
		/// <see cref="false"/> if not exists.
		/// 指定した時刻に対応するエントリーが見つかった場合は<see cref="true"/>です。
		/// エントリーが見つからなかった場合は<see cref="false"/>です。
		/// </returns>
		public bool TryGetValue(in BeatPoint time, out T value) => TryGetValue(time, out value, out _);

		/// <summary>
		/// Get the value and its index that is contained by entry, its time equals the specified time
		/// or is maximum until the specified time.
		/// 指定した時間の値、もしくは指定した時間より小さくかつ最大の時刻にある値とそのインデックスを取得します。
		/// </summary>
		/// <param name="time">
		/// A time.
		/// 取得したい値の時刻です。
		/// </param>
		/// <param name="value">
		/// An out variable that receives result value.
		/// The value that was related to the specified time if exists.
		/// The value that was related to the maximum time until the specified time if exists.
		/// The <see cref="default"/> value if no entity exists until the specified time.
		/// 取得する値を保持する変数です。
		/// 見つかった場合は、それと関連付けられた値です。
		/// 見つからず、指定した時刻よりも小さい中で最大の時刻のエントリーがある場合は、そこに関連付けられた値です。
		/// 見つからず、指定した時刻よりも小さい時刻のエントリーが存在しない場合は、<see cref="default"/>です。
		/// </param>
		/// <param name="index">
		/// An out variable that receives an index, it points to the value.
		/// <c>-1</c> otherwise.
		/// 取得した値が存在しているインデックスを保持する変数です。
		/// 存在しない場合は、<c>-1</c>です。
		/// </param>
		/// <returns>
		/// <see cref="true"/> if the value that was related to the specified time exists.
		/// <see cref="false"/> if not exists.
		/// 指定した時刻に対応するエントリーが見つかった場合は<see cref="true"/>
		/// エントリーが見つからなかった場合は<see cref="false"/>です。
		/// </returns>
		public bool TryGetValue(in BeatPoint time, out T value, out int index)
		{
			var found = _list.BinarySearchBounds(
				new Entry(time, default),
				out var lower,
				out _,
				EntryTimeComparer.Default
			);
			if (found)
			{
				value = _list[lower].Value;
				index = lower;
				return true;
			}
			index = lower - 1;
			value = lower > 0 ? _list[lower - 1].Value : default;
			return false;
		}

		/// <summary>
		/// Remove the entry at the specified time.
		/// 指定した時刻のエントリーを削除します。
		/// </summary>
		/// <param name="time">
		/// A time.
		/// 削除したいエントリーの時刻です。
		/// </param>
		/// <returns>
		/// <see cref="true"/>if the entry exists.
		/// <see cref="false"/> otherwise.
		/// エントリーが見つかり、削除された場合は<see cref="true"/>です。
		/// エントリーが見つからなかった場合は<see cref="false"/>です。
		/// </returns>
		public bool TryRemoveAt(in BeatPoint time)
		{
			var found = _list.BinarySearchBounds(
				new Entry(time, default),
				out var lower,
				out _,
				EntryTimeComparer.Default
			);
			if (found)
			{
				_list.RemoveAt(lower);
			}
			return found;
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

		/// <inheritdoc/>
		IEnumerator<Entry> IEnumerable<Entry>.GetEnumerator() => _list.GetEnumerator();

		private readonly List<Entry> _list = new List<Entry>();

		/// <summary>
		/// A struct that contains a value and a time.
		/// 登録された値を時刻とともに保持する構造体です。
		/// </summary>
		public readonly struct Entry
		{
			/// <summary>
			/// Create a new instance.
			/// 新しいインスタンスを生成します。
			/// </summary>
			/// <param name="time">
			/// A time.
			/// 時刻です。
			/// </param>
			/// <param name="value">
			/// A value.
			/// 保持する値です。
			/// </param>
			public Entry(in BeatPoint time, T value)
			{
				Time = time;
				Value = value;
			}

			/// <summary>
			/// A time.
			/// 時刻です。
			/// </summary>
			public readonly BeatPoint Time;

			/// <summary>
			/// A value.
			/// 保持する値です。
			/// </summary>
			public readonly T Value;
		}

		/// <summary>
		/// A class that compare by the <see cref="Entry.Time"/>.
		/// <see cref="Entry.Time"/>で大小関係を比較するクラスです。
		/// </summary>
		public sealed class EntryTimeComparer : IComparer<Entry>
		{
			/// <summary>
			/// An instance.
			/// インスタンスです。
			/// </summary>
			public static readonly EntryTimeComparer Default = new EntryTimeComparer();

			/// <inheritdoc/>
			public int Compare(Entry left, Entry right) => left.Time.CompareTo(right.Time);
		}
	}
}
