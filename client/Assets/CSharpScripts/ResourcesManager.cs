using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcesManager : XSingleton<ResourcesManager> {

	public class LoadProcesser
	{
		public CallBackDele CallBack;
		public ResourceRequest Request;
	}

	public delegate void CallBackDele(Object res);
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (loaders.Count > 0) {
			foreach (var i in loaders) {
				if (i.Request.isDone) {
					_dones.Enqueue (i);	
				}
			}

			while (_dones.Count > 0) 
			{
				var d = _dones.Dequeue ();
				d.CallBack (d.Request.asset);
				loaders.Remove (d);
			}

		}
	}

	private Queue<LoadProcesser> _dones = new Queue<LoadProcesser> ();
			
	private HashSet<LoadProcesser> loaders = new HashSet<LoadProcesser> ();
		
	public T LoadResources<T>(string path) where T:Object
	{
		return Resources.Load<T> (path);
	}

	public T[] LoadAll<T>(string path) where T:Object
	{
		return Resources.LoadAll<T> (path);
	}

	public ResourceRequest LoadAsync<T>(string path) where T:Object
	{
		return Resources.LoadAsync<T> (path);
	}

	public void LoadAsyncCallBack<T>(string path, CallBackDele callBack) where T:Object
	{
		var request = LoadAsync<T> (path);
		var processer = new LoadProcesser{ CallBack = callBack, Request = request };
		loaders.Add (processer);
	}
}
