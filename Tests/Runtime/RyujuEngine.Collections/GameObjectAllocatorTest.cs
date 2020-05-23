// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using System.Collections;

using UnityEngine;
using UnityEngine.TestTools;

using RyujuEngine.Collections;

namespace Collections
{
	public sealed class GameObjectAllocatorTest
	{
		[OneTimeSetUp]
		public void Initialize()
		{
			_baseObject = new GameObject(NamePrefix);
		}

		[OneTimeTearDown]
		public void Destruct()
		{
			Object.Destroy(_baseObject);
		}

		private const string NamePrefix = "__test__";
		private GameObject _baseObject = default;

		[Test]
		public void It_should_be_able_to_allocate()
		{
			var allocator = new GameObjectAllocator(_baseObject);
			var clone = allocator.Allocate();
			Assert.That(clone.name, Does.StartWith(NamePrefix), "The names must have the same prefix.");
			Assert.That(clone.activeSelf, Is.False, "The clone must be deactivated.");
		}

		[UnityTest]
		public IEnumerator It_should_be_able_to_allocate_async()
		{
			var allocator = new GameObjectAllocator(_baseObject);
			var operation = allocator.CreateAllocationOperator();
			Assert.That(operation.IsDone, Is.False, "The operation must wait for yielding.");
			yield return operation.Execute();
			Assert.That(operation.IsDone, Is.True, "The operation must complete yielding.");
			Assert.That(operation.Instance, Is.Not.Null, "The instance property must not be null after done.");
			Assert.That(operation.Instance.name, Does.StartWith(NamePrefix), "The instance must have the same prefix.");
			Assert.That(operation.Instance.activeSelf, Is.False, "The clone must be deactivated.");
		}

		[Test]
		public void It_should_be_able_to_activate_an_instance()
		{
			var allocator = new GameObjectAllocator(_baseObject);
			var clone = allocator.Allocate();
			allocator.Activate(clone);
			Assert.That(clone.activeSelf, Is.True, "The clone must be activated.");
		}

		[Test]
		public void It_should_be_able_to_deactivate_an_instance()
		{
			var allocator = new GameObjectAllocator(_baseObject);
			var clone = allocator.Allocate();
			allocator.Activate(clone);
			allocator.Deactivate(clone);
			Assert.That(clone.activeSelf, Is.False, "The clone must be deactivated.");
		}

		[UnityTest]
		public IEnumerator It_should_be_able_to_release_an_instance()
		{
			var allocator = new GameObjectAllocator(_baseObject);
			var clone = allocator.Allocate();
			allocator.Release(clone);
			yield return null;
			Assert.That((bool)clone, Is.False, "The clone must be released.");
		}
	}
}
