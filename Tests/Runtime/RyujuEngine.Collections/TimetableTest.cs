// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using RyujuEngine.Collections;
using RyujuEngine.Units;

namespace Collections
{
	public sealed class TimetableTest
	{
		[Test]
		public void It_should_be_able_to_get_a_time_point_from_only_the_default_tempo()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			Assert.That(
				timetable.GetTimeAt(BeatPoint.Zero),
				Is.EqualTo(TimePoint.Zero),
				"Invalid at 0.0 beats."
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(4, 0, 1)),
				Is.EqualTo(TimePoint.AtSeconds(2.0f)),
				"Invalid at 4.0 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(5, 1, 2)),
				Is.EqualTo(TimePoint.AtSeconds(2.75f)),
				"Invalid at 5.5 beats"
			);
		}

		[Test]
		public void It_should_be_able_to_get_a_beat_count_from_only_the_default_tempo()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.Zero),
				Is.EqualTo(BeatPointFloat.Zero),
				"Invalid at 0.0 sec."
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(2.0f)),
				Is.EqualTo(BeatPointFloat.At(4.0f)),
				"Invalid at 2.0 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(2.75f)),
				Is.EqualTo(BeatPointFloat.At(5.5f)),
				"Invalid at 2.65 sec"
			);
		}

		[Test]
		public void It_should_be_able_to_get_a_sequence_position_from_only_the_default_tempo()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.Zero),
				Is.EqualTo(BeatPointFloat.Zero),
				"Invalid at 0.0 sec."
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(2.0f)),
				Is.EqualTo(BeatPointFloat.At(4.0f)),
				"Invalid at 2.0 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(2.75f)),
				Is.EqualTo(BeatPointFloat.At(5.5f)),
				"Invalid at 2.65 sec"
			);
		}

		[Test]
		public void It_should_be_able_to_get_a_time_point_from_only_the_positive_tempo_controls()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			timetable.Add(BeatPoint.At(4, 0, 1), Tempo.FromBPM(60));
			timetable.Add(BeatPoint.At(6, 0, 1), Tempo.FromBPM(30));
			timetable.Add(BeatPoint.At(7, 0, 1), Tempo.FromBPM(15));
			Assert.That(
				timetable.GetTimeAt(BeatPoint.Zero),
				Is.EqualTo(TimePoint.Zero),
				"Invalid at 0.0 beats."
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(0, 1, 2)),
				Is.EqualTo(TimePoint.AtSeconds(0.25f)),
				"Invalid at 0.5 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(4, 0, 1)),
				Is.EqualTo(TimePoint.AtSeconds(2.0f)),
				"Invalid at 4.0 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(5, 0, 1)),
				Is.EqualTo(TimePoint.AtSeconds(3.0f)),
				"Invalid at 5.0 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(6, 3, 4)),
				Is.EqualTo(TimePoint.AtSeconds(5.5f)),
				"Invalid at 6.75 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(8, 0, 1)),
				Is.EqualTo(TimePoint.AtSeconds(10.0f)),
				"Invalid at 8.0 beats"
			);
		}

		[Test]
		public void It_should_be_able_to_get_a_beat_count_from_only_the_positive_tempo_controls()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			timetable.Add(BeatPoint.At(4, 0, 1), Tempo.FromBPM(60));
			timetable.Add(BeatPoint.At(6, 0, 1), Tempo.FromBPM(30));
			timetable.Add(BeatPoint.At(7, 0, 1), Tempo.FromBPM(15));
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.Zero),
				Is.EqualTo(BeatPointFloat.Zero),
				"Invalid at 0.0 sec."
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(0.25f)),
				Is.EqualTo(BeatPointFloat.At(0.5f)),
				"Invalid at 0.25 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(2.0f)),
				Is.EqualTo(BeatPointFloat.At(4.0f)),
				"Invalid at 2.0 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(3.0f)),
				Is.EqualTo(BeatPointFloat.At(5.0f)),
				"Invalid at 3.0 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(5.5f)),
				Is.EqualTo(BeatPointFloat.At(6.75f)),
				"Invalid at 4.75 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(10.0f)),
				Is.EqualTo(BeatPointFloat.At(8.0f)),
				"Invalid at 10.0 sec"
			);
		}

		[Test]
		public void It_should_be_able_to_get_a_sequence_position_from_only_the_positive_tempo_controls()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			timetable.Add(BeatPoint.At(4, 0, 1), Tempo.FromBPM(60));
			timetable.Add(BeatPoint.At(6, 0, 1), Tempo.FromBPM(30));
			timetable.Add(BeatPoint.At(7, 0, 1), Tempo.FromBPM(15));
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.Zero),
				Is.EqualTo(BeatPointFloat.Zero),
				"Invalid at 0.0 sec."
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(0.25f)),
				Is.EqualTo(BeatPointFloat.At(0.5f)),
				"Invalid at 0.25 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(2.0f)),
				Is.EqualTo(BeatPointFloat.At(4.0f)),
				"Invalid at 2.0 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(3.0f)),
				Is.EqualTo(BeatPointFloat.At(5.0f)),
				"Invalid at 3.0 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(5.5f)),
				Is.EqualTo(BeatPointFloat.At(6.75f)),
				"Invalid at 4.75 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(10.0f)),
				Is.EqualTo(BeatPointFloat.At(8.0f)),
				"Invalid at 10.0 sec"
			);
		}

		[Test]
		public void It_should_be_able_to_get_a_time_point_from_the_reversed_tempo_controls()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			timetable.Add(BeatPoint.At(4, 0, 1), Tempo.FromBPM(-60));
			timetable.Add(BeatPoint.At(6, 0, 1), Tempo.FromBPM(30));
			timetable.Add(BeatPoint.At(7, 0, 1), Tempo.FromBPM(-15));
			Assert.That(
				timetable.GetTimeAt(BeatPoint.Zero),
				Is.EqualTo(TimePoint.Zero),
				"Invalid at 0.0 beats."
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(0, 1, 2)),
				Is.EqualTo(TimePoint.AtSeconds(0.25f)),
				"Invalid at 0.5 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(4, 0, 1)),
				Is.EqualTo(TimePoint.AtSeconds(2.0f)),
				"Invalid at 4.0 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(5, 0, 1)),
				Is.EqualTo(TimePoint.AtSeconds(3.0f)),
				"Invalid at 5.0 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(6, 3, 4)),
				Is.EqualTo(TimePoint.AtSeconds(5.5f)),
				"Invalid at 6.75 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(8, 0, 1)),
				Is.EqualTo(TimePoint.AtSeconds(10.0f)),
				"Invalid at 8.0 beats"
			);
		}

		[Test]
		public void It_should_be_able_to_get_a_beat_count_from_the_reversed_tempo_controls()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			timetable.Add(BeatPoint.At(4, 0, 1), Tempo.FromBPM(-60));
			timetable.Add(BeatPoint.At(6, 0, 1), Tempo.FromBPM(30));
			timetable.Add(BeatPoint.At(7, 0, 1), Tempo.FromBPM(-15));
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.Zero),
				Is.EqualTo(BeatPointFloat.Zero),
				"Invalid at 0.0 sec."
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(0.25f)),
				Is.EqualTo(BeatPointFloat.At(0.5f)),
				"Invalid at 0.25 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(2.0f)),
				Is.EqualTo(BeatPointFloat.At(4.0f)),
				"Invalid at 2.0 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(3.0f)),
				Is.EqualTo(BeatPointFloat.At(5.0f)),
				"Invalid at 3.0 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(5.5f)),
				Is.EqualTo(BeatPointFloat.At(6.75f)),
				"Invalid at 5.5 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(10.0f)),
				Is.EqualTo(BeatPointFloat.At(8.0f)),
				"Invalid at 10.0 sec"
			);
		}

		[Test]
		public void It_should_be_able_to_get_a_sequence_position_from_the_reversed_tempo_controls()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			timetable.Add(BeatPoint.At(4, 0, 1), Tempo.FromBPM(-60));
			timetable.Add(BeatPoint.At(6, 0, 1), Tempo.FromBPM(30));
			timetable.Add(BeatPoint.At(7, 0, 1), Tempo.FromBPM(-15));
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.Zero),
				Is.EqualTo(BeatPointFloat.Zero),
				"Invalid at 0.0 sec."
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(0.25f)),
				Is.EqualTo(BeatPointFloat.At(0.5f)),
				"Invalid at 0.25 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(2.0f)),
				Is.EqualTo(BeatPointFloat.At(4.0f)),
				"Invalid at 2.0 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(3.0f)),
				Is.EqualTo(BeatPointFloat.At(3.0f)),
				"Invalid at 3.0 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(5.5f)),
				Is.EqualTo(BeatPointFloat.At(2.75f)),
				"Invalid at 5.5 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(10.0f)),
				Is.EqualTo(BeatPointFloat.At(2.0f)),
				"Invalid at 10.0 sec"
			);
		}

		[Test]
		public void It_should_be_able_to_get_a_time_point_from_the_stop_sequence_controls()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			timetable.Add(BeatPoint.At(4, 0, 1), Tempo.FromBPM(-60), BeatDuration.One);
			timetable.Add(BeatPoint.At(6, 0, 1), Tempo.FromBPM(30), BeatDuration.One);
			timetable.Add(BeatPoint.At(7, 0, 1), Tempo.FromBPM(-15), BeatDuration.One);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.Zero),
				Is.EqualTo(TimePoint.Zero),
				"Invalid at 0.0 beats."
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(0, 1, 2)),
				Is.EqualTo(TimePoint.AtSeconds(0.25f)),
				"Invalid at 0.5 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(4, 0, 1)),
				Is.EqualTo(TimePoint.AtSeconds(2.0f)),
				"Invalid at 4.0 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(5, 0, 1)),
				Is.EqualTo(TimePoint.AtSeconds(4.0f)),
				"Invalid at 5.0 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(6, 3, 4)),
				Is.EqualTo(TimePoint.AtSeconds(8.5f)),
				"Invalid at 6.75 beats"
			);
			Assert.That(
				timetable.GetTimeAt(BeatPoint.At(8, 0, 1)),
				Is.EqualTo(TimePoint.AtSeconds(17.0f)),
				"Invalid at 8.0 beats"
			);
		}

		[Test]
		public void It_should_be_able_to_get_a_beat_count_from_the_stop_sequence_controls()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			timetable.Add(BeatPoint.At(4, 0, 1), Tempo.FromBPM(-60), BeatDuration.One);
			timetable.Add(BeatPoint.At(6, 0, 1), Tempo.FromBPM(30), BeatDuration.One);
			timetable.Add(BeatPoint.At(7, 0, 1), Tempo.FromBPM(-15), BeatDuration.One);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.Zero),
				Is.EqualTo(BeatPointFloat.Zero),
				"Invalid at 0.0 sec."
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(0.25f)),
				Is.EqualTo(BeatPointFloat.At(0.5f)),
				"Invalid at 0.25 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(2.0f)),
				Is.EqualTo(BeatPointFloat.At(4.0f)),
				"Invalid at 2.0 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(4.0f)),
				Is.EqualTo(BeatPointFloat.At(5.0f)),
				"Invalid at 4.0 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(8.5f)),
				Is.EqualTo(BeatPointFloat.At(6.75f)),
				"Invalid at 8.5 sec"
			);
			Assert.That(
				timetable.GetBeatTimeAt(TimePoint.AtSeconds(12.0f)),
				Is.EqualTo(BeatPointFloat.At(7.0f)),
				"Invalid at 12.0 sec"
			);
		}

		[Test]
		public void It_should_be_able_to_get_a_sequence_position_from_the_stop_sequence_controls()
		{
			var timetable = new Timetable(Tempo.FromBPM(120));
			timetable.Add(BeatPoint.At(4, 0, 1), Tempo.FromBPM(-60), BeatDuration.One);
			timetable.Add(BeatPoint.At(6, 0, 1), Tempo.FromBPM(30), BeatDuration.One);
			timetable.Add(BeatPoint.At(7, 0, 1), Tempo.FromBPM(-15), BeatDuration.One);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.Zero),
				Is.EqualTo(BeatPointFloat.Zero),
				"Invalid at 0.0 sec."
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(0.25f)),
				Is.EqualTo(BeatPointFloat.At(0.5f)),
				"Invalid at 0.25 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(2.0f)),
				Is.EqualTo(BeatPointFloat.At(4.0f)),
				"Invalid at 2.0 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(4.0f)),
				Is.EqualTo(BeatPointFloat.At(3.0f)),
				"Invalid at 4.0 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(8.5f)),
				Is.EqualTo(BeatPointFloat.At(2.75f)),
				"Invalid at 8.5 sec"
			);
			Assert.That(
				timetable.GetSequencePositionAt(TimePoint.AtSeconds(12.0f)),
				Is.EqualTo(BeatPointFloat.At(3.0f)),
				"Invalid at 12.0 sec"
			);
		}
	}
}
