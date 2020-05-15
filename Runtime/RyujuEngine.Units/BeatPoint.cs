// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Runtime.CompilerServices;

using RyujuEngine.Mathematics;

namespace RyujuEngine.Units
{
	/// <summary>
	/// A struct that contains a position on a beat.
	/// あるタイミングの時刻を拍数で表す構造体です。
	/// </summary>
	public readonly struct BeatPoint
	: IComparable
	, IComparable<BeatPoint>
	, IEquatable<BeatPoint>
	{
		/// <summary>
		/// An instance that indicates the origin position on a beat.
		/// 原点となる時刻です。
		/// </summary>
		public static readonly BeatPoint Zero = new BeatPoint(0, Rational.Zero);

		/// <summary>
		/// A minimum value.
		/// 扱える最小の時刻です。
		/// </summary>
		public static readonly BeatPoint Min = new BeatPoint(int.MinValue, Rational.Zero);

		/// <summary>
		/// A maximum value.
		/// 扱える最大の時刻です。
		/// </summary>
		public static readonly BeatPoint Max = new BeatPoint(int.MaxValue, Rational.Zero);

		/// <summary>
		/// 浮動小数点で表された原点からの拍数を、指定した解像度で BeatPoint に変換します。
		/// </summary>
		/// <param name="beats">小数点以下も含む拍数です。</param>
		/// <param name="resolution">1 拍あたりの分割解像度です。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BeatPoint(double beats, uint resolution)
			=> DurationFromZero = new BeatDuration(beats, resolution);

		/// <summary>
		/// 原点からの拍数と小数点を組み合わせて BeatPoint を生成します。
		/// </summary>
		/// <param name="beat">
		/// 拍数です。
		/// </param>
		/// <param name="subBeat">
		/// 追加の分数可の拍数です。
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BeatPoint(int beat, in Rational subBeat)
			=> DurationFromZero = new BeatDuration(beat, subBeat);

		/// <summary>
		/// 原点からの拍数から BeatPoint を生成します。
		/// </summary>
		/// <param name="durationFromZero">
		/// 原点からの拍数です。
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BeatPoint(in BeatDuration durationFromZero)
			=> DurationFromZero = durationFromZero;

		/// <summary>
		/// A duration from the origin.
		/// 原点からの経過時間です。
		/// </summary>
		public readonly BeatDuration DurationFromZero;

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
		public static BeatPoint operator +(in BeatPoint x, in BeatDuration y)
			=> new BeatPoint(x.DurationFromZero + y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatPoint operator +(in BeatDuration y, in BeatPoint x)
			=> new BeatPoint(x.DurationFromZero + y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatPoint operator -(in BeatPoint x, in BeatDuration y)
			=> new BeatPoint(x.DurationFromZero - y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator -(in BeatPoint x, in BeatPoint y)
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

			return obj is BeatPoint point && Equals(point);
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

			return obj is BeatPoint point ? CompareTo(point) : throw new ArgumentException(nameof(obj));
		}

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(BeatPoint x) => this == x;

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(in BeatPoint x) => this == x;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(BeatPoint x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(in BeatPoint x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in BeatPoint x, in BeatPoint y)
			=> x.DurationFromZero == y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in BeatPoint x, in BeatPoint y)
			=> x.DurationFromZero != y.DurationFromZero;

		#endregion

		#region Compare operators.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(in BeatPoint x, in BeatPoint y)
			=> x.DurationFromZero < y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(in BeatPoint x, in BeatPoint y)
			=> x.DurationFromZero > y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(in BeatPoint x, in BeatPoint y)
			=> x.DurationFromZero <= y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(in BeatPoint x, in BeatPoint y)
			=> x.DurationFromZero >= y.DurationFromZero;

		#endregion
	}
}
