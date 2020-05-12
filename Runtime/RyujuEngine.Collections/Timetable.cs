// Copyright (c) 2020 Ryuju Orchestra
using System.Collections;
using System.Collections.Generic;

using RyujuEngine.Collections.Extension;
using RyujuEngine.Units;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// 拍数から時刻を計算するクラスです。
	/// </summary>
	public sealed class Timetable
	: IEnumerable
	, IEnumerable<BeatOrderedList<Timetable.Value>.Entry>
	{
		/// <summary>
		/// 空のインスタンスを生成します。
		/// </summary>
		public Timetable()
		: this(ImplicitDefaultTempo)
		{
		}

		/// <summary>
		/// デフォルトのテンポを指定して生成します。
		/// </summary>
		/// <param name="defaultTempo">デフォルトのテンポです。</param>
		public Timetable(in Tempo defaultTempo)
			=> _values.Add(BeatPoint.Zero, new Value(defaultTempo, BeatDuration.Zero));

		/// <summary>
		/// 開始時のテンポです。
		/// </summary>
		public Tempo DefaultTempo
		{
			get
			{
				_ = _values.TryGetValue(BeatPoint.Zero, out var value);
				return value.Tempo;
			}
		}

		/// <summary>
		/// 指定した拍数での時刻を取得します。
		/// </summary>
		/// <param name="time">拍時間です。</param>
		/// <returns>実時間です。</returns>
		public TimePoint GetTimeAt(in BeatPoint beatTime)
		{
			var durations = new List<TimeDuration>();

			var first = true;
			BeatOrderedList<Value>.Entry current = default;
			foreach (var next in this)
			{
				if (first)
				{
					current = next;
					first = false;
					continue;
				}

				if (next.Time >= beatTime)
				{
					break;
				}

				durations.InsertWithBinarySearch(
					current.Value.Tempo.AbsDurationOfBeat * (next.Time - current.Time).Double
				);
				durations.InsertWithBinarySearch(
					next.Value.Tempo.AbsDurationOfBeat * next.Value.StopDuration.Double
				);
				current = next;
			}
			durations.InsertWithBinarySearch(
				current.Value.Tempo.AbsDurationOfBeat * (beatTime - current.Time).Double
			);

			var durationFromZero = TimeDuration.Zero;
			foreach (var duration in durations)
			{
				durationFromZero += duration;
			}
			return new TimePoint(durationFromZero);
		}

		/// <summary>
		/// 指定した時間にて、新しいテンポとストップシーケンスを追加または上書きします。
		/// </summary>
		/// <param name="time">0 以上の時刻です。</param>
		/// <param name="tempo">テンポです。</param>
		public void Add(BeatPoint time, Tempo tempo)
		{
			if (time == BeatPoint.Zero && tempo == Tempo.Zero)
			{
				return;
			}
			if (_values.TryGetValue(time, out var resident))
			{
				_values.Add(time, resident.Replace(tempo));
			}
			else
			{
				_values.Add(time, new Value(tempo));
			}
		}

		/// <summary>
		/// 指定した時間にて、新しいストップシーケンスを追加または上書きします。
		/// </summary>
		/// <param name="time">0 以上の時刻です。</param>
		/// <param name="stopDuration">ストップシーケンスの拍数です。</param>
		public void Add(BeatPoint time, BeatDuration stopDuration)
		{
			if (_values.TryGetValue(time, out var resident))
			{
				_values.Add(time, resident.Replace(stopDuration));
			}
			else
			{
				_values.Add(time, new Value(stopDuration));
			}
		}

		/// <summary>
		/// 指定した時間にて、新しいテンポとストップシーケンスを追加または上書きします。
		/// </summary>
		/// <param name="time">0 以上の時刻です。</param>
		/// <param name="tempo">テンポです。</param>
		/// <param name="stopDuration">ストップシーケンスの拍数です。</param>
		public void Add(BeatPoint time, Tempo tempo, BeatDuration stopDuration)
		{
			if (time == BeatPoint.Zero && tempo == Tempo.Zero)
			{
				return;
			}
			_values.Add(time, new Value(tempo, stopDuration));
		}

		/// <summary>
		/// 指定した時刻のテンポ変更を削除します。
		/// </summary>
		/// <param name="time">削除したいテンポ変更の時刻です。</param>
		public void RemoveTempo(BeatPoint time)
		{
			if (time == BeatPoint.Zero)
			{
				return;
			}
			if (!_values.TryGetValue(time, out var resident))
			{
				return;
			}

			if (resident.StopDuration == BeatDuration.Zero)
			{
				_ = _values.TryRemoveAt(time);
			}
			else
			{
				_values.Add(time, resident.Replace(Tempo.Zero));
			}
		}

		/// <summary>
		/// 指定した時刻のストップシーケンスを削除します。
		/// </summary>
		/// <param name="time">削除したいストップシーケンスの時刻です。</param>
		public void RemoveStopSequence(BeatPoint time)
		{
			if (!_values.TryGetValue(time, out var resident))
			{
				return;
			}

			if (resident.Tempo == Tempo.Zero)
			{
				_ = _values.TryRemoveAt(time);
			}
			else
			{
				_values.Add(time, resident.Replace(BeatDuration.Zero));
			}
		}

		/// <summary>
		/// 指定した時刻のテンポ変更およびストップシーケンスを削除します。
		/// </summary>
		/// <param name="time">削除したいテンポ変更やストップシーケンスがある時刻です。</param>
		public void Remove(BeatPoint time) => _values.TryRemoveAt(time);

		/// <summary>
		/// テンポ補完を行わない列挙を行うインスタンスを生成します。
		/// </summary>
		public IEnumerable<BeatOrderedList<Value>.Entry> GetRawEnumerable() => _values;

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<BeatOrderedList<Value>.Entry>)this).GetEnumerator();

		/// <inheritdoc/>
		IEnumerator<BeatOrderedList<Value>.Entry> IEnumerable<BeatOrderedList<Value>.Entry>.GetEnumerator()
		{
			var prev = _values[0];
			foreach (var entry in _values)
			{
				var fixedEntry = entry.Value.Tempo != Tempo.Zero
					? entry
					: new BeatOrderedList<Value>.Entry(entry.Time, entry.Value.Replace(prev.Value.Tempo))
					;
				yield return fixedEntry;
			}
		}

		private static readonly Tempo ImplicitDefaultTempo = Tempo.FromBPM(130);
		private readonly BeatOrderedList<Value> _values = new BeatOrderedList<Value>();

		/// <summary>
		/// タイムテーブルで保持される値です。
		/// </summary>
		public readonly struct Value
		{
			/// <summary>
			/// デフォルト値です。
			/// </summary>
			public static readonly Value Default = new Value(ImplicitDefaultTempo);

			/// <summary>
			/// テンポのみの新しいインスタンスを生成します。
			/// </summary>
			/// <param name="tempo">テンポです。</param>
			public Value(in Tempo tempo)
			{
				Tempo = tempo;
				StopDuration = BeatDuration.Zero;
			}

			/// <summary>
			/// ストップシーケンスのみの新しいインスタンスを生成します。
			/// </summary>
			/// <param name="stopDuration">停止拍数です。</param>
			public Value(in BeatDuration stopDuration)
			{
				Tempo = Tempo.Zero;
				StopDuration = stopDuration;
			}

			/// <summary>
			/// 新しいインスタンスを生成します。
			/// </summary>
			/// <param name="tempo">テンポです。</param>
			/// <param name="stopDuration">このテンポで停止する時刻です。</param>
			public Value(in Tempo tempo, in BeatDuration stopDuration)
			{
				Tempo = tempo;
				StopDuration = stopDuration;
			}

			/// <summary>
			/// テンポを変更したインスタンスを生成します。
			/// </summary>
			/// <param name="newTempo">新しいテンポです。</param>
			public Value Replace(in Tempo newTempo) => new Value(newTempo, StopDuration);

			/// <summary>
			/// 停止時間を変更したインスタンスを生成します。
			/// </summary>
			/// <param name="stopDuration">新しい停止時間です。</param>
			public Value Replace(in BeatDuration stopDuration) => new Value(Tempo, stopDuration);

			/// <summary>
			/// テンポです。
			/// 変化がない場合は<see cref="Tempo.Zero"/>です。
			/// <see cref="Timetable.GetEnumerator"/>では必ず<see cref="Tempo.Zero"/>ではない値が入ります。
			/// </summary>
			public readonly Tempo Tempo;

			/// <summary>
			/// このテンポ変化時に停止する拍数です。
			/// 基準となるテンポは変更後のテンポです。
			/// </summary>
			public readonly BeatDuration StopDuration;
		}
	}
}
