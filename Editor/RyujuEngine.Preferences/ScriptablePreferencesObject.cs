// Copyright (c) 2020 Ryuju Orchestra
using System;
using System.IO;
using System.Linq;

using UnityEditorInternal;
using UnityEngine;

using RyujuEngine.Asset;

using Object = UnityEngine.Object;

namespace RyujuEngine.Preferences
{
	/// <summary>
	/// Preferences 用に使用できるシングルトンな<see cref="ScriptableObject"/>クラスです。
	/// </summary>
	/// <typeparam name="T">
	/// 自分自身の型です。
	/// </typeparam>
	/// <remarks>
	/// このクラスを継承した場合、sealed クラスにし、
	/// <see cref="PreferencesFileLocationAttribute"/>を付ける必要があります。
	/// </remarks>
	public abstract class ScriptablePreferencesObject<T> : ScriptableObject
	where T : ScriptablePreferencesObject<T>
	{
		/// <summary>
		/// シングルトンインスタンスです。
		/// </summary>
		public static T Instance => LoadOrDefault();
		private static T _instance;

		/// <summary>
		/// アセットに保存します。
		/// </summary>
		public virtual void Save()
		{
			var assetPath = GetFilePath();
			if (!Folder.Make(assetPath.Parent()))
			{
				throw new IOException("Failed to create");
			}
			InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { Instance }, assetPath, true);
		}

		/// <summary>
		/// <see cref="PreferencesFileLocationAttribute"/>で指定されたパスから読み込むか
		/// 新しくインスタンスを生成します。
		/// </summary>
		private static T LoadOrDefault()
		{
			var assetPath = GetFilePath();
			_ = InternalEditorUtility.LoadSerializedFileAndForget(assetPath);
			if (_instance == null)
			{
				var instance = CreateInstance<T>();
				instance.hideFlags = HideFlags.HideAndDontSave;
			}
			return _instance;
		}

		/// <summary>
		/// アセットパスを取得します。
		/// </summary>
		private static string GetFilePath()
		{
			if (!typeof(T).IsDefined(typeof(PreferencesFileLocationAttribute), false))
			{
				throw new NotImplementedException("This class must be attached a PreferencesFileLocation attribute.");
			}
			var attribute = typeof(T).GetCustomAttributes(false)
				.Where(attr => attr is PreferencesFileLocationAttribute)
				.Single()
				;
			return ((PreferencesFileLocationAttribute)attribute).GetFilePath();
		}

		/// <summary>
		/// インスタンスが生成されたときに呼び出されます。
		/// </summary>
		protected ScriptablePreferencesObject() => _instance = this as T;
	}
}
