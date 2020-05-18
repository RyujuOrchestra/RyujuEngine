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
	public readonly struct BeatPointFloat
	: IComparable
	, IComparable<BeatPointFloat>
	, IEquatable<BeatPointFloat>
	{
		/// <summary>
		/// An instance that indicates the origin position on a beat.
		/// 原点となる時刻です。
		/// </summary>
		public static readonly BeatPointFloat Zero = new BeatPointFloat(BeatDurationFloat.Zero);

		/// <summary>
		/// An instance that indicates positive infinite time.
		/// 正の無限大を表すインスタンスです。
		/// </summary>
		/// <returns></returns>
		public static readonly BeatPointFloat PositiveInfinity = new BeatPointFloat(BeatDurationFloat.PositiveInfinity);

		/// <summary>
		/// An instance that indicates negative infinite time.
		/// 負の無限大を表すインスタンスです。
		/// </summary>
		/// <returns></returns>
		public static readonly BeatPointFloat NegativeInfinity = new BeatPointFloat(BeatDurationFloat.NegativeInfinity);

		/// <summary>
		/// Create an instance with the specified beats count.
		/// 指定した拍数のインスタンスを生成します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatPointFloat At(float beats) => new BeatPointFloat(BeatDurationFloat.Of(beats));

		/// <summary>
		/// Create an instance with the specified duration from the origin.
		/// 原点からの拍数から BeatPointFloat を生成します。
		/// </summary>
		/// <param name="durationFromZero">原点からの拍数です。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatPointFloat At(in BeatDurationFloat durationFromZero)
			=> new BeatPointFloat(durationFromZero);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BeatPointFloat(in BeatDurationFloat durationFromZero)
			=> DurationFromZero = durationFromZero;

		/// <summary>
		/// A duration from the origin.
		/// 原点からの経過時間です。
		/// </summary>
		public readonly BeatDurationFloat DurationFromZero;

		/// <summary>
		/// A hash value.
		/// ハッシュ値を求めます。
		/// </summary>
		public override int GetHashCode() => DurationFromZero.GetHashCode();

		public static implicit operator BeatPointFloat(in BeatPoint time)
			=> BeatPointFloat.At(time.DurationFromZero.BeatPart + time.DurationFromZero.SubBeatPart.Float);

#if UNITY_EDITOR
		/// <summary>
		/// A string for debugging.
		/// デバッグ用の文字列表現を返します。
		/// </summary>
		public override string ToString() => DurationFromZero.ToString();
#endif

		#region Basic arithmetic operations.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatPointFloat operator +(in BeatPointFloat x, in BeatDurationFloat y)
			=> new BeatPointFloat(x.DurationFromZero + y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatPointFloat operator +(in BeatDurationFloat y, in BeatPointFloat x)
			=> new BeatPointFloat(x.DurationFromZero + y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatPointFloat operator -(in BeatPointFloat x, in BeatDurationFloat y)
			=> new BeatPointFloat(x.DurationFromZero - y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDurationFloat operator -(in BeatPointFloat x, in BeatPointFloat y)
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

			return obj is BeatPointFloat point && Equals(point);
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

			return obj is BeatPointFloat point ? CompareTo(point) : throw new ArgumentException(nameof(obj));
		}

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(BeatPointFloat x) => this == x;

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(in BeatPointFloat x) => this == x;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(BeatPointFloat x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(in BeatPointFloat x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in BeatPointFloat x, in BeatPointFloat y)
			=> x.DurationFromZero == y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in BeatPointFloat x, in BeatPointFloat y)
			=> x.DurationFromZero != y.DurationFromZero;

		#endregion

		#region Compare operators.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(in BeatPointFloat x, in BeatPointFloat y)
			=> x.DurationFromZero < y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(in BeatPointFloat x, in BeatPointFloat y)
			=> x.DurationFromZero > y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(in BeatPointFloat x, in BeatPointFloat y)
			=> x.DurationFromZero <= y.DurationFromZero;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(in BeatPointFloat x, in BeatPointFloat y)
			=> x.DurationFromZero >= y.DurationFromZero;

		#endregion
	}
}
