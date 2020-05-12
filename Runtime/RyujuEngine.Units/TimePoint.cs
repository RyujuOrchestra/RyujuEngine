// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Runtime.CompilerServices;

using RyujuEngine.Mathematics;

namespace RyujuEngine.Units
{
	/// <summary>
	/// あるタイミングの時刻を秒数で表す構造体です。
	/// </summary>
	public readonly struct TimePoint
	: IComparable
	, IComparable<TimePoint>
	, IEquatable<TimePoint>
	{
		/// <summary>
		/// 原点となる時刻です。
		/// </summary>
		public static readonly TimePoint Zero = new TimePoint(TimeDuration.Zero);

		/// <summary>
		/// 正の無限大を表すインスタンスです。
		/// </summary>
		/// <returns></returns>
		public static readonly TimePoint PositiveInfinity = new TimePoint(TimeDuration.PositiveInfinity);

		/// <summary>
		/// 負の無限大を表すインスタンスです。
		/// </summary>
		/// <returns></returns>
		public static readonly TimePoint NegativeInfinity = new TimePoint(TimeDuration.NegativeInfinity);

		/// <summary>
		/// 指定した秒数のインスタンスを生成します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimePoint AtSeconds(double seconds) => new TimePoint(TimeDuration.OfSeconds(seconds));

		/// <summary>
		/// 原点からの秒数から TimePoint を生成します。
		/// </summary>
		/// <param name="durationFromZero">原点からの秒数です。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TimePoint(in TimeDuration durationFromZero)
			=> DurationFromZero = durationFromZero;

		/// <summary>
		/// 原点からの経過時間です。
		/// </summary>
		public readonly TimeDuration DurationFromZero;

		/// <summary>
		/// ハッシュ値を求めます。
		/// </summary>
		public override int GetHashCode() => DurationFromZero.GetHashCode();

#if UNITY_EDITOR
		/// <summary>
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
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(TimePoint x) => this == x;

		/// <summary>
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(in TimePoint x) => this == x;

		/// <summary>
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(TimePoint x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		/// <summary>
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
