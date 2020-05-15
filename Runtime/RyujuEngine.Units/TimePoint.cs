// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Runtime.CompilerServices;

using RyujuEngine.Mathematics;

namespace RyujuEngine.Units
{
	/// <summary>
	/// A struct that contains a time position.
	/// あるタイミングの時刻を保持する構造体です。
	/// </summary>
	public readonly struct TimePoint
	: IComparable
	, IComparable<TimePoint>
	, IEquatable<TimePoint>
	{
		/// <summary>
		/// A instance that indicates the origin.
		/// 原点となる時刻です。
		/// </summary>
		public static readonly TimePoint Zero = new TimePoint(TimeDuration.Zero);

		/// <summary>
		/// An instance that indicates the position infinite position.
		/// 正の無限大を表すインスタンスです。
		/// </summary>
		/// <returns></returns>
		public static readonly TimePoint PositiveInfinity = new TimePoint(TimeDuration.PositiveInfinity);

		/// <summary>
		/// An instance that indicates the negative infinite position.
		/// 負の無限大を表すインスタンスです。
		/// </summary>
		/// <returns></returns>
		public static readonly TimePoint NegativeInfinity = new TimePoint(TimeDuration.NegativeInfinity);

		/// <summary>
		/// Create an instance with the specified seconds.
		/// 指定した秒数のインスタンスを生成します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimePoint AtSeconds(double seconds) => new TimePoint(TimeDuration.OfSeconds(seconds));

		/// <summary>
		/// Create an instance with the specified duration from the origin.
		/// 原点からの秒数から TimePoint を生成します。
		/// </summary>
		/// <param name="durationFromZero">原点からの秒数です。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TimePoint(in TimeDuration durationFromZero)
			=> DurationFromZero = durationFromZero;

		/// <summary>
		/// A duration from the origin.
		/// 原点からの経過時間です。
		/// </summary>
		public readonly TimeDuration DurationFromZero;

		/// <summary>
		/// A hash value.
		/// ハッシュ値を求めます。
		/// </summary>
		public override int GetHashCode() => DurationFromZero.GetHashCode();

#if UNITY_EDITOR
		/// <summary>
		/// A string for debugging.
		/// デバッグ用の文字列表現を返します。
		/// </summary>
		public override string ToString() => DurationFromZero.ToString();
#endif

		#region Basic arithmetic operations.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimePoint operator +(in TimePoint x, in TimeDuration y)
			=> new TimePoint(x.DurationFromZero + y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimePoint operator +(in TimeDuration y, in TimePoint x)
			=> new TimePoint(x.DurationFromZero + y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimePoint operator -(in TimePoint x, in TimeDuration y)
			=> new TimePoint(x.DurationFromZero - y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeDuration operator -(in TimePoint x, in TimePoint y)
			=> x.DurationFromZero - y.DurationFromZero;

		#endregion

		#region Equal and not equal.

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (obj is null)
			{
				return false;
			}

			return obj is TimePoint point && Equals(point);
		}

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(object obj)
		{
			if (obj is null)
			{
				return 1;
			}

			return obj is TimePoint point ? CompareTo(point) : throw new ArgumentException(nameof(obj));
		}

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(TimePoint x) => this == x;

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(in TimePoint x) => this == x;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(TimePoint x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(in TimePoint x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in TimePoint x, in TimePoint y)
			=> x.DurationFromZero == y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in TimePoint x, in TimePoint y)
			=> x.DurationFromZero != y.DurationFromZero;

		#endregion

		#region Compare operators.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(in TimePoint x, in TimePoint y)
			=> x.DurationFromZero < y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(in TimePoint x, in TimePoint y)
			=> x.DurationFromZero > y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(in TimePoint x, in TimePoint y)
			=> x.DurationFromZero <= y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(in TimePoint x, in TimePoint y)
			=> x.DurationFromZero >= y.DurationFromZero;

		#endregion
	}
}
