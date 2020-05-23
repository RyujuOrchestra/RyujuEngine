// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using System.Collections;

using UnityEngine;
using UnityEngine.TestTools;

using RyujuEngine.Collections;

namespace Collections
{
	public sealed class ObjectPoolTest
	{
		private const string Content = "dummy";

		public static int[] TestCounts = {
			0,
			1,
			2,
			3,
			5,
			42,
		};

		public void It_should_be_able_to_dispose_empty()
		{
			var pool = new ObjectPool<string>(new DummyAllocator());
			pool.Dispose();
		}

		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(42)]
		public void It_should_be_able_to_prepare(int count)
		{
			var allocator = new DummyAllocator();
			using (var pool = new ObjectPool<string>(allocator))
			{
				Assert.That(pool.Count, Is.EqualTo(0), "The pool must be empty.");

				pool.Prepare(count);
				Assert.That(pool.Count, Is.EqualTo(count), "The pool must prepare.");
				Assert.That(allocator.AllocatedCount, Is.EqualTo(count), "The allocator must allocate.");
				Assert.That(allocator.ActivatedCount, Is.EqualTo(0), "The allocator must not activate.");
			}
			Assert.That(allocator.DeactivatedCount, Is.EqualTo(count), "The allocator must deactivated.");
			Assert.That(allocator.ReleasedCount, Is.EqualTo(count), "The allocator must release.");
		}

		[UnityTest]
		public IEnumerator It_should_be_able_to_prepare_async(
			[ValueSource(nameof(TestCounts))] int count,
			[ValueSource(nameof(TestCounts))] int perFrame
		)
		{
			var completedCount = 0;
			var canceledCount = 0;

			var allocator = new DummyAllocator();
			using (var pool = new ObjectPool<string>(allocator))
			{
				Assert.That(pool.Count, Is.EqualTo(0), "The pool must prepare.");

				var operation = pool.PrepareAsync(count, perFrame);
				operation.CompletedEvent += () => completedCount++;
				operation.CanceledEvent += () => canceledCount++;
				while (!operation.IsDone)
				{
					yield return null;
				}

				Assert.That(pool.Count, Is.EqualTo(count), "The pool must prepare.");
				Assert.That(operation.IsDone, Is.True, "The done flag must be true.");
				Assert.That(operation.IsCompleted, Is.True, "The completed flag must be true.");
				Assert.That(operation.IsCanceled, Is.False, "The canceled flag must be false.");
				Assert.That(operation.Progress, Is.EqualTo(1.0f), "The progress rate must be 1.0f.");
				Assert.That(completedCount, Is.EqualTo(1), "The completed event must be dispatched only once.");
				Assert.That(canceledCount, Is.EqualTo(0), "The canceled event must not be dispatched.");
			}
			Assert.That(allocator.DeactivatedCount, Is.EqualTo(count), "The allocator must deactivated.");
			Assert.That(allocator.ReleasedCount, Is.EqualTo(count), "The allocator must release.");
		}

		[UnityTest]
		public IEnumerator It_should_be_able_to_wait_with_coroutine(
			[ValueSource(nameof(TestCounts))] int count,
			[ValueSource(nameof(TestCounts))] int perFrame
		)
		{
			var completedCount = 0;
			var canceledCount = 0;

			var allocator = new DummyAllocator();
			using (var pool = new ObjectPool<string>(allocator))
			{
				var operation = pool.PrepareAsync(count, perFrame);
				operation.CompletedEvent += () => completedCount++;
				operation.CanceledEvent += () => canceledCount++;
				yield return operation.WaitAsync();

				Assert.That(pool.Count, Is.EqualTo(count), "The pool must prepare the specified count exactly.");
				Assert.That(operation.IsDone, Is.True, "The done flag must be true.");
				Assert.That(operation.IsCompleted, Is.True, "The completed flag must be true.");
				Assert.That(operation.IsCanceled, Is.False, "The canceled flag must be false.");
				Assert.That(operation.Progress, Is.EqualTo(1.0f), "The progress rate must be 1.0f.");
				Assert.That(completedCount, Is.EqualTo(1), "The completed event must be dispatched only once.");
				Assert.That(canceledCount, Is.EqualTo(0), "The canceled event must not be dispatched.");
			}
			Assert.That(allocator.DeactivatedCount, Is.EqualTo(count), "The allocator must deactivated.");
			Assert.That(allocator.ReleasedCount, Is.EqualTo(count), "The allocator must release.");
		}

		[UnityTest]
		public IEnumerator It_should_be_able_to_cancel()
		{
			const int count = 10;
			const int perFrame = 3;
			var canceledCount = 0;

			var allocator = new DummyAllocator();
			using (var pool = new ObjectPool<string>(allocator))
			{
				var operation = pool.PrepareAsync(count, perFrame);
				operation.CanceledEvent += () => canceledCount++;
				IEnumerator CancelCoroutine()
				{
					operation.Cancel();
					yield break;
				}
				SingletonGameObject.StartCoroutine(CancelCoroutine());
				yield return operation.WaitAsync();

				Assert.That(operation.IsDone, Is.True, "The done flag must be true.");
				Assert.That(operation.IsCanceled, Is.True, "The canceled flag must be true.");
				Assert.That(canceledCount, Is.EqualTo(1), "The canceled event must be dispatched only once.");
			}
		}

		[Test]
		public void It_should_be_able_to_lend_on_empty()
		{
			var allocator = new DummyAllocator();
			using (var pool = new ObjectPool<string>(allocator))
			{
				var obj = pool.Rent();
				Assert.That(obj, Is.EqualTo(Content), "Invalid object.");
				Assert.That(allocator.ActivatedCount, Is.EqualTo(1), "The allocator must allocate only once.");
				Assert.That(allocator.DeactivatedCount, Is.EqualTo(0), "The allocator must not deactivate.");

				pool.Return(obj);
				Assert.That(allocator.ActivatedCount, Is.EqualTo(1), "The allocator must not allocate.");
				Assert.That(allocator.DeactivatedCount, Is.EqualTo(1), "The allocator must deactivate only once.");
			}
			Assert.That(allocator.ReleasedCount, Is.EqualTo(1), "The allocator must release only once.");
		}

		[Test]
		public void It_should_be_able_to_lend_after_preparing()
		{
			var allocator = new DummyAllocator();
			using (var pool = new ObjectPool<string>(allocator))
			{
				pool.Prepare(1);
				Assert.That(allocator.AllocatedCount, Is.EqualTo(1), "The allocator must allocate.");
				Assert.That(allocator.ActivatedCount, Is.EqualTo(0), "The allocator must not activate.");


				var obj = pool.Rent();
				Assert.That(obj, Is.EqualTo(Content), "Invalid object.");
				Assert.That(allocator.AllocatedCount, Is.EqualTo(1), "The allocator must not allocate.");
				Assert.That(allocator.ActivatedCount, Is.EqualTo(1), "The allocator must activate only once.");
				Assert.That(allocator.DeactivatedCount, Is.EqualTo(0), "The allocator must not deactivate.");

				pool.Return(obj);
				Assert.That(allocator.ActivatedCount, Is.EqualTo(1), "The allocator must not allocate.");
				Assert.That(allocator.DeactivatedCount, Is.EqualTo(1), "The allocator must deactivate only once.");
			}
			Assert.That(allocator.ReleasedCount, Is.EqualTo(1), "The allocator must release only once.");
		}

		[Test]
		public void It_should_be_able_to_try_lend_on_empty()
		{
			var allocator = new DummyAllocator();
			using (var pool = new ObjectPool<string>(allocator))
			{
				var succeeded = pool.TryRent(out var obj);
				Assert.That(succeeded, Is.False, "The pool lend no object on empty.");
				Assert.That(allocator.AllocatedCount, Is.EqualTo(0), "The allocator must not allocate.");
				Assert.That(allocator.ActivatedCount, Is.EqualTo(0), "The allocator must not activate.");
				Assert.That(allocator.DeactivatedCount, Is.EqualTo(0), "The allocator must not deactivate.");
			}
			Assert.That(allocator.ReleasedCount, Is.EqualTo(0), "The allocator must not release.");
		}

		[Test]
		public void It_should_be_able_to_try_lend_after_prepared()
		{
			var allocator = new DummyAllocator();
			using (var pool = new ObjectPool<string>(allocator))
			{
				pool.Prepare(1);
				var succeeded = pool.TryRent(out var obj);
				Assert.That(succeeded, Is.True, "The pool lend an object.");
				Assert.That(obj, Is.EqualTo(Content), "Invalid object.");
				Assert.That(allocator.AllocatedCount, Is.EqualTo(1), "The allocator must not allocate.");
				Assert.That(allocator.ActivatedCount, Is.EqualTo(1), "The allocator must activate only once.");
				Assert.That(allocator.DeactivatedCount, Is.EqualTo(0), "The allocator must not deactivate.");

				pool.Return(obj);
				Assert.That(allocator.ActivatedCount, Is.EqualTo(1), "The allocator must not allocate.");
				Assert.That(allocator.DeactivatedCount, Is.EqualTo(1), "The allocator must deactivate only once.");
			}
			Assert.That(allocator.ReleasedCount, Is.EqualTo(1), "The allocator must release only once.");
		}

		private class DummyAllocator : IObjectAllocator<string>
		{
			public int AllocatedCount
			{
				get;
				private set;
			} = 0;

			public int ActivatedCount
			{
				get;
				private set;
			} = 0;

			public int DeactivatedCount
			{
				get;
				private set;
			} = 0;

			public int ReleasedCount
			{
				get;
				private set;
			} = 0;

			public string Allocate()
			{
				AllocatedCount++;
				return Content;
			}

			public IAllocationOperation<string> CreateAllocationOperator()
				=> new DeferredAllocationOperation<string>(Allocate);
			public void Activate(string instance) => ActivatedCount++;
			public void Deactivate(string instance) => DeactivatedCount++;
			public void Release(string instance) => ReleasedCount++;
			public void Dispose() { }
		}
	}
}
