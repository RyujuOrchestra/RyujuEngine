// Copyright (c) 2020 Ryuju Orchestra
using System;

namespace RyujuEngine.Preferences
{
	/// <summary>
	/// <see cref="PreferencesObject"/>の配置位置を設定する属性です。
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class PreferencesFileLocationAttribute : Attribute
	{
		/// <summary>
		/// インスタンスを作成します。
		/// </summary>
		/// <param name="scope">
		/// <see cref="Scope"/>を参照してください。
		/// </param>
		/// <param name="fileName">
		/// <see cref="FileName"/>を参照してください。
		/// </param>
		public PreferencesFileLocationAttribute(PreferencesScope scope, string fileName)
		{
			Scope = scope;
			FileName = fileName;
		}

		/// <summary>
		/// この属性で指定されたクラスの Preferences クラスのスコープです。
		/// </summary>
		public PreferencesScope Scope { get; }

		/// <summary>
		/// ファイル名です。
		/// </summary>
		/// <value></value>
		public string FileName { get; }

		/// <summary>
		/// アセットパスを取得します
		/// </summary>
		internal string GetFilePath()
		{
			if (FileName.Contains("..") || FileName.Contains("/"))
			{
				throw new ArgumentException("FileName must not contain '..' and '/'.");
			}

			switch (Scope)
			{
			case PreferencesScope.User:
				return $"Library/RyujuEngine.Preferences/{FileName}";
			case PreferencesScope.Project:
				return $"ProjectSettings/RyujuEngine.Preferences/{FileName}";
			}

			throw new NotImplementedException("Unknown scope.");
		}
	}
}
