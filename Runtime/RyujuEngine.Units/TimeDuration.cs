// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Runtime.CompilerServices;

namespace RyujuEngine.Units
{
	/// <summary>
	/// A struct that contains a duration between two time position.
	/// 時間を秒数で表す構造体です。
	/// </summary>
	public readonly struct TimeDuration
	: IComparable
	, IComparable<TimeDuration>
	, IEquatable<TimeDuration>
	{
		/// <summary>
		/// An instance that indicates zero distance.
		/// 0 秒を表すインスタンスです。
		/// </summary>
		public static readonly TimeDuration Zero = new TimeDuration(0.0f);

		/// <summary>
		/// An instance that indicates the positive infinite distance.
		/// 正の無限大を表すインスタンスです。
		/// </summary>
		public static readonly TimeDuration PositiveInfinity = new TimeDuration(float.PositiveInfinity);

		/// <summary>
		/// An instance that indicates the negative infinite distance.
		/// 負の無限大を表すインスタンスです。
		/// </summary>
		public static readonly TimeDuration NegativeInfinity = new TimeDuration(float.NegativeInfinity);

		/// <summary>
		/// An instance that indicates a second.
		/// 1 秒を表すインスタンスです。
		/// </summary>
		public static readonly TimeDuration Second = new TimeDuration(1.0f);

		/// <summary>
		/// An instance that indicates a minute.
		/// 1 分を表すインスタンスです。
		/// </summary>
		public static readonly TimeDuration Minute = new TimeDuration(60.0f);

		/// <summary>
		/// An instance that indicates a millisecond.
		/// 1 ミリ秒を表すインスタンスです。
		/// </summary>
		public static readonly TimeDuration MilliSecond = new TimeDuration(0.001f);

		/// <summary>
		/// Create an instance with the specified seconds.
		/// 指定した秒数のインスタンスを生成します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeDuration OfSeconds(float seconds) => new TimeDuration(seconds);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private TimeDuration(float seconds) => Seconds = seconds;

		/// <summary>
		/// The time duration in a second.
		/// 秒数で表された時間です。
		/// </summary>
		public readonly float Seconds;

		/// <summary>
		/// The time duration in a millisecond.
		/// ミリ秒で表された時刻です。
		/// </summary>
		public float MilliSeconds => Seconds * 1000.0f;

		/// <summary>
		/// The time duration in a minute.
		/// 分で表された時刻です。
		/// </summary>
		public float Minutes => Seconds / 60.0f;

		/// <summary>
		/// A millisecond part of the time duration.
		/// この時間のミリ秒の部分のみを表す整数値です。
		/// </summary>
		public int MilliSecondPart => (int)(MilliSeconds % 1.0 * 1000.0f);

		/// <summary>
		/// A second part of the time duration.
		/// この時間の秒の部分のみを表す整数値です。
		/// </summary>
		public int SecondPart => (int)(Seconds % 60.0f);

		/// <summary>
		/// A minute part of the time duration.
		/// この時間の分の部分のみを表す整数値です。
		/// </summary>
		public int MinutePart => (int)Minutes;

#if UNITY_EDITOR
		/// <summary>
		/// デバッグ用の文字列表現です。
		/// </summary>
		public override string ToString() => $"{Seconds:0.000}sec";
#endif

		#region Basic arithmetic operations.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeDuration operator +(TimeDuration x) => x;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeDuration operator -(TimeDuration x) => new TimeDuration(-x.Seconds);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeDuration operator +(in TimeDuration x, in TimeDuration y)
			=> new TimeDuration(x.Seconds + y.Seconds);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeDuration operator -(in TimeDuration x, in TimeDuration y)
			=> new TimeDuration(x.Seconds - y.Seconds);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeDuration operator *(in TimeDuration x, float y)
			=> new TimeDuration(x.Seconds * y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeDuration operator *(float y, in TimeDuration x)
			=> new TimeDuration(x.Seconds * y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeDuration operator /(in TimeDuration x, float y)
			=> new TimeDuration(x.Seconds / y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float operator /(in TimeDuration x, in TimeDuration y)
			=> x.Seconds / y.Seconds;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float operator %(in TimeDuration x, in TimeDuration y)
			=> x.Seconds % y.Seconds;

		#endregion

		#region Equal and not equal.

		/// <summary>
		/// A hash value.
		/// ハッシュ値を求めます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode() => Seconds.GetHashCode();

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

			return obj is TimeDuration duration && Equals(duration);
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

			return obj is TimeDuration duration ? CompareTo(duration) : throw new ArgumentException(nameof(obj));
		}

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(TimeDuration x) => this == x;

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(in TimeDuration x) => this == x;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(TimeDuration x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(in TimeDuration x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in TimeDuration x, in TimeDuration y) => x.Seconds == y.Seconds;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in TimeDuration x, in TimeDuration y) => x.Seconds != y.Seconds;

		#endregion

		#region Compare operators.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(in TimeDuration x, in TimeDuration y) => Less(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(in TimeDuration x, in TimeDuration y) => Less(y, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(in TimeDuration x, in TimeDuration y) => !Less(y, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(in TimeDuration x, in TimeDuration y) => !Less(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool Less(in TimeDuration x, in TimeDuration y) => x.Seconds < y.Seconds;

		#endregion
	}
}
