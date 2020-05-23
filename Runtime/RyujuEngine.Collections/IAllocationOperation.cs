// Copyright (c) 2020 Ryuju Orchestra
using System.Collections;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// An operation class to allocate an instance.
	/// インスタンス確保のための操作クラスです。
	/// </summary>
	public interface IAllocationOperation<out T>
	{
		/// <summary>
		/// An allocated instance.
		/// 確保されたインスタンスです。
		/// </summary>
		T Instance
		{
			get;
		}

		/// <summary>
		/// A flag that indicates the instance is already allocated.
		/// インスタンスが既に確保されているかどうかを表すフラグです。
		/// </summary>
		/// <value></value>
		bool IsDone
		{
			get;
		}

		/// <summary>
		/// A yield coroutine to wait for the allocation.
		/// 核を完了を待つ yield コルーチンです。
		/// </summary>
		IEnumerator Execute();
	}
}
