// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Collections;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// The class that contains the progress of the action.
	/// Action の進捗を保持するクラスです。
	/// </summary>
	public sealed class AsyncActionOperation
	{
		/// <summary>
		/// An event that is dispatched on progress updating.
		/// 進捗度が更新されたときに発火するイベントです。
		/// </summary>
		public event Action<float> ProgressEvent;

		/// <summary>
		/// An event that is dispatched on canceled.
		/// キャンセルされた時に発火するイベントです。
		/// </summary>
		public event Action CanceledEvent;

		/// <summary>
		/// An event that is dispatched on completed.
		/// 完了したときに発火するイベントです。
		/// </summary>
		public event Action CompletedEvent;

		/// <summary>
		/// A flag that indicates the action is done.
		/// アクションが終了したことを表すフラグです。
		/// </summary>
		public bool IsDone
		{
			get;
			private set;
		} = false;

		/// <summary>
		/// A flag that indicates the action is completed.
		/// アクションが完了したことを表すフラグです。
		/// </summary>
		public bool IsCompleted
		{
			get;
			private set;
		} = false;

		/// <summary>
		/// A flag that indicates the action is canceled.
		/// アクションがキャンセルされたことを表すフラグです。
		/// </summary>
		public bool IsCanceled
		{
			get;
			private set;
		} = false;

		/// <summary>
		/// The progress rate between 0.0f to 1.0f.
		/// 0.0f から 1.0f で表される進捗率です。
		/// </summary>
		public float Progress
		{
			get;
			private set;
		} = 0.0f;

		/// <summary>
		/// Wait until the action is done.
		/// アクションが完了するまで待機します。
		/// </summary>
		public IEnumerator WaitAsync()
		{
			while (!IsDone)
			{
				yield return null;
			}
		}

		/// <summary>
		/// Cancel the action.
		/// アクションをキャンセルします。
		/// </summary>
		public void Cancel()
		{
			IsCanceled = true;
		}

		/// <summary>
		/// Update the progress rate.
		/// 進捗率を更新します。
		/// </summary>
		public void SetProgress(float progress)
		{
			Progress = progress;
			ProgressEvent?.Invoke(progress);
		}

		/// <summary>
		/// Mark the action as canceled.
		/// このアクションをキャンセル済みとします。
		/// </summary>
		public void MarkAsCanceled()
		{
			IsCanceled = true;
			IsDone = true;
			CanceledEvent?.Invoke();
		}

		/// <summary>
		/// Mark the action as completed.
		/// このアクションを完了済みとします。
		/// </summary>
		public void MarkAsCompleted()
		{
			Progress = 1.0f;
			IsCompleted = true;
			IsDone = true;
			CompletedEvent?.Invoke();
		}
	}
}
