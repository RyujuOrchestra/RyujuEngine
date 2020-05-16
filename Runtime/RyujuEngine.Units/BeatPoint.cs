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
		public static readonly BeatPoint Zero = new BeatPoint(BeatDuration.Zero);

		/// <summary>
		/// A minimum value.
		/// 扱える最小の時刻です。
		/// </summary>
		public static readonly BeatPoint Min = new BeatPoint(BeatDuration.Min);

		/// <summary>
		/// A maximum value.
		/// 扱える最大の時刻です。
		/// </summary>
		public static readonly BeatPoint Max = new BeatPoint(BeatDuration.Max);

		/// <summary>
		/// Create an instance from floated beats using rationalization.
		/// 浮動小数点で表された原点からの拍数から、指定した解像度でインスタンスを生成します。
		/// </summary>
		/// <param name="beats">
		/// A floated beats.
		/// 小数点以下も含む拍数です。
		/// </param>
		/// <param name="resolution">
		/// A resolution in a beat.
		/// 1 拍あたりの分割解像度です。
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatPoint Rationalize(BeatPointFloat beats, uint resolution)
			=> new BeatPoint(BeatDuration.Rationalize(beats.DurationFromZero, resolution));

		/// <summary>
		/// Create an instance from the beats and sub-beats.
		/// 原点からの拍数と分数を組み合わせてインスタンスを生成します。
		/// </summary>
		/// <param name="beats">
		/// The beats.
		/// 拍数です。
		/// </param>
		/// <param name="subBeatPosition">
		/// An additional beats from the `beats` parameter.
		/// 分数で表された追加の拍数です。
		/// </param>
		/// <param name="subBeatResolution">
		/// An resolution of the sub-beats.
		/// sub-beat の解像度です。
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatPoint At(int beats, long subBeatPosition, int subBeatResolution)
			=> new BeatPoint(BeatDuration.From(beats, subBeatPosition, subBeatResolution));

		/// <summary>
		/// Create an instance with the specified duration from the origin.
		/// 原点からの秒数からインスタンスを生成します。
		/// </summary>
		/// <param name="durationFromZero">
		/// A duration from zero.
		/// 原点からの時間です。
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatPoint FromZeroTo(in BeatDuration durationFromZero) => new BeatPoint(durationFromZero);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private BeatPoint(in BeatDuration durationFromZero)
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
