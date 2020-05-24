// Copyright (c) 2020 Ryuju Orchestra
namespace RyujuEngine.Collections
{
	/// <summary>
	/// A part of the ObjectPool interface to enable to rent.
	/// Rent できるための ObjectPool インターフェイスの一部です。
	/// </summary>
	public interface IObjectRentable<out T>
	{
		/// <summary>
		/// Borrow an instance.
		/// インスタンスを借ります。
		/// </summary>
		T Rent();
	}
}
