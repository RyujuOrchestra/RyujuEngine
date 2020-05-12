// Copyright (c) 2020 Ryuju Orchestra
namespace RyujuEngine.Preferences
{
	/// <summary>
	/// <see cref="ScriptablePreferencesObject"/>のインスタンスのスコープを表す列挙型です。
	/// </summary>
	public enum PreferencesScope
	{
		/// <summary>
		/// `{ProjectDir}/Libraries` ディレクトリに保存されることを示すスコープ列挙子です。
		/// </summary>
		User,

		/// <summary>
		/// `{ProjectDir}/ProjectSettings` ディレクトリに保存されることを示すスコープ列挙子です。
		/// </summary>
		Project,
	}
}
