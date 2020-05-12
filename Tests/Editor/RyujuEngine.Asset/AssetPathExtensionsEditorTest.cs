// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using UnityEngine;

using RyujuEngine.Asset;

namespace Asset
{
	public sealed class AssetPathExtensionsEditorTest
	{
		[Test]
		public void It_should_be_able_to_get_a_script_asset_path_from_a_scriptable_object_instance()
		{
			Assert.That(
				ScriptableObject.CreateInstance<DummyScriptableObject>().GetAssetPath(),
				Does.EndWith("DummyScriptableObject.cs")
			);
		}
	}
}
