// Copyright (c) 2020 Ryuju Orchestra
using System.Threading;

using UnityEngine;

using RyujuEngine.Units;

namespace RyujuEngine.Audio
{
	/// <summary>
	/// An audio player class that measures time exactly.
	/// 正確な再生時刻を算出する音声再生クラスです。
	/// </summary>
	public sealed class ExactTimeAudioPlayer
	{
		/// <summary>
		/// Create a new instance.
		/// 新しいインスタンスを生成します。
		/// </summary>
		/// <param name="speaker">
		/// An <see cref="AudioSource"/> instance for playing an <see cref="AudioClip"/>.
		/// <see cref="AudioClip"/> を再生させる <see cref="AudioSource"/> のインスタンスです。
		/// </param>
		public ExactTimeAudioPlayer(AudioSource speaker)
		{
			_speaker = speaker;
			_speaker.Stop();
		}

		/// <summary>
		/// A clip.
		/// 再生させるクリップです。
		/// </summary>
		public AudioClip Clip
		{
			get => _speaker.clip;
			set
			{
				Stop();
				_speaker.clip = value;
			}
		}

		/// <summary>
		/// A flag that indicates the player is playing the audio or not.
		/// 再生中かどうかを表すフラグです。
		/// </summary>
		public bool IsPlaying { get; private set; } = false;

		/// <summary>
		/// The length of the audio clip.
		/// クリップの長さを表す秒数です。
		/// </summary>
		public TimeDuration Duration => Clip != null
			? TimeDuration.OfSeconds(Clip.length)
			: TimeDuration.Zero;

		/// <summary>
		/// A current time that origin is the clip's 0 seconds.
		/// This value can take a negative value and an early time than the seek point.
		/// クリップの 0 秒目を原点とした再生時刻です。
		/// この値は 0 未満の値を取り、シーク先よりも前の時刻を取ることがあります。
		/// </summary>
		public TimePoint RawTime
		{
			get
			{
				if (!IsPlaying)
				{
					return _offset - _delay - _additionalDelay;
				}

				var cpuElapsed = TimePoint.AtSeconds(UnityEngine.Time.realtimeSinceStartup) - _startCpuTime;
				var dspElapsed = TimeDuration.OfSeconds((float)(AudioSettings.dspTime - _startDspSeconds));
				var diff = cpuElapsed - dspElapsed;
				var latency = AudioLatency.Duration;

				TimeDuration elapsed;
				if (diff < TimeDuration.Zero)
				{
					_startCpuTime += diff;
					elapsed = dspElapsed;
				}
				else if (diff > latency)
				{
					_startCpuTime += diff - latency;
					elapsed = dspElapsed + latency;
				}
				else
				{
					elapsed = cpuElapsed;
				}

				return _offset + elapsed - _delay - _additionalDelay;
			}
			set
			{
				if (value < TimePoint.Zero)
				{
					_offset = TimePoint.Zero;
					_delay = -value.DurationFromZero;
				}
				else
				{
					_offset = value;
					_delay = TimeDuration.Zero;
					_speaker.time
						= value.DurationFromZero < Duration
						? (float)value.DurationFromZero.Seconds
						: (float)Duration.Seconds
						;
				}
				RecordStartTime();
			}
		}

		/// <summary>
		/// A current time that origin is the clip's 0 seconds.
		/// This value can take a negative value but not an early time than the seek point.
		/// 遅延再生分を含めない再生時刻です。
		/// この値は 0 未満の値を取りえますが、シークした位置より前の値を示すことはありません。
		/// </summary>
		public TimePoint Time
		{
			get
			{
				var rawTime = RawTime;
				var time = rawTime >= _offset ? rawTime : _offset;
				return time;
			}
			set => RawTime = value;
		}

		/// <summary>
		/// A current time in the audio clip.
		/// This value can take from 0 to <see cref="Duration"/>.
		/// クリップ内での現在再生している時刻です。
		/// この値は 0 以上<see cref="Duration"/>以下の値を取ります。
		/// </summary>
		public TimePoint ClipTime
		{
			get
			{
				var time = Time;
				if (time.DurationFromZero >= Duration)
				{
					return TimePoint.At(Duration);
				}
				if (time <= _offset)
				{
					return _offset;
				}
				return time;
			}
		}

		/// <summary>
		/// Play the audio clip with the specified delay time.
		/// 指定した秒数だけ遅延させて再生します。
		/// </summary>
		/// <param name="delay">遅延させる秒数です。</param>
		public void Play(TimeDuration delay)
		{
			if (IsPlaying || !_speaker || !WaitForLoading())
			{
				return;
			}
			IsPlaying = true;

			_additionalDelay = delay;
			if (_offset.DurationFromZero < Duration)
			{
				_speaker.time = (float)_offset.DurationFromZero.Seconds;
				_speaker.PlayDelayed((float)(delay + _delay).Seconds);
			}
			RecordStartTime();
		}

		/// <summary>
		/// Pause the playing audio.
		/// 一時停止します。
		/// </summary>
		public void Pause()
		{
			if (!IsPlaying || !_speaker)
			{
				return;
			}
			IsPlaying = false;

			var nextDspSeconds = AudioSettings.dspTime + AudioLatency.Duration.Seconds;
			_speaker.Stop();
			var elapsed = TimeDuration.OfSeconds((float)(nextDspSeconds - _startDspSeconds));
			var isDelayPassed = elapsed >= _delay + _additionalDelay;
			var isAdditionalDelayPassed = elapsed >= _additionalDelay;
			_offset += isDelayPassed ? elapsed - _delay - _additionalDelay : TimeDuration.Zero;
			_speaker.time = (float)_offset.DurationFromZero.Seconds;
			_delay
				= isDelayPassed ? TimeDuration.Zero
				: isAdditionalDelayPassed ? _delay - (elapsed - _additionalDelay)
				: _delay
				;
			_additionalDelay = TimeDuration.Zero;
		}

		/// <summary>
		/// Stop playing audio.
		/// 再生を中止します。
		/// </summary>
		public void Stop()
		{
			_speaker.Stop();
			IsPlaying = false;
			RawTime = TimePoint.Zero;
			_delay = TimeDuration.Zero;
			_additionalDelay = TimeDuration.Zero;
			_offset = TimePoint.Zero;
		}

		private void RecordStartTime()
		{
			_startCpuTime = TimePoint.AtSeconds(UnityEngine.Time.realtimeSinceStartup);
			_startDspSeconds = AudioSettings.dspTime + AudioLatency.Duration.Seconds;
		}

		/// <summary>
		/// Wait for loading the clip synchronously.
		/// 読み込みが完了するまで同期的に待ちます。
		/// </summary>
		/// <returns>
		/// <see cref="true"/> if succeeded.
		/// <see cref="false"/> otherwise.
		/// 読み込にに成功した場合は<see cref="true"/>です。
		/// そうでなければ<see cref="false"/>です。
		/// </returns>
		private bool WaitForLoading()
		{
			if (Clip.loadState == AudioDataLoadState.Loading)
			{
				return true;
			}
			if (!Clip.LoadAudioData())
			{
				return false;
			}
			while (
				Clip.loadState == AudioDataLoadState.Unloaded
				|| Clip.loadState == AudioDataLoadState.Loading
			)
			{
				_ = Thread.Yield();
			}
			return true;
		}

		private readonly AudioSource _speaker;
		private TimeDuration _delay = TimeDuration.Zero;
		private TimeDuration _additionalDelay = TimeDuration.Zero;
		private TimePoint _offset = TimePoint.Zero;
		private TimePoint _startCpuTime = TimePoint.Zero;
		private double _startDspSeconds = 0.0f;
	}
}
