// Copyright (c) 2020 Ryuju Orchestra
using UnityEditor;
using UnityEngine;

namespace RyujuEngine.Asset
{
	/// <summary>
	/// A utilities for the asset paths on the Unity.
	/// Unity 上で扱われるアセットパスの操作を便利にするユーティリティクラスです。
	/// </summary>
	public static partial class AssetPathExtensions
	{
		/// <summary>
		/// Get an asset path of a script from <see cref="ScriptableObject"/> instance that class defined by it.
		/// <see cref="ScriptableObject"/>のインスタンスから、それが定義されているスクリプトのアセットパスを取得します。
		/// </summary>
		public static string GetAssetPath(this ScriptableObject obj)
		{
			var mono = MonoScript.FromScriptableObject(obj);
			return AssetDatabase.GetAssetPath(mono);
		}
	}
}
