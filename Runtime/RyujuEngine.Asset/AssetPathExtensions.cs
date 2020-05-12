// Copyright (c) 2020 Ryuju Orchestra
using System.IO;

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
		/// Get the parent folder asset path from the specified asset path.
		/// 指定したアセットパスの親フォルダを取得します。
		/// </summary>
		/// <param name="path">
		/// An asset path.
		/// アセットパスです。
		/// </param>
		/// <returns>
		/// An asset path to a parent folder of the specified asset path
		/// or <see cref="string.Empty"/> if the parent folder cannot exist.
		/// 親フォルダのアセットパスです。
		/// 親フォルダが存在し得ない場合は<see cref="string.Empty"/>です。
		/// </returns>
		public static string Parent(this string path)
		{
			var index = path.TrimEnd(AssetFolderSeparatorChar).LastIndexOf(AssetFolderSeparatorChar);
			return index >= 0 ? path.Substring(0, index) : string.Empty;
		}

		/// <summary>
		/// Get the last component on the specified asset path.
		/// アセットパスで示されるパスのファイル名または最下層フォルダの名前を取得します。
		/// </summary>
		/// <param name="path">
		/// An asset path.
		/// アセットパスです。
		/// </param>
		/// <returns>
		/// A string not null.
		/// null でない文字列です。
		/// </returns>
		public static string LastPathComponent(this string path)
		{
			var normalized = path.TrimEnd(AssetFolderSeparatorChar);
			var index = normalized.LastIndexOf(AssetFolderSeparatorChar);
			return index >= 0 ? normalized.Substring(index + 1) : path;
		}

		/// <summary>
		/// Remove an extension from the specified asset path.
		/// アセットパスに含まれる拡張子を 1 つ取り除きます。
		/// </summary>
		/// <param name="path">
		/// An asset path.
		/// アセットパスです。
		/// </param>
		/// <returns>
		/// An asset path that was removed one extension, not null.
		/// null ではない文字列であり、拡張子が 1 つ取り除かれたアセットパスです。
		/// </returns>
		public static string RemoveExtension(this string path)
		{
			var lastComponentSeparatorIndex = path.LastIndexOf(AssetFolderSeparatorChar);
			var lastExtensionDotIndex = path.LastIndexOf('.');
			return lastExtensionDotIndex > lastComponentSeparatorIndex + 1
				? path.Substring(0, lastExtensionDotIndex)
				: path
				;
		}

		/// <summary>
		/// Remove all extensions from the specified asset path.
		/// アセットパスに含まれる拡張子をすべて取り除きます。
		/// </summary>
		/// <param name="path">
		/// An asset path.
		/// アセットパスです。
		/// </param>
		/// <returns>
		/// An asset path that was removed all extensions, not null.
		/// null ではない文字列であり、拡張子がすべて取り除かれたアセットパスです。
		/// </returns>
		public static string RemoveAllExtensions(this string path)
		{
			var lastComponentSeparatorIndex = path.LastIndexOf(AssetFolderSeparatorChar);
			var lastExtensionDotIndex = path.IndexOf('.', lastComponentSeparatorIndex + 1);
			return lastExtensionDotIndex >= 0
				? path.Substring(0, lastExtensionDotIndex)
				: path
				;
		}

		/// <summary>
		/// Split the asset path into a component.
		/// コンポーネント単位にアセットパスを分割します。
		/// </summary>
		/// <param name="path">
		/// An asset path.
		/// アセットパスです。
		/// </param>
		/// <returns>
		/// 指定されたアセットパスのコンポーネントの配列です。
		/// </returns>
		public static string[] SplitComponents(this string path)
			=> string.IsNullOrEmpty(path)
				? new string[0]
				: path.Trim(AssetFolderSeparatorChar).Split(AssetFolderSeparatorChar)
				;

		/// <summary>
		/// アセットパスをプロジェクトディレクトリからの OS ネイティブな相対パスに変換します。
		/// </summary>
		/// <param name="assetPath">変換したい Unity アセットパスです。</param>
		/// <returns>
		/// 現在の OS で使われるディレクトリ区切り文字に変換されたネイティブな相対パスです。
		/// </returns>
		public static string ToSystemPath(this string assetPath)
		{
			var separator = Path.DirectorySeparatorChar;
			return separator != '/'
				? assetPath.Replace('/', separator)
				: assetPath
				;
		}

		private const char AssetFolderSeparatorChar = '/';
	}
}
