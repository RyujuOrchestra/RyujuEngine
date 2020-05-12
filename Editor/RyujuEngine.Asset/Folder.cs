// Copyright (c) 2020 Ryuju Orchestra
using System.Linq;

using UnityEditor;

namespace RyujuEngine.Asset
{
	/// <summary>
	/// アセットフォルダに関する処理を行うクラスです。
	/// </summary>
	public static class Folder
	{
		/// <summary>
		/// 中間フォルダを含めて、指定したアセットパスにフォルダを作成します。
		/// </summary>
		/// <param name="path">
		/// 作成したいフォルダへのパスです。
		/// </param>
		public static bool Make(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			if (AssetDatabase.IsValidFolder(path))
			{
				return true;
			}
			var components = path.SplitComponents();
			var parentFolder = components[0];
			foreach (var folderName in components.Skip(1))
			{
				var folderPath = $"{parentFolder}/{folderName}";
				if (AssetDatabase.IsValidFolder(folderPath))
				{
					continue;
				}
				if (string.IsNullOrEmpty(AssetDatabase.CreateFolder(parentFolder, folderName)))
				{
					return false;
				}
				parentFolder = folderPath;
			}
			return true;
		}
	}
}
