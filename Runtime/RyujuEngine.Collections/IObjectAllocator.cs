// Copyright (c) 2020 Ryuju Orchestra
using System;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// An allocator interface that can operate the specified type instances.
	/// 指定した型のインスタンスを操作できるアロケーターインターフェイスです。
	/// </summary>
	public interface IObjectAllocator<T> : IDisposable
	{
		/// <summary>
		/// Create an instance.
		/// インスタンスを生成します。
		/// </summary>
		T Allocate();

		/// <summary>
		/// Create a yield coroutine to wait for creation.
		/// 生成を待つ yield コルーチンを作成します。
		/// </summary>
		/// <returns>
		/// </returns>
		IAllocationOperation<T> CreateAllocationOperator();

		/// <summary>
		///
		/// </summary>
		/// <param name="instance">
		/// </param>
		void Activate(T instance);

		/// <summary>
		///
		/// </summary>
		/// <param name="instance">
		/// </param>
		void Deactivate(T instance);

		/// <summary>
		///
		/// </summary>
		/// <param name="instance">
		/// </param>
		void Release(T instance);
	}
}
