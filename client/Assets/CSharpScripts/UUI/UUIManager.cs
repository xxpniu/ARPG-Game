using System;
using System.Collections.Generic;
using UGameTools;
using UnityEngine.UI;


public class UUIElement
{
	
}

public class UUIManager:XSingleton<UUIManager>
{
	public void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
	}

	private Dictionary<string,UUIWindow> _window=new Dictionary<string, UUIWindow> ();
	private Dictionary<int,UUIElement> _tips= new Dictionary<int, UUIElement> ();

	void Update(){



		foreach (var i in _window) {
			UUIWindow.UpdateUI( i.Value);
		}

		foreach (var i in _tips) 
		{
			
		}
	}

	public void GetUIWindow<T>()
	{
		
	}

	public T CreateWindow<T>() where T:UUIWindow, new()
	{
		
		return	UUIWindow.Create<T> ();
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

}