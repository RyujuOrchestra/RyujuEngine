// Copyright (c) 2020 Ryuju Orchestra
using System.Collections;
using System.Collections.Generic;

using RyujuEngine.Collections.Extension;
using RyujuEngine.Mathematics;
using RyujuEngine.Units;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// A class that calculates time from beats count.
	/// 拍数から時刻を計算するクラスです。
	/// </summary>
	public sealed class Timetable
	: IEnumerable
	, IEnumerable<BeatOrderedList<Timetable.Value>.Entry>
	{
		/// <summary>
		/// Create an empty instance.
		/// 空のインスタンスを生成します。
		/// </summary>
		public Timetable()
		: this(ImplicitDefaultTempo)
		{
		}

		/// <summary>
		/// Create an instance with the initial tempo.
		/// デフォルトのテンポを指定して生成します。
		/// </summary>
		/// <param name="defaultTempo">デフォルトのテンポです。</param>
		public Timetable(in Tempo defaultTempo)
			=> _values.Add(BeatPoint.Zero, new Value(defaultTempo, BeatDuration.Zero));

		/// <summary>
		/// The initial tempo.
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
		/// Get time at the beats count.
		/// 指定した拍数での時刻を取得します。
		/// </summary>
		/// <param name="time">拍時間です。</param>
		/// <returns>実時間です。</returns>
		public TimePoint GetTimeAt(in BeatPoint beatTime)
		{
			var totalSeconds = new Accumulator();

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
				var sectionDuration = current.Value.Tempo.AbsDurationOfBeat
					* ((BeatDurationFloat)(next.Time - current.Time)).Beats;
				totalSeconds.Add(sectionDuration.Seconds);

				var stopDuration = next.Value.Tempo.AbsDurationOfBeat
					* ((BeatDurationFloat)next.Value.StopDuration).Beats;
				totalSeconds.Add(stopDuration.Seconds);
				current = next;
			}
			var tailDuration = current.Value.Tempo.AbsDurationOfBeat
				* ((BeatDurationFloat)(beatTime - current.Time)).Beats;
			totalSeconds.Add(tailDuration.Seconds);

			return TimePoint.AtSeconds(totalSeconds.Get());
		}

		/// <summary>
		/// Get a beat count at the specified time point.
		/// 指定した時刻での拍数を取得します。
		/// </summary>
		/// <param name="time">
		/// A time.
		/// 時刻です。
		/// </param>
		public BeatPointFloat GetBeatTimeAt(in TimePoint time)
		{
			GetBeatsAt(time, out var beats, out _);
			return beats;
		}

		/// <summary>
		/// Get a sequence position at the specified time point.
		/// 指定した時刻でのシーケンス位置を取得します。
		/// </summary>
		/// <param name="time">
		/// A time.
		/// 時刻です。
		/// </param>
		public BeatPointFloat GetSequencePositionAt(in TimePoint time)
		{
			GetBeatsAt(time, out _, out var position);
			return position;
		}

		/// <summary>
		/// Get a beat count and a sequence position at the specified time point.
		/// 指定した時刻での拍数とシーケンス位置を取得します。
		/// </summary>
		/// <param name="time">
		/// A time.
		/// 時刻です。
		/// </param>
		/// <param name="beats">
		/// A variable that will be contained a beat count at the specified time.
		/// 指定した時刻での拍数を格納する変数です。
		/// </param>
		/// <param name="sequencePosition">
		/// A variable that will be contained a beat count at the specified time.
		/// 指定した時刻でのシーケンス位置を格納する変数です。
		/// </param>
		public void GetBeatsAt(in TimePoint time, out BeatPointFloat beats, out BeatPointFloat sequencePosition)
		{
			var totalTimeSeconds = new Accumulator();
			var leadingBeatPoint = BeatPoint.Zero;
			var leadingSequencePosition = BeatPoint.Zero;
			var trailingBeatDuration = BeatDurationFloat.Zero;
			var trailingSequenceMovement = BeatDurationFloat.Zero;

			var first = true;
			var reached = false;
			BeatOrderedList<Value>.Entry current = default;
			foreach (var next in this)
			{
				if (first)
				{
					current = next;
					first = false;
					continue;
				}

				reached = ReachedAt(
					time,
					current.Value.Tempo,
					current.Value.StopDuration,
					next.Time - current.Time,
					totalTimeSeconds,
					ref leadingBeatPoint,
					ref leadingSequencePosition,
					out trailingBeatDuration,
					out trailingSequenceMovement
				);
				if (reached)
				{
					break;
				}
				current = next;
			}

			if (!reached)
			{
				_ = ReachedAt(
					time,
					current.Value.Tempo,
					current.Value.StopDuration,
					BeatDuration.Max,
					totalTimeSeconds,
					ref leadingBeatPoint,
					ref leadingSequencePosition,
					out trailingBeatDuration,
					out trailingSequenceMovement
				);
			}

			beats = (BeatPointFloat)leadingBeatPoint + trailingBeatDuration;
			sequencePosition = (BeatPointFloat)leadingSequencePosition + trailingSequenceMovement;
		}

		private static bool ReachedAt(
			TimePoint time,
			in Tempo tempo,
			in BeatDuration stopBeatDuration,
			in BeatDuration sectionBeatDuration,
			Accumulator prevTailTime,
			ref BeatPoint prevTailBeats,
			ref BeatPoint prevTailMovement,
			out BeatDurationFloat trailingBeatDuration,
			out BeatDurationFloat trailingSequenceMovement
		)
		{
			var stopTimeDuration = tempo.AbsDurationOfBeat * ((BeatDurationFloat)stopBeatDuration).Beats;
			prevTailTime.Add(stopTimeDuration.Seconds);
			if (TimePoint.AtSeconds(prevTailTime.Get()) >= time)
			{
				trailingBeatDuration = BeatDurationFloat.Zero;
				trailingSequenceMovement = BeatDurationFloat.Zero;
				return true;
			}

			var sectionTimeDuration = tempo.AbsDurationOfBeat * ((BeatDurationFloat)sectionBeatDuration).Beats;
			if (TimePoint.AtSeconds(prevTailTime.Get(sectionTimeDuration.Seconds)) >= time)
			{
				var trailingTimeDuration = time - TimePoint.AtSeconds(prevTailTime.Get());
				trailingBeatDuration = BeatDurationFloat.Of(trailingTimeDuration / tempo.AbsDurationOfBeat);
				trailingSequenceMovement = tempo.IsNegative ? -trailingBeatDuration : trailingBeatDuration;
				return true;
			}

			prevTailTime.Add(sectionTimeDuration.Seconds);
			prevTailBeats += sectionBeatDuration;
			prevTailMovement += tempo.IsNegative ? -sectionBeatDuration : sectionBeatDuration;
			trailingBeatDuration = BeatDurationFloat.Zero;
			trailingSequenceMovement = BeatDurationFloat.Zero;
			return false;
		}

		/// <summary>
		/// Add or replace the tempo change point at the specified time.
		/// 指定した時間にて、新しいテンポ変化を追加または上書きします。
		/// </summary>
		/// <param name="time">
		/// A time that is greater or equal than 0.
		/// 0 以上の時刻です。
		/// </param>
		/// <param name="tempo">
		/// A tempo.
		/// テンポです。
		/// </param>
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
		/// Add or replace the stop sequence duration at the specified time.
		/// 指定した時間にて、新しいストップシーケンスを追加または上書きします。
		/// </summary>
		/// <param name="time">
		/// A time that is greater or equal than 0.
		/// 0 以上の時刻です。
		/// </param>
		/// <param name="stopDuration">
		/// A stop sequence duration.
		/// ストップシーケンスの拍数です。
		/// </param>
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
		/// Add or replace the tempo change point and the stop sequence duration
		/// at the specified time.
		/// 指定した時間にて、新しいテンポとストップシーケンスを追加または上書きします。
		/// </summary>
		/// <param name="time">
		/// A time that is greater or equal than 0.
		/// 0 以上の時刻です。
		/// </param>
		/// <param name="tempo">
		/// A tempo.
		/// テンポです。
		/// </param>
		/// <param name="stopDuration">
		/// A stop sequence duration.
		/// ストップシーケンスの拍数です。
		/// </param>
		public void Add(BeatPoint time, Tempo tempo, BeatDuration stopDuration)
		{
			if (time == BeatPoint.Zero && tempo == Tempo.Zero)
			{
				return;
			}
			_values.Add(time, new Value(tempo, stopDuration));
		}

		/// <summary>
		/// Remove a tempo change point at the specified time.
		/// 指定した時刻のテンポ変更を削除します。
		/// </summary>
		/// <param name="time">
		/// A time.
		/// 削除したいテンポ変更の時刻です。
		/// </param>
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
		/// Remove a stop sequence duration at the specified time.
		/// 指定した時刻のストップシーケンスを削除します。
		/// </summary>
		/// <param name="time">
		/// A time.
		/// 削除したいストップシーケンスの時刻です。
		/// </param>
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
		/// Remove a tempo change point and a stop sequence duration at the specified time.
		/// 指定した時刻のテンポ変更およびストップシーケンスを削除します。
		/// </summary>
		/// <param name="time">
		/// A time.
		/// 削除したいテンポ変更やストップシーケンスがある時刻です。
		/// </param>
		public void Remove(BeatPoint time) => _values.TryRemoveAt(time);

		/// <summary>
		/// Get enumerable that enumerate the entries which are not autocompleted the tempo change points.
		/// テンポ補完を行わない列挙を行うインスタンスを生成します。
		/// </summary>
		public IEnumerable<BeatOrderedList<Value>.Entry> GetRawEnumerable() => _values;

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<BeatOrderedList<Value>.Entry>)this).GetEnumerator();

		/// <summary>
		/// Get enumerator that enumerate the entries which are autocompleted the tempo change points.
		/// テンポ補完を行った列挙を行う列挙子を生成します。
		/// </summary>
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
		/// A value that is contained by a timetable.
		/// タイムテーブルで保持される値です。
		/// </summary>
		public readonly struct Value
		{
			/// <summary>
			/// The default value.
			/// デフォルト値です。
			/// </summary>
			public static readonly Value Default = new Value(ImplicitDefaultTempo);

			/// <summary>
			/// Create an instance that contains only the tempo change point.
			/// テンポのみの新しいインスタンスを生成します。
			/// </summary>
			/// <param name="tempo">
			/// A tempo.
			/// テンポです。
			/// </param>
			public Value(in Tempo tempo)
			{
				Tempo = tempo;
				StopDuration = BeatDuration.Zero;
			}

			/// <summary>
			/// Create an instance that contains only the stop sequence duration.
			/// ストップシーケンスのみの新しいインスタンスを生成します。
			/// </summary>
			/// <param name="stopDuration">
			/// A stop sequence duration.
			/// 停止拍数です。
			/// </param>
			public Value(in BeatDuration stopDuration)
			{
				Tempo = Tempo.Zero;
				StopDuration = stopDuration;
			}

			/// <summary>
			/// Create an instance with the specified tempo and the stop sequence duration.
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
			/// Create an instance that is replaced with the specified tempo.
			/// テンポを変更したインスタンスを生成します。
			/// </summary>
			/// <param name="newTempo">
			/// A new tempo.
			/// 新しいテンポです。
			/// </param>
			public Value Replace(in Tempo newTempo) => new Value(newTempo, StopDuration);

			/// <summary>
			/// Create an instance that is replaced with the specified stop sequence duration.
			/// 停止時間を変更したインスタンスを生成します。
			/// </summary>
			/// <param name="stopDuration">
			/// A new stop sequence duration.
			/// 新しい停止時間です。
			/// </param>
			public Value Replace(in BeatDuration stopDuration) => new Value(Tempo, stopDuration);

			/// <summary>
			/// A tempo.
			/// The value is <see cref="Tempo.Zero"/> if this point does not contain a tempo change point.
			/// The value that is returned by  <see cref="Timetable.GetEnumerator"/> must not
			/// be <see cref="Tempo.Zero"/>.
			/// テンポです。
			/// 変化がない場合は<see cref="Tempo.Zero"/>です。
			/// <see cref="Timetable.GetEnumerator"/>では必ず<see cref="Tempo.Zero"/>ではない値が入ります。
			/// </summary>
			public readonly Tempo Tempo;

			/// <summary>
			/// A stop sequence duration in a beat count.
			/// The value depends on the next tempo.
			/// このテンポ変化時に停止する拍数です。
			/// 基準となるテンポは変更後のテンポです。
			/// </summary>
			public readonly BeatDuration StopDuration;
		}
	}
}
