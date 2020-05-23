// Copyright (c) 2020 Ryuju Orchestra
using UnityEngine;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// An allocator class that operate the <see cref="GameObject"/> instances.
	/// <see cref="GameObject"/> インスタンスを操作するアロケータークラスです。
	/// </summary>
	public sealed class GameObjectAllocator : IObjectAllocator<GameObject>
	{
		/// <summary>
		/// Create an allocator that clones the specified game object.
		/// 指定した GameObject を複製するアロケータを作成します。
		/// </summary>
		public GameObjectAllocator(GameObject obj)
			=> _obj = obj;

		/// <inheritdoc/>
		public void Dispose()
		{
		}

		/// <Inheritdoc/>
		public GameObject Allocate()
		{
			var instance = Object.Instantiate(_obj);
			instance.SetActive(false);
			return instance;
		}

		/// <Inheritdoc/>
		public IAllocationOperation<GameObject> CreateAllocationOperator()
			=> new DeferredAllocationOperation<GameObject>(Allocate);

		/// <Inheritdoc/>
		public void Activate(GameObject instance) => instance.SetActive(true);

		/// <Inheritdoc/>
		public void Deactivate(GameObject instance) => instance.SetActive(false);

		/// <Inheritdoc/>
		public void Release(GameObject instance) => Object.Destroy(instance);

		private readonly GameObject _obj;
	}
}
