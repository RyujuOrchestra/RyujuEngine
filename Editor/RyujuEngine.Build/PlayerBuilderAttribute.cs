using System;

namespace RyujuEngine.Build
{
	/// <summary>
	/// 指定したクラスがプレイヤービルドのフックであることを示す属性です。
	/// </summary>
	/// <remarks>
	/// 指定したクラスは<see cref="IPlayerBuilder"/>を継承しており、
	/// 引数を持たないコンストラクタを定義してください。
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class)]
	public class PlayerBuilderAttribute : Attribute
	{
	}
}
