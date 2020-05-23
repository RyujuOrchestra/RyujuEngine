// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Collections;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// A allocate class that will create an instance with a function.
	/// 関数を使用してインスタンスを作成するアロケーションクラスです。
	/// </summary>
	public sealed class DeferredAllocationOperation<T> : IAllocationOperation<T>
	{
		/// <summary>
		/// Create an instance with the allocation function.
		/// アロケーション関数とともにインスタンスを生成します。
		/// </summary>
		public DeferredAllocationOperation(Func<T> allocate)
			=> _allocate = allocate;

		/// <inheritdoc/>
		public T Instance
		{
			get;
			private set;
		} = default;

		/// <inheritdoc/>
		public bool IsDone
		{
			get;
			private set;
		} = false;

		/// <inheritdoc/>
		public IEnumerator Execute()
		{
			Instance = _allocate();
			IsDone = true;
			yield break;
		}

		private readonly Func<T> _allocate;
	}
}
