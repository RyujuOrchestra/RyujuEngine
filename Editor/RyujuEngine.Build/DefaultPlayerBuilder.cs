// Copyright (c) 2020 Ryuju Orchestra
using System;

using UnityEditor;

namespace RyujuEngine.Build
{
	/// <summary>
	/// デフォルトの動作を提供するビルダークラスです。
	/// </summary>
	public sealed class DefaultPlayerBuilder : IPlayerBuilder
	{
		/// <inheritdoc/>
		public void BuildPlayer(BuildPlayerOptions options)
		{
			BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
			BuildPreferencesCleaner.ClearPreloadedAssets();
			BuildPreferencesCleaner.ClearAlwaysIncludedShaders();
			ScriptDefinedSymbols.Set(options.target, Array.Empty<string>());
		}
	}
}
