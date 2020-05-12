// Copyright (c) 2020 Ryuju Orchestra
using System.Collections.Generic;

using UnityEditor;

namespace RyujuEngine.Build
{
	/// <summary>
	/// スクリプト定義済みシンボルの書き換えを行うためのクラスです。
	/// </summary>
	public static class ScriptDefinedSymbols
	{
		/// <summary>
		/// シンボル一覧を取得します。
		/// </summary>
		/// <param name="target">対象とする実行プラットフォームです。</param>
		public static IEnumerable<string> Get(BuildTarget target)
		{
			var targetGroup = BuildPipeline.GetBuildTargetGroup(target);
			var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
			return defines.Split(ScriptDefinitionSeparator);
		}

		/// <summary>
		/// シンボル一覧を設定します。
		/// </summary>
		/// <param name="target">対象とする実行プラットフォームです。</param>
		/// <param name="symbols">設定するシンボル一覧です。</param>
		public static void Set(BuildTarget target, IEnumerable<string> symbols)
		{
			var targetGroup = BuildPipeline.GetBuildTargetGroup(target);
			var defines = string.Join(ScriptDefinitionSeparator.ToString(), symbols);
			PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
		}

		private const char ScriptDefinitionSeparator = ';';
	}
}
