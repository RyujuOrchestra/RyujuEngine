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
		/// A maximum value.
		/// <see cref="BeatDuration"/>の最大拍数です。
		/// </summary>
		public static readonly BeatDuration Max = new BeatDuration(int.MaxValue, Rational.Zero);

		/// <summary>
		/// 浮動小数点で表された拍数を、指定した解像度で BeatDuration に変換します。
		/// </summary>
		/// <param name="beat">小数点以下も含む拍数です。</param>
		/// <param name="resolution">1 拍あたりの分割解像度です。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BeatDuration(double beat, uint resolution)
		{
			var b = (int)beat;
			var delta = beat - b;
			var s = new Rational((long)(delta * resolution + (delta >= 0 ? 0.5 : -0.5)), (int)resolution);
			Normalize(b, s, out Beat, out SubBeat);
		}

		/// <summary>
		/// 拍数と小数点を組み合わせて BeatDuration を生成します。
		/// </summary>
		/// <param name="beat">
		/// 拍数です。
		/// </param>
		/// <param name="subBeat">
		/// 追加の分数可の拍数です。
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BeatDuration(int beat, in Rational subBeat)
			=> Normalize(beat, subBeat, out Beat, out SubBeat);

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
		public readonly int Beat;

		/// <summary>
		/// The fraction part of the beats.
		/// 拍数の小数部です。
		/// </summary>
		public readonly Rational SubBeat;

		/// <summary>
		/// The value in float type.
		/// float 型で表された値です。
		/// </summary>
		public double Float => Beat + SubBeat.Float;

		/// <summary>
		/// The value in double type.
		/// double 型で表された値です。
		/// </summary>
		public double Double => Beat + SubBeat.Double;

#if UNITY_EDITOR
		/// <summary>
		/// A string for debugging.
		/// デバッグ用の文字列表現を返します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString() => $"{Beat}+({SubBeat})";
#endif

		#region Basic arithmetic operations.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator +(in BeatDuration x) => x;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator -(in BeatDuration x)
			=> new BeatDuration(-x.Beat, -x.SubBeat);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator +(in BeatDuration x, in BeatDuration y)
			=> new BeatDuration(x.Beat + y.Beat, x.SubBeat + y.SubBeat);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator -(in BeatDuration x, in BeatDuration y)
			=> new BeatDuration(x.Beat - y.Beat, x.SubBeat - y.SubBeat);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator *(in BeatDuration x, int y)
			=> new BeatDuration(x.Beat * y, x.SubBeat * y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator *(int y, in BeatDuration x)
			=> x * y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDuration operator *(in BeatDuration x, in Rational y)
		{
			var b = x.Beat * y;
			var s = x.SubBeat * y;
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
		public override int GetHashCode() => (int)((uint)Beat * 0x2ac60f42) ^ SubBeat.GetHashCode();

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
			=> x.Beat == y.Beat && x.SubBeat == y.SubBeat;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in BeatDuration x, in BeatDuration y)
			=> x.SubBeat != y.SubBeat || x.Beat != y.Beat;

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
			=> x.Beat < y.Beat || x.Beat == y.Beat && x.SubBeat < y.SubBeat;

		#endregion
	}
}
