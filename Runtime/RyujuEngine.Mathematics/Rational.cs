// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Runtime.CompilerServices;

using UnityEngine.Assertions;

namespace RyujuEngine.Mathematics
{
	/// <summary>
	/// A struct that contains a rational number.
	/// 有理数を保持する構造体です。
	/// </summary>
	public struct Rational
	: IComparable
	, IComparable<Rational>
	, IComparable<int>
	, IEquatable<Rational>
	, IEquatable<int>
	{
		/// <summary>
		/// An instance that indicates 0.
		/// 0 を表す値です。
		/// </summary>
		public static readonly Rational Zero = new Rational(0, 1);

		/// <summary>
		/// An instance that indicates 1.
		/// 1 を表す値です。
		/// </summary>
		public static readonly Rational One = new Rational(1, 1);

		/// <summary>
		/// Create a new rational number.
		/// 有理数を生成します。
		/// </summary>
		/// <param name="numerator">
		/// A numerator.
		/// 分子です。
		/// </param>
		/// <param name="denominator">
		/// A denominator.
		/// 分母です。
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rational(long numerator, int denominator)
		{
			var gcd = (int)MathEx.GCD(numerator, denominator);
			var num = numerator / gcd;
			var den = denominator / gcd;
			if (denominator < 0)
			{
				num = -num;
				den = -den;
			}
			Numerator = num;
			Denominator = den;
		}

		/// <summary>
		/// A numerator.
		/// 分子です。
		/// </summary>
		public readonly long Numerator;

		/// <summary>
		/// A denominator.
		/// 分母です。
		/// </summary>
		public readonly int Denominator;

		/// <summary>
		/// The value in float type.
		/// float 型で表された値です。
		/// </summary>
		public float Float => Numerator / (float)Denominator;

		/// <summary>
		/// An integer part of this value.
		/// 有理数の整数部分です。
		/// </summary>
		public long IntegerPart => Numerator / Denominator;

		/// <summary>
		/// A fraction part of this value.
		/// 有理数の分数部分です。
		/// </summary>
		public Rational FractionPart => new Rational(Numerator - IntegerPart * Denominator, Denominator);

		/// <summary>
		/// A value that indicates {1 / plain_value}.
		/// 逆数です。
		/// </summary>
		public Rational Reciprocal
		{
			get
			{
				Assert.AreNotEqual(0, Numerator, "The Numerator must not be 0.");
				return new Rational(Denominator, (int)Numerator);
			}
		}

		/// <summary>
		/// A hash value.
		/// ハッシュ値を求めます。
		/// </summary>
		public override int GetHashCode()
			=> ((uint)Numerator * 0x2eb54dd1U).GetHashCode()
			^ ((ulong)Denominator * 0x7331e169a01cd0bfUL).GetHashCode();

#if UNITY_EDITOR
		/// <summary>
		/// A string for debugging.
		/// デバッグ用の文字列表現を返します。
		/// </summary>
		/// <returns></returns>
		public override string ToString() => $"{Numerator}/{Denominator}";
#endif

		#region Sign.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator +(in Rational x) => x;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator -(in Rational x) => new Rational(-x.Numerator, x.Denominator);

		#endregion

		#region Add.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator +(in Rational x, in Rational y)
			=> Add(x.Numerator, x.Denominator, y.Numerator, y.Denominator);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator +(in Rational x, int y)
			=> Add(x.Numerator, x.Denominator, y, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator +(int y, in Rational x)
			=> Add(x.Numerator, x.Denominator, y, 1);

		#endregion

		#region Sub.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator -(in Rational x, in Rational y)
			=> Add(x.Numerator, x.Denominator, -y.Numerator, y.Denominator);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator -(in Rational x, int y)
			=> Add(x.Numerator, x.Denominator, -y, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator -(int y, in Rational x)
			=> Add(-x.Numerator, x.Denominator, y, 1);

		#endregion

		#region Mul.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator *(in Rational x, in Rational y)
			=> Mul(x.Numerator, x.Denominator, y.Numerator, y.Denominator);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator *(in Rational x, int y)
			=> Mul(x.Numerator, x.Denominator, y, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator *(int y, in Rational x)
			=> Mul(x.Numerator, x.Denominator, y, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float operator *(in Rational x, float y)
			=> x.Numerator * y / x.Denominator;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float operator *(float y, in Rational x)
			=> x.Numerator * y / x.Denominator;

		#endregion

		#region Div.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator /(in Rational x, in Rational y)
			=> Mul(x.Numerator, x.Denominator, y.Denominator, y.Numerator);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator /(in Rational x, int y)
			=> Mul(x.Numerator, x.Denominator, 1, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rational operator /(int y, in Rational x)
			=> Mul(x.Denominator, x.Numerator, y, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float operator /(in Rational x, float y)
			=> x.Denominator * y / x.Numerator;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float operator /(float y, in Rational x)
			=> x.Denominator * y / x.Numerator;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]

		#endregion

		#region Equal and not equal.

		/// <summary>
		/// Detect the same value.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj is null)
			{
				return false;
			}

			if (obj is Rational rational)
			{
				return Equals(rational);
			}

			var boxedInt = obj as int?;
			if (boxedInt != null)
			{
				return Equals((int)boxedInt);
			}

			return false;
		}

		/// <summary>
		/// Detect the same value.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		public int CompareTo(object obj)
		{
			if (obj is null)
			{
				return 1;
			}

			if (obj is Rational rational)
			{
				return CompareTo(rational);
			}

			var boxedInt = obj as int?;
			if (boxedInt != null)
			{
				return CompareTo((int)boxedInt);
			}

			throw new ArgumentException(nameof(obj));
		}

		/// <summary>
		/// Detect the same value.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		public bool Equals(Rational x) => this == x;

		/// <summary>
		/// Detect the same value.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		public bool Equals(in Rational x) => this == x;

		/// <summary>
		/// Detect the same value.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		public bool Equals(int x) => this == x;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		public int CompareTo(Rational x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		public int CompareTo(in Rational x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		public int CompareTo(int x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in Rational x, in Rational y)
			=> x.Numerator == y.Numerator && x.Denominator == y.Denominator;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in Rational x, int y)
			=>  x.Numerator == y && x.Denominator == 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(int y, in Rational x)
			=> x == y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in Rational x, in Rational y)
			=> x.Numerator != y.Numerator || x.Denominator != y.Denominator;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in Rational x, int y)
			=> x.Numerator != y || x.Denominator != 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(int y, in Rational x)
			=> x != y;

		#endregion

		#region Less.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(in Rational x, in Rational y)
			=> Less(x.Numerator, x.Denominator, y.Numerator, y.Denominator);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(in Rational x, int y)
			=> Less(x.Numerator, x.Denominator, y, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(int y, in Rational x)
			=> Less(y, 1, x.Numerator, x.Denominator);

		#endregion

		#region Greater.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(in Rational x, in Rational y)
			=> Less(y.Numerator, y.Denominator, x.Numerator, x.Denominator);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(in Rational x, int y)
			=> Less(y, 1, x.Numerator, x.Denominator);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(int y, in Rational x)
			=> Less(x.Numerator, x.Denominator, y, 1);

		#endregion

		#region Less or equal.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(in Rational x, in Rational y)
			=> !Less(y.Numerator, y.Denominator, x.Numerator, x.Denominator);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(in Rational x, int y)
			=> !Less(y, 1, x.Numerator, x.Denominator);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(int y, in Rational x)
			=> !Less(x.Numerator, x.Denominator, y, 1);

		#endregion

		#region Greater or equal.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(in Rational x, in Rational y)
			=> !Less(x.Numerator, x.Denominator, y.Numerator, y.Denominator);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(in Rational x, int y)
			=> !Less(x.Numerator, x.Denominator, y, 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(int y, in Rational x)
			=> !Less(y, 1, x.Numerator, x.Denominator);

		#endregion

		#region Operator utilities.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Rational Add(long xn, int xd, long yn, int yd)
		{
			var gcd = MathEx.GCD(xd, yd);
			var xdGCD = xd / gcd;
			var ydGCD = yd / gcd;
			var num = xn * ydGCD + yn * xdGCD;
			var den = xd * ydGCD;
			return new Rational(num, den);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Rational Mul(long xn, long xd, long yn, long yd)
		{
			var gcdND = (int)MathEx.GCD(xn, yd);
			var gcdDN = (int)MathEx.GCD(xd, yn);
			var num = xn / gcdND * yn / gcdDN;
			var den = ((int)xd / gcdDN) * ((int)yd / gcdND);
			return new Rational(num, den);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool Less(long xn, long xd, long yn, long yd)
		{
			var gcd = MathEx.GCD(xd, yd);
			var xdGCD = xd / gcd;
			var ydGCD = yd / gcd;
			return xn * ydGCD < yn * xdGCD;
		}

		#endregion
	}
}
