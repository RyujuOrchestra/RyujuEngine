// Copyright (c) 2020 Ryuju Orchestra
using System;

using UnityEditor;

using Object = UnityEngine.Object;

namespace RyujuEngine.Build
{
	/// <summary>
	/// ビルド処理の途中で自動で変更される値を消去するためのクラスです。
	/// </summary>
	public static class BuildPreferencesCleaner
	{
		/// <summary>
		/// 起動時に自動的に読み込まれるアセット一覧をクリアします。
		/// </summary>
		public static void ClearPreloadedAssets() => PlayerSettings.SetPreloadedAssets(Array.Empty<Object>());

		/// <summary>
		/// 常に含まれるシェーダープログラム一覧をクリアします。
		/// </summary>
		public static void ClearAlwaysIncludedShaders()
		{
			var graphicsSettings = new SerializedObject(
				AssetDatabase.LoadAssetAtPath<Object>("Projectsettings/GraphicsSettings.asset")
			);
			graphicsSettings.FindProperty("m_AlwaysIncludedShaders").ClearArray();
			_ = graphicsSettings.ApplyModifiedProperties();
			AssetDatabase.SaveAssets();
		}
	}
}
