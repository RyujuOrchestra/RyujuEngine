// Copyright (c) 2020 Ryuju Orchestra
using NUnit.Framework;

using System.IO;

using RyujuEngine.Asset;

namespace Asset
{
	public sealed class AssetPathExtensionsTest
	{
		[Test]
		public void It_should_be_able_to_get_a_parent_folder_from_an_asset_path()
		{
			Assert.That("Assets/folder/child".Parent(), Is.EqualTo("Assets/folder"));
			Assert.That("/Assets/folder///".Parent(), Is.EqualTo("/Assets"));
			Assert.That("/Assets".Parent(), Is.EqualTo(string.Empty));
			Assert.That("Assets".Parent(), Is.EqualTo(string.Empty));
			Assert.That(string.Empty.Parent(), Is.EqualTo(string.Empty));
		}

		[Test]
		public void It_should_be_able_to_get_the_last_component_from_an_asset_path()
		{
			Assert.That("Assets/folder/child".LastPathComponent(), Is.EqualTo("child"));
			Assert.That("/Assets/folder///".LastPathComponent(), Is.EqualTo("folder"));
			Assert.That("/Assets".LastPathComponent(), Is.EqualTo("Assets"));
			Assert.That("Assets".LastPathComponent(), Is.EqualTo("Assets"));
			Assert.That(string.Empty.LastPathComponent(), Is.EqualTo(string.Empty));
		}

		[Test]
		public void It_should_be_able_to_remove_an_extension_from_an_asset_path()
		{
			Assert.That("Assets/folder/child.ext".RemoveExtension(), Is.EqualTo("Assets/folder/child"));
			Assert.That("/Assets/folder/child.ext".RemoveExtension(), Is.EqualTo("/Assets/folder/child"));
			Assert.That("Assets/folder.ext/child".RemoveExtension(), Is.EqualTo("Assets/folder.ext/child"));
			Assert.That("Assets/folder/child.ext1.ext2".RemoveExtension(), Is.EqualTo("Assets/folder/child.ext1"));
			Assert.That("Assets/folder/child".RemoveExtension(), Is.EqualTo("Assets/folder/child"));
			Assert.That("Assets.ext".RemoveExtension(), Is.EqualTo("Assets"));
			Assert.That("Assets".RemoveExtension(), Is.EqualTo("Assets"));
			Assert.That(string.Empty.RemoveExtension(), Is.EqualTo(string.Empty));
		}

		[Test]
		public void It_should_be_able_to_remove_all_extension_from_an_asset_path()
		{
			Assert.That("Assets/folder/child.ext".RemoveAllExtensions(), Is.EqualTo("Assets/folder/child"));
			Assert.That("/Assets/folder/child.ext".RemoveAllExtensions(), Is.EqualTo("/Assets/folder/child"));
			Assert.That("Assets/folder.ext/child".RemoveAllExtensions(), Is.EqualTo("Assets/folder.ext/child"));
			Assert.That("Assets/folder/child.ext1.ext2".RemoveAllExtensions(), Is.EqualTo("Assets/folder/child"));
			Assert.That("Assets/folder/child".RemoveAllExtensions(), Is.EqualTo("Assets/folder/child"));
			Assert.That("Assets.ext".RemoveAllExtensions(), Is.EqualTo("Assets"));
			Assert.That("Assets".RemoveAllExtensions(), Is.EqualTo("Assets"));
			Assert.That(string.Empty.RemoveAllExtensions(), Is.EqualTo(string.Empty));
		}

		[Test]
		public void It_should_be_able_to_split_an_asset_path_into_a_component()
		{
			Assert.That(
				"Assets/folder/child.ext".SplitComponents(),
				Is.EqualTo(new string[] { "Assets", "folder", "child.ext" })
			);
			Assert.That(
				"/Assets/folder.ext1/child.ext2.ext3".SplitComponents(),
				Is.EqualTo(new string[] { "Assets", "folder.ext1", "child.ext2.ext3" })
			);
			Assert.That(
				"/Assets".SplitComponents(),
				Is.EqualTo(new string[] { "Assets" })
			);
			Assert.That(
				"Assets".SplitComponents(),
				Is.EqualTo(new string[] { "Assets" })
			);
			Assert.That(string.Empty.SplitComponents(), Is.EqualTo(new string[0]));
		}

		[Test]
		public void It_should_be_able_to_convert_an_asset_path_to_system_path()
		{
			Assert.That(
				"Assets/folder/child.ext".ToSystemPath(),
				Is.EqualTo(Path.Combine("Assets", "folder", "child.ext"))
			);
			Assert.That(
				"Assets".ToSystemPath(),
				Is.EqualTo("Assets")
			);
			Assert.That(string.Empty.ToSystemPath(), Is.EqualTo(string.Empty));
		}
	}
}
