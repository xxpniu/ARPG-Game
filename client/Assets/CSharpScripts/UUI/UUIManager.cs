using System;
using System.Collections.Generic;
using UGameTools;
using UnityEngine.UI;
using UnityEngine;


public abstract class UUIElement
{
	protected GameObject uiRoot;
	protected abstract void OnDestory ();
	protected abstract void OnCreate ();

	public static void Destory(UUIElement el){
		el.OnDestory ();
	}
}

public class UUIManager:XSingleton<UUIManager>
{
	public void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
		DontDestroyOnLoad (top);
	}

	private Dictionary<string,UUIWindow> _window=new Dictionary<string, UUIWindow> ();
	private Dictionary<int,UUIElement> _tips= new Dictionary<int, UUIElement> ();

	void Update()
	{

		while (_addTemp.Count > 0) {
			var t = _addTemp.Dequeue ();
			_window.Add (t.GetType ().Name, t);
		}

		foreach (var i in _window) {
			UUIWindow.UpdateUI( i.Value);
			if (i.Value.CanDestory) {
				_delTemp.Enqueue (i.Value);
			}
		}

		while (_delTemp.Count > 0) {
			var t = _delTemp.Dequeue ();
			if (_window.Remove (t.GetType ().Name))
				UUIElement.Destory (t);
		}

		foreach (var i in _tips) 
		{
			
		}
	}

	public T GetUIWindow<T>()where T:UUIWindow, new()
	{
		UUIWindow obj;
		if (_window.TryGetValue (typeof(T).Name, out obj)) {
			return obj as T;
		}
		return default(T);
	}

	private Queue<UUIWindow> _addTemp = new Queue<UUIWindow> ();
	private Queue<UUIWindow> _delTemp = new Queue<UUIWindow> ();

	public T CreateWindow<T>() where T:UUIWindow, new()
	{
		var ui = GetUIWindow<T> ();
		if (ui == null) {
			ui = UUIWindow.Create<T> (this.gameObject.transform);
			_addTemp.Enqueue (ui);
		}
		return ui;
	}

	public void ShowMask(bool show)
	{
		BackImage.ActiveSelfObject (show);
	}

	public void ShowLoading(float p)
	{
		BackImage.transform.FindChild<Scrollbar> ("Scrollbar").size =  p;
	}

	public Image BackImage;
	public GameObject top;

}