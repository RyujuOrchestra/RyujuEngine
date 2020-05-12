// Copyright (c) 2020 Ryuju Orchestra
using UnityEditor;

namespace RyujuEngine.Build
{
	/// <summary>
	/// ビルドの動作を提供できるインターフェイスです。
	/// </summary>
	public interface IPlayerBuilder
	{
		/// <summary>
		/// プレイヤーをビルドします。
		/// </summary>
		/// <param name="options">ビルドオプションです。</param>
		void BuildPlayer(BuildPlayerOptions options);
	}
}
