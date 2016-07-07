using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class XSingleton<T> : MonoBehaviour  where T :MonoBehaviour , new()
{
	protected static T _instance;
	public static T Singleton
	{
		get
		{
			if (!_instance)
				_instance = GameObject.FindObjectOfType(typeof(T)) as T;

			if (!_instance)
				_instance = new GameObject(typeof(T).ToString()).AddComponent<T>();

			return _instance;
		}
	}
}

