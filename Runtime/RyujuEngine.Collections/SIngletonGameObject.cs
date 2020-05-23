// Copyright (c) 2020 Ryuju Orchestra
using System.Collections;

using UnityEngine;

namespace RyujuEngine.Collections
{
	/// <summary>
	/// A class that hold a singleton <see cref="GameObject"/>.
	/// シングルトンな<see cref="GameObject"/>を保持するクラスです。
	/// </summary>
	public static class SingletonGameObject
	{
		/// <summary>
		/// A singleton GameObject.
		/// シングルトンな GameObject です。
		/// </summary>
		/// <value></value>
		public static GameObject Instance
		{
			get
			{
				if (!_instance)
				{
					Initialize();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Start the specified instruction as coroutine.
		/// 指定したインストラクションをコルーチンとして開始します。
		/// </summary>
		public static Coroutine StartCoroutine(IEnumerator instruction)
		{
			Initialize();
			return _coroutineExecutor.Execute(instruction);
		}

		/// <summary>
		/// Stop the specified coroutine.
		/// 指定したイコルーチンを停止させます。
		/// </summary>
		public static void StopCoroutine(Coroutine coroutine)
		{
			Initialize();
			_coroutineExecutor.Stop(coroutine);
		}

		/// <summary>
		/// Initialize.
		/// 初期化します。
		/// </summary>
		public static void Initialize()
		{
			if (_instance)
			{
				return;
			}

#if UNITY_EDITOR
			const string Name = "[RyujuEngine.Collection.SingletonGameObject]";
			_instance = GameObject.Find(Name);
			if (_instance)
			{
				return;
			}
			_instance = new GameObject(Name, typeof(CoroutineExecutor));
#else
			_instance = new GameObject(string.Empty, typeof(CoroutineExecutor));
#endif
			_coroutineExecutor = _instance.GetComponent<CoroutineExecutor>();
			Object.DontDestroyOnLoad(_instance);
		}

		private sealed class CoroutineExecutor : MonoBehaviour
		{
			public Coroutine Execute(IEnumerator instruction) => StartCoroutine(instruction);
			public void Stop(Coroutine coroutine) => StopCoroutine(coroutine);
		}

		private static GameObject _instance = default;
		private static CoroutineExecutor _coroutineExecutor = default;
	}
}
