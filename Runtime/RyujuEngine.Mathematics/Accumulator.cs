// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Collections.Generic;

using UnityEngine;

using RyujuEngine.Collections.Extension;

namespace RyujuEngine.Mathematics
{
	/// <summary>
	/// A class that accumulates the appended float values.
	/// 追加された float 値を合算するクラスです。
	/// </summary>
	public sealed class Accumulator
	{
		/// <summary>
		/// Append the value.
		/// 値を追加します。
		/// </summary>
		public void Add(float value) => _values.InsertWithBinarySearch(value, AbsComparer.Default);

		/// <summary>
		/// Get the total value.
		/// 合計値を取得します。
		/// </summary>
		public float Get(float offset = 0.0f)
		{
			if (float.IsPositiveInfinity(offset))
			{
				return float.PositiveInfinity;
			}
			if (float.IsNegativeInfinity(offset))
			{
				return float.NegativeInfinity;
			}

			var total = offset;
			var totalErr = 0.0f;
			var totalErr2 = 0.0f;
			foreach (var value in _values)
			{
				var nextSum = total + value;
				var err = Mathf.Abs(total) >= Mathf.Abs(value)
					// Lost the lower bits of the `value`.
					? value - (nextSum - total)
					// Lost the lower bits of the `sum`.
					: total - (nextSum - value)
					;
				total = nextSum;

				var nextTotalErr = totalErr + err;
				var err2 = Mathf.Abs(nextTotalErr) >= Mathf.Abs(err)
					// Lost the lower bits of the `err`.
					? err - (nextTotalErr - totalErr)
					// Lost the lower bits of the `totalErr`.
					: totalErr - (nextTotalErr - err)
					;
				totalErr = nextTotalErr;

				totalErr2 += err2;
			}
			return total + totalErr + totalErr2;
		}

		private sealed class AbsComparer : IComparer<float>
		{
			public static AbsComparer Default = new AbsComparer();

			public int Compare(float x, float y)
			{
				var absX = Mathf.Abs(x);
				var absY = Mathf.Abs(y);
				return absX > absY ? 1 :
				       absX < absY ? -1
				                   : 0
								   ;
			}
		}

		private readonly List<float> _values = new List<float>();
	}
}
