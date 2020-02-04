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
			if (!_instance) _instance = FindObjectOfType(typeof(T)) as T;
			if (!_instance) _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
			return _instance;
		}
	}

    /// <summary>
    /// See as Singleton
    /// </summary>
    /// <value>The s.</value>
    public static T S{ get { return Singleton; } }

    private void Awake()
    {
        _instance = this as T;
		DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// get don't auto create
    /// </summary>
    /// <returns></returns>
	public static T G()
    {
		if (_instance) return _instance;
		return null;
    }
}

