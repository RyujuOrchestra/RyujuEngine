// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Runtime.CompilerServices;

namespace RyujuEngine.Units
{
	/// <summary>
	/// A struct that contains a duration between two beats position.
	/// 2 つのタイミングの間隔を拍数で表す構造体です。
	/// </summary>
	public readonly struct BeatDurationFloat
	: IComparable
	, IComparable<BeatDurationFloat>
	, IEquatable<BeatDurationFloat>
	{
		/// <summary>
		/// An instance that indicates zero distance.
		/// 0 拍を表すインスタンスです。
		/// </summary>
		public static readonly BeatDurationFloat Zero = new BeatDurationFloat(0.0f);

		/// <summary>
		/// An instance that indicates 1 beats distance.
		/// 1 拍を表すインスタンスです。
		/// </summary>
		public static readonly BeatDurationFloat One = new BeatDurationFloat(1.0f);

		/// <summary>
		/// An instance that indicates positive infinite distance.
		/// 正の無限大を表すインスタンスです。
		/// </summary>
		public static readonly BeatDurationFloat PositiveInfinity = new BeatDurationFloat(float.PositiveInfinity);

		/// <summary>
		/// An instance that indicates negative infinite distance.
		/// 負の無限大を表すインスタンスです。
		/// </summary>
		public static readonly BeatDurationFloat NegativeInfinity = new BeatDurationFloat(float.NegativeInfinity);

		/// <summary>
		/// Create an instance with the specified beats count.
		/// 指定した拍数のインスタンスを生成します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDurationFloat Of(float beats) => new BeatDurationFloat(beats);

		/// <summary>
		/// Create an instance.
		/// 新しいインスタンスを生成します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private BeatDurationFloat(float beats) => Beats = beats;

		/// <summary>
		/// A duration in a beat.
		/// 拍数で表された時間です。
		/// </summary>
		public readonly float Beats;

		public static implicit operator BeatDurationFloat(in BeatDuration duration)
			=> BeatDurationFloat.Of(duration.BeatPart + duration.SubBeatPart.Float);

#if UNITY_EDITOR
		/// <summary>
		/// デバッグ用の文字列表現です。
		/// </summary>
		public override string ToString() => $"{Beats:0.000}beats";
#endif

		#region Basic arithmetic operations.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDurationFloat operator +(BeatDurationFloat x) => x;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDurationFloat operator -(BeatDurationFloat x) => new BeatDurationFloat(-x.Beats);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDurationFloat operator +(in BeatDurationFloat x, in BeatDurationFloat y)
			=> new BeatDurationFloat(x.Beats + y.Beats);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDurationFloat operator -(in BeatDurationFloat x, in BeatDurationFloat y)
			=> new BeatDurationFloat(x.Beats - y.Beats);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDurationFloat operator *(in BeatDurationFloat x, float y)
			=> new BeatDurationFloat(x.Beats * y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDurationFloat operator *(float y, in BeatDurationFloat x)
			=> new BeatDurationFloat(x.Beats * y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BeatDurationFloat operator /(in BeatDurationFloat x, float y)
			=> new BeatDurationFloat(x.Beats / y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float operator %(in BeatDurationFloat x, in BeatDurationFloat y)
			=> x.Beats % y.Beats;

		#endregion

		#region Equal and not equal.

		/// <summary>
		/// A hash value.
		/// ハッシュ値を求めます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode() => Beats.GetHashCode();

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

			return obj is BeatDurationFloat duration && Equals(duration);
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

			return obj is BeatDurationFloat duration ? CompareTo(duration) : throw new ArgumentException(nameof(obj));
		}

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(BeatDurationFloat x) => this == x;

		/// <summary>
		/// Detect the same values.
		/// 同じ値かどうかを確かめます。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(in BeatDurationFloat x) => this == x;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(BeatDurationFloat x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		/// <summary>
		/// Compare the values.
		/// 値を比較します。
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(in BeatDurationFloat x)
			=> this < x ? -1 :
			   this == x ? 0 :
			   1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in BeatDurationFloat x, in BeatDurationFloat y) => x.Beats == y.Beats;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in BeatDurationFloat x, in BeatDurationFloat y) => x.Beats != y.Beats;

		#endregion

		#region Compare operators.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(in BeatDurationFloat x, in BeatDurationFloat y) => Less(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(in BeatDurationFloat x, in BeatDurationFloat y) => Less(y, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(in BeatDurationFloat x, in BeatDurationFloat y) => !Less(y, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(in BeatDurationFloat x, in BeatDurationFloat y) => !Less(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool Less(in BeatDurationFloat x, in BeatDurationFloat y) => x.Beats < y.Beats;

		#endregion
	}
}
