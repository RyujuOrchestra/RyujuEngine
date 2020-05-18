// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Runtime.CompilerServices;

using RyujuEngine.Mathematics;

namespace RyujuEngine.Units
{
	/// <summary>
	/// A struct that contains a duration between two beats position.
	/// 2 つのタイミングの間隔を拍数で表す構造体です。
	/// </summary>
	public readonly struct BeatDuration
	: IComparable
	, IComparable<BeatDuration>
	, IEquatable<BeatDuration>
	{
		/// <summary>
		/// An instance that indicates zero distance.
		/// 0 拍を表す値です。
		/// </summary>
		public static readonly BeatDuration Zero = new BeatDuration(0, Rational.Zero);

		/// <summary>
		/// An instance that indicates 1 beats distance.
		/// 1 拍を表す値です。
		/// </summary>
		public static readonly BeatDuration One = new BeatDuration(1, Rational.Zero);

		/// <summary>
		/// A minimum value.
		/// <see cref="BeatDuration"/>の最小拍数です。
		/// </summary>
		public static readonly BeatDuration Min = new BeatDuration(int.MinValue, Rational.Zero);

		/// <summary>
		/// A maximum value.
		/// <see cref="BeatDuration"/>の最大拍数です。
		/// </summary>
		public static readonly BeatDuration Max = new BeatDuration(int.MaxValue, Rational.Zero);

		/// <summary>
		/// Create an instance from floated beats using rationalization.
		/// 浮動小数点で表された拍数から、指定した解像度でインスタンスを生成します。
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
		public static BeatDuration Rationalize(BeatDurationFloat beats, uint resolution)
			=> new BeatDuration(beats, resolution);

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
		public static BeatDuration Of(int beats, long subBeatPosition, int subBeatResolution)
			=> new BeatDuration(beats, new Rational(subBeatPosition, subBeatResolution));

		/// <summary>
		/// Create an instance from the beats and sub-beats.
		/// 原点からの拍数と分数を組み合わせてインスタンスを生成します。
		/// </summary>
		/// <param name="beats">
		/// The beats.
		/// 拍数です。
		/// </param>
		/// <param name="subBeat">
		/// An additional beats from the `beats` parameter.
		/// 分数で表された追加の拍数です。
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration Of(int beats, in Rational subBeats)
			=> new BeatDuration(beats, subBeats);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private BeatDuration(BeatDurationFloat duration, uint resolution)
		{
			var b = (int)duration.Beats;
			var delta = duration.Beats - b;
			var s = new Rational((long)(delta * resolution + (delta >= 0 ? 0.5 : -0.5)), (int)resolution);
			Normalize(b, s, out BeatPart, out SubBeatPart);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private BeatDuration(int beats, in Rational subBeats)
			=> Normalize(beats, subBeats, out BeatPart, out SubBeatPart);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Normalize(int b, Rational s, out int beat, out Rational subBeat)
		{
			var normalizedBeat = (int)(b + s.IntegerPart);
			var normalizedSubBeat = s.FractionPart;
			if (normalizedBeat > 0 && normalizedSubBeat < 0)
			{
				normalizedBeat--;
				normalizedSubBeat += 1;
			}
			else if (normalizedBeat < 0 && normalizedSubBeat > 0)
			{
				normalizedBeat++;
				normalizedSubBeat -= 1;
			}
			beat = normalizedBeat;
			subBeat = normalizedSubBeat;
		}

		/// <summary>
		/// The integer part of the beats.
		/// 拍数の整数部です。
		/// </summary>
		public readonly int BeatPart;

		/// <summary>
		/// The fraction part of the beats.
		/// 拍数の小数部です。
		/// </summary>
		public readonly Rational SubBeatPart;

#if UNITY_EDITOR
		/// <summary>
		/// A string for debugging.
		/// デバッグ用の文字列表現を返します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString() => $"{BeatPart}+({SubBeatPart})";
#endif

		#region Basic arithmetic operations.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator +(in BeatDuration x) => x;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator -(in BeatDuration x)
			=> new BeatDuration(-x.BeatPart, -x.SubBeatPart);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator +(in BeatDuration x, in BeatDuration y)
			=> new BeatDuration(x.BeatPart + y.BeatPart, x.SubBeatPart + y.SubBeatPart);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator -(in BeatDuration x, in BeatDuration y)
			=> new BeatDuration(x.BeatPart - y.BeatPart, x.SubBeatPart - y.SubBeatPart);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator *(in BeatDuration x, int y)
			=> new BeatDuration(x.BeatPart * y, x.SubBeatPart * y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator *(int y, in BeatDuration x)
			=> x * y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator *(in BeatDuration x, in Rational y)
		{
			var b = x.BeatPart * y;
			var s = x.SubBeatPart * y;
			return new BeatDuration((int)(b.IntegerPart + s.IntegerPart), b.FractionPart + s.FractionPart);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator *(in Rational y, in BeatDuration x)
			=> x * y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator /(in BeatDuration x, in Rational y)
			=> x * y.Reciprocal;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator /(in BeatDuration x, int y)
			=> x * new Rational(1, y);

		#endregion

		#region Equal and not equal.

		/// <summary>
		/// A Hash value.
		/// ハッシュ値を求めます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode() => (int)((uint)BeatPart * 0x2ac60f42) ^ SubBeatPart.GetHashCode();

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

			return obj is BeatDuration duration && Equals(duration);
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

			return obj is BeatDuration duration ? CompareTo(duration) : throw new ArgumentException(nameof(obj));
		}

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(BeatDuration x) => this == x;

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(in BeatDuration x) => this == x;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(BeatDuration x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(in BeatDuration x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in BeatDuration x, in BeatDuration y)
			=> x.BeatPart == y.BeatPart && x.SubBeatPart == y.SubBeatPart;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in BeatDuration x, in BeatDuration y)
			=> x.SubBeatPart != y.SubBeatPart || x.BeatPart != y.BeatPart;

		#endregion

		#region Compare operators.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(in BeatDuration x, in BeatDuration y) => Less(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(in BeatDuration x, in BeatDuration y) => Less(y, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(in BeatDuration x, in BeatDuration y) => !Less(y, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(in BeatDuration x, in BeatDuration y) => !Less(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool Less(in BeatDuration x, in BeatDuration y)
			=> x.BeatPart < y.BeatPart || x.BeatPart == y.BeatPart && x.SubBeatPart < y.SubBeatPart;

		#endregion
	}
}
