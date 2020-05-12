// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.Linq;

using UnityEditor;

namespace RyujuEngine.Build
{
	/// <summary>
	/// `Build And Run` メニューの挙動を変更するためのフッククラスです。
	/// </summary>
	public static class BuildPlayerHook
	{
		/// <summary>
		/// 新しいプレイヤービルドの処理を提供するビルダーです。
		/// </summary>
		public static IPlayerBuilder Builder => _builder ?? (_builder = CreateBuilder());

		/// <summary>
		/// <see cref="Builder"/>のキャッシュです。
		/// </summary>
		private static IPlayerBuilder _builder = null;

		/// <summary>
		/// <see cref="IPlayerBuilder"/>インスタンスを見つけます。
		/// </summary>
		private static IPlayerBuilder CreateBuilder()
		{
			var builderType = AppDomain.CurrentDomain.GetAssemblies()
				.Select(assembly => assembly.GetTypes())
				.SelectMany(types => types)
				.SingleOrDefault(type => type.IsDefined(typeof(PlayerBuilderAttribute), false))
				?? typeof(DefaultPlayerBuilder);
			return (IPlayerBuilder)Activator.CreateInstance(builderType);
		}

		/// <summary>
		/// すべてのアセンブのの読み込みに成功した時に Unity Editor から呼び出されます。
		/// </summary>
		[InitializeOnLoadMethod]
		internal static void Initialize()
			=> BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayer);

		/// <summary>
		/// ビルド処理が実行された時に Unity Editor から呼び出されます。
		/// </summary>
		/// <param name="options">
		/// プレイヤービルドのための設定です。
		/// </param>
		private static void BuildPlayer(BuildPlayerOptions options)
			=> Builder.BuildPlayer(options);
	}
}
