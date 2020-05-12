// Copyright (c) 2020 Ryuju Orchestra
using UnityEngine;

using RyujuEngine.Units;

namespace RyujuEngine.Audio
{
	/// <summary>
	/// A class that minimizes the audio latency and measures it.
	/// 音声の発声遅延を最小にし、計測するクラスです。
	/// </summary>
	public static class AudioLatency
	{
		/// <summary>
		/// The audio latency.
		/// 発声の遅延量です。
		/// </summary>
		public static TimeDuration Duration
		{
			get;
			private set;
		} = TimeDuration.OfSeconds(0.0);

		/// <summary>
		/// A flag that indicates whether the audio latency is being minimized or not.
		/// レイテンシを最小化中かどうかを表すフラグです。
		/// </summary>
		public static bool IsMinimizing
		{
			get;
			private set;
		} = false;

		/// <summary>
		/// An initializer.
		/// 起動時の初期化を行います。
		/// </summary>
		[RuntimeInitializeOnLoadMethod]
		internal static void Initialize()
		{
			AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;
		}

		/// <summary>
		/// Minimize the audio latency.
		/// 遅延量の最小化を行います。
		/// </summary>
		public static void Minimize()
		{
			IsMinimizing = true;
			Duration = TimeDuration.Zero;
			foreach (var candidateSize in BufferSizeCandidates)
			{
				var config = AudioSettings.GetConfiguration();
				config.dspBufferSize = candidateSize;
				_ = AudioSettings.Reset(config);
			}
			IsMinimizing = false;
		}

		/// <summary>
		/// Called when the audio settings were changed.
		/// 音声設定が変更されたときに呼び出されます。
		/// </summary>
		/// <param name="deviceWasChanged">
		/// A flag that indicates the event was dispatched by device changing.
		/// このイベントの発火はデバイス変更によるものかを表すフラグです。
		/// </param>
		private static void OnAudioConfigurationChanged(bool deviceWasChanged)
		{
			if (deviceWasChanged)
			{
				Minimize();
			}
			else
			{
				var config = AudioSettings.GetConfiguration();
				var newDuration = TimeDuration.OfSeconds((double)config.dspBufferSize / config.sampleRate);
				if (Duration <= TimeDuration.Zero || newDuration < Duration)
				{
					Duration = newDuration;
				}
			}
		}

		/// <summary>
		/// The candidates of the audio buffer size.
		/// 音声バッファサイズの候補一覧です。
		/// </summary>
		private static readonly int[] BufferSizeCandidates = new[]
		{
			2048,
			1280,
			1024,
			960,
			720,
			512,
			480,
		};
	}
}
