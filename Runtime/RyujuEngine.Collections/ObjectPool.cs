// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// A class that caches the specified type instances to reuse.
	/// 再利用できるように指定した肩のインスタンスきキャッシュするクラスです。
	/// </summary>
	public sealed class ObjectPool<T> : IDisposable
	{
		/// <summary>
		/// Create an instance with the instance allocator.
		/// インスタンスアロケーターを用いてインスタンスを生成します。
		/// </summary>
		/// <param name="allocator">
		/// An instance allocator to create and destroy.
		/// 作成及び破棄するためのインスタンスアロケーターです。
		/// </param>
		public ObjectPool(IObjectAllocator<T> allocator)
		{
			_allocator = allocator;
		}

		/// <summary>
		/// このインスタンスを破棄します。
		/// </summary>
		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;

			foreach (var instance in _pool)
			{
				_allocator.Deactivate(instance);
				_allocator.Release(instance);
			}
			_allocator.Dispose();
			_pool.Clear();
		}

		/// <summary>
		/// The number of created instances.
		/// 生成済みのインスタンスの数です。
		/// </summary>
		public int Count => _pool.Count;

		/// <summary>
		/// Create the instances and prepare these for use.
		/// インスタンスを生成し使えるように準備します。
		/// </summary>
		/// <param name="count">
		/// The number of instances.
		/// インスタンスの数です。
		/// </param>
		public void Prepare(int count)
		{
			for (int i = 0; i < count; i++)
			{
				_pool.Enqueue(_allocator.Allocate());
			}
		}

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
		public AsyncActionOperation PrepareAsync(int count, int maxCreationPerFrame)
		{
			var operation = new AsyncActionOperation();
			var coroutine
				= SingletonGameObject.StartCoroutine(PrepareAsyncCoroutine(count, maxCreationPerFrame, operation));
			operation.CompletedEvent += () => _coroutines.Remove(coroutine);
			operation.CanceledEvent += () => _coroutines.Remove(coroutine);
			return operation;
		}

		private IEnumerator PrepareAsyncCoroutine(int count, int maxCreationPerFrame, AsyncActionOperation operation)
		{
			yield return new WaitForEndOfFrame();

			var rest = count;
			var perFrame = maxCreationPerFrame > 0 ? maxCreationPerFrame : 1;
			var repeatFrameCount = (count + perFrame - 1) / perFrame;
			for (var frame = 0; frame < repeatFrameCount; frame++)
			{
				var frameNumber = Time.frameCount;
				var countInFrame = rest > perFrame ? perFrame : rest;
				for (var i = 0; i < countInFrame; i++)
				{
					var allocation = _allocator.CreateAllocationOperator();
					yield return allocation.Execute();
					_pool.Enqueue(allocation.Instance);
					operation.SetProgress((count - rest) / count);
					if (operation.IsCanceled)
					{
						operation.MarkAsCanceled();
						yield break;
					}
				}
				rest -= countInFrame;
				if (Time.frameCount == frameNumber)
				{
					yield return null;
				}
			}
			operation.MarkAsCompleted();
		}

		/// <summary>
		/// Borrow an instance.
		/// インスタンスを借ります。
		/// </summary>
		public T Rent()
		{
			if (!TryRent(out var instance))
			{
				instance = _allocator.Allocate();
				_allocator.Activate(instance);
			}
			return instance;
		}

		/// <summary>
		/// Try to borrow an instance.
		/// インスタンスを借りることを施行します。
		/// </summary>
		/// <returns>
		/// </returns>
		public bool TryRent(out T instance)
		{
			var exists = _pool.Count > 0;
			if (exists)
			{
				instance = _pool.Dequeue();
				_allocator.Activate(instance);
			}
			else
			{
				instance = default;
			}
			return exists;
		}

		/// <summary>
		/// Return the instance.
		/// インスタンスを返却します。
		/// </summary>
		public void Return(T instance)
		{
			_allocator.Deactivate(instance);
			_pool.Enqueue(instance);
		}

		private readonly IObjectAllocator<T> _allocator;
		private readonly Queue<T> _pool = new Queue<T>();
		private readonly List<Coroutine> _coroutines = new List<Coroutine>();
		private bool _disposed = false;
	}
}
