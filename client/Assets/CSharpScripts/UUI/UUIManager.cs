using System;
using System.Collections.Generic;



public class UUIElement
{
	
}

public class UUIManager:XSingleton<UUIManager>
{
	public void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
		_window = new Dictionary<string, UUIWindow> ();
		_tips = new Dictionary<int, UUIElement> ();
	}

	private Dictionary<string,UUIWindow> _window;
	private Dictionary<int,UUIElement> _tips;

	void Update(){
		foreach (var i in _window) {
			UUIWindow.UpdateUI( i.Value);
		}

		foreach (var i in _tips) 
		{
			
		}
	}


	public T CreateWindow<T>() where T:UUIWindow, new()
	{
		return new T ();
	}

}