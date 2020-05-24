// Copyright (c) 2020 Ryuju Orchestra

namespace RyujuEngine.Collections
{
	/// <summary>
	/// A part of the ObjectPool interface to enable to return.
	/// Return できるための ObjectPool インターフェイスの一部です。
	/// </summary>
	public interface IObjectReturnable<in T>
	{
		/// <summary>
		/// Return the instance.
		/// インスタンスを返却します。
		/// </summary>
		void Return(T instance);
	}
}
