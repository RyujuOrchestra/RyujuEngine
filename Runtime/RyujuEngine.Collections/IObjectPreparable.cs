// Copyright (c) 2020 Ryuju Orchestra
using System;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// A part of the ObjectPool interface to enable to prepare.
	/// Prepare できるための ObjectPool インターフェイスの一部です。
	/// </summary>
	public interface IObjectPreparable
	{
		/// <summary>
		/// The number of created instances.
		/// 生成済みのインスタンスの数です。
		/// </summary>
		int Count
		{
			get;
		}

		/// <summary>
		/// Create the instances and prepare these for use.
		/// インスタンスを生成し使えるように準備します。
		/// </summary>
		/// <param name="count">
		/// The number of instances.
		/// インスタンスの数です。
		/// </param>
		void Prepare(int count);

		/// <summary>
		/// Create the instances frame by frame and prepare these for use.
		/// インスタンスをフレームごとに生成し使えるように準備します。
		/// </summary>
		/// <param name="count">
		/// The number of instances.
		/// インスタンスの数です。
		/// </param>
		/// <param name="maxCreationPerFrame">
		/// The maximum count of creating instances per frame.
		/// フレームごとのインスタンス生成の最大数です。
		/// </param>
		/// <returns>
		/// A yield coroutine to wait for the instance creation.
		/// インスタンス生成完了を待つための yield コルーチンです。
		/// </returns>
		AsyncActionOperation PrepareAsync(int count, int maxCreationPerFrame);
	}
}
