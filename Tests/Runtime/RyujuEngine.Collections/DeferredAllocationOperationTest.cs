// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using System.Collections;

using UnityEngine;
using UnityEngine.TestTools;

using RyujuEngine.Collections;
using RyujuEngine.Units;

namespace Collections
{
	public sealed class DeferredAllocationOperationTest
	{
		[UnityTest]
		public IEnumerator It_should_be_able_to_allocate()
		{
			const string Content = "TestText";
			var operation = new DeferredAllocationOperation<string>(() => Content);
			Assert.That(operation.IsDone, Is.False, "The `IsDone` property must be false before the execution.");
			yield return operation.Execute();
			Assert.That(operation.IsDone, Is.True, "The `IsDone` property must be true after the execution.");
			Assert.That(operation.Instance, Is.EqualTo(Content), "Invalid instance.");
		}
	}
}
