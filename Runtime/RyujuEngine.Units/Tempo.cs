// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Runtime.CompilerServices;

namespace RyujuEngine.Units
{
	/// <summary>
	/// A struct that contains a music tempo.
	/// テンポの速さを保持するクラスです。
	/// </summary>
	public readonly struct Tempo
	: IEquatable<Tempo>
	{
		/// <summary>
		/// An instance that indicates the stop.
		/// 停止を表すテンポです。
		/// </summary>
		public static readonly Tempo Zero = FromBPM(0.0f);

		/// <summary>
		/// An instance that indicates the one beat per minute.
		/// 1 分間に 1 拍のテンポを表すインスタンスです。
		/// </summary>
		public static readonly Tempo One = FromBPM(1.0f);

		/// <summary>
		/// Create an instance with the specified beats per minute.
		/// BPM でインスタンスを生成します。
		/// </summary>
		public static Tempo FromBPM(double beatsPerMinute) => new Tempo(beatsPerMinute);

		private Tempo(double beatsPerMinute) => BeatsPerMinute = beatsPerMinute;

		/// <summary>
		/// The beats per minute.
		/// 1 分間で刻まれる拍数です。
		/// </summary>
		public readonly double BeatsPerMinute;

		/// <summary>
		/// The beats per second.
		/// 1 秒間で刻まれる拍数です。
		/// </summary>
		public double BeatsPerSecond => BeatsPerMinute / 60.0;

		/// <summary>
		/// The seconds per beat.
		/// 1 拍で刻まれる秒数です。
		/// </summary>
		public TimeDuration DurationOfBeat => TimeDuration.OfSeconds(60.0 / BeatsPerMinute);

		/// <summary>
		/// A flag that indicates a negative tempo.
		/// テンポが負数かどうかを確かめます。
		/// </summary>
		public bool IsNegative => BeatsPerMinute < 0;

		/// <summary>
		/// An absolute value of  <see cref="BeatsPerSecond"/>.
		/// <see cref="BeatsPerSecond"/> の絶対値を返します。
		/// </summary>
		public double AbsBeatsPerSecond => IsNegative ? -BeatsPerSecond : BeatsPerSecond;

		/// <summary>
		/// An absolute value of <see cref="DurationOfBeat"/>.
		/// <see cref="DurationOfBeat"/> の絶対値を返します。
		/// </summary>
		public TimeDuration AbsDurationOfBeat => IsNegative ? -DurationOfBeat : DurationOfBeat;

		/// <summary>
		/// A hash value.
		/// ハッシュ値です。
		/// </summary>
		public override int GetHashCode() => BeatsPerMinute.GetHashCode();

		/// <summary>
		/// Detect the same values.
		/// オブジェクトが同値かどうかを確かめます。
		/// </summary>
		public override bool Equals(object obj)
			=> !(obj is null) && obj is Tempo tempo && tempo == this;

		/// <summary>
		/// Detect the same values.
		/// <see cref="Tempo"/> 同士が同値かどうかを確かめます。
		/// </summary>
		public bool Equals(Tempo tempo) => Equals(in tempo);

		/// <summary>
		/// Detect the same values.
		/// <see cref="Tempo"/> 同士が同値かどうかを確かめます。
		/// </summary>
		public bool Equals(in Tempo tempo) => tempo == this;

#if UNITY_EDITOR
		/// <summary>
		/// A string per debugging.
		/// デバッグ用の文字列表現です。
		/// </summary>
		public override string ToString() => $"{BeatsPerMinute:0.00}bpm";
#endif

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in Tempo x, in Tempo y) => x.BeatsPerMinute == y.BeatsPerMinute;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in Tempo x, in Tempo y) => x.BeatsPerMinute != y.BeatsPerMinute;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Tempo operator *(Tempo tempo, double scale)
			=> new Tempo(tempo.BeatsPerMinute * scale);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Tempo operator *(double scale, Tempo tempo)
			=> new Tempo(tempo.BeatsPerMinute * scale);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Tempo operator /(Tempo tempo, double divider)
			=> new Tempo(tempo.BeatsPerMinute / divider);
	}
}
