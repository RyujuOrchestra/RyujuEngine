// Copyright (c) 2020 Ryuju Orchestra
using System.Collections;
using System.Collections.Generic;

using RyujuEngine.Collections.Extension;
using RyujuEngine.Units;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// <see cref="BeatPoint"/>でソートされたリストを保持するクラスです。
	/// </summary>
	/// <typeparam name="T">保持したいエンティティの型です。</typeparam>
	public sealed class BeatOrderedList<T>
	: IEnumerable
	, IEnumerable<BeatOrderedList<T>.Entry>
	{
		/// <summary>
		/// 要素の数です。
		/// </summary>
		public int Count => _list.Count;

		/// <summary>
		/// 要素を直接インデックスで参照します。
		/// </summary>
		public BeatOrderedList<T>.Entry this[int index]
		{
			get => _list[index];
			set => _list[index] = value;
		}

		/// <summary>
		/// エントリーを挿入または入れ替えます。
		/// </summary>
		/// <param name="time">時刻です。</param>
		/// <param name="value">エントリーで保持する値です。</param>
		public void Add(in BeatPoint time, T value)
		{
			if (_list[_list.Count -1].Time > time)
			{
				_list.Add(new Entry(time, value));
			}
			_ = _list.ReplaceWithBinarySearch(new Entry(time, value), EntryTimeComparer.Default);
		}

		/// <summary>
		/// 指定した時間の値、もしくは指定した時間より小さくかつ最大の時刻にある値を取得します。
		/// </summary>
		/// <param name="time">取得したい値の時刻です。</param>
		/// <param name="value">
		/// 取得する値を保持する変数です。
		/// 見つかった場合は、それと関連付けられた値です。
		/// 見つからず、指定した時刻よりも小さい中で最大の時刻のエンティティがある場合は、そこに関連付けられた値です。
		/// 見つからず、指定した時刻よりも小さい時刻のエンティティが存在しない場合は、<see cref="default"/>です。
		/// </param>
		/// <returns>
		/// 指定した時刻に対応するエンティティが見つかった場合は<see cref="true"/>
		/// エンティティが見つからなかった場合は<see cref="false"/>です。
		/// </returns>
		public bool TryGetValue(in BeatPoint time, out T value) => TryGetValue(time, out value, out _);

		/// <summary>
		/// 指定した時間の値、もしくは指定した時間より小さい中の最大の時刻にある値とそのインデックスを取得します。
		/// </summary>
		/// <param name="time">取得したい値の時刻です。</param>
		/// <param name="value">
		/// 取得する値を保持する変数です。
		/// 見つかった場合は、それと関連付けられた値です。
		/// 見つからず、指定した時刻よりも小さい中で最大の時刻のエンティティがある場合は、そこに関連付けられた値です。
		/// 見つからず、指定した時刻よりも小さい時刻のエンティティが存在しない場合は、<see cref="default"/>です。
		/// </param>
		/// <param name="index">
		/// 取得した値が存在しているインデックスです。
		/// 見つかった場合は、そのエンティティのインデックスです。
		/// 見つからず、指定した時刻よりも小さいなかで最大の時刻のエンティティがある場合は、それを指すインデックスです。
		/// 見つからず、指定した時刻よりも小さい時刻のエンティティが存在しない場合は、<c>-1</c>です。
		/// </param>
		/// <returns>
		/// 指定した時刻に対応するエンティティが見つかった場合は<see cref="true"/>
		/// エンティティが見つからなかった場合は<see cref="false"/>です。
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
		/// 指定した時刻のエントリーを削除します。
		/// </summary>
		/// <param name="time">削除したいエントリーの時刻です。</param>
		/// <returns>
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
		/// 登録されたエンティティを時刻とともに保持する構造体です。
		/// </summary>
		public readonly struct Entry
		{
			/// <summary>
			/// 新しいインスタンスを生成します。
			/// </summary>
			/// <param name="time">時刻です。</param>
			/// <param name="value">保持する値です。</param>
			public Entry(in BeatPoint time, T value)
			{
				Time = time;
				Value = value;
			}

			/// <summary>
			/// 時刻です。
			/// </summary>
			public readonly BeatPoint Time;

			/// <summary>
			/// 保持する値です。
			/// </summary>
			public readonly T Value;
		}

		/// <summary>
		/// <see cref="Entry.Time"/>で大小関係を比較するクラスです。
		/// </summary>
		public sealed class EntryTimeComparer : IComparer<Entry>
		{
			/// <summary>
			/// インスタンスです。
			/// </summary>
			public static EntryTimeComparer Default { get; } = new EntryTimeComparer();

			/// <inheritdoc/>
			public int Compare(Entry left, Entry right) => left.Time.CompareTo(right.Time);
		}
	}
}
