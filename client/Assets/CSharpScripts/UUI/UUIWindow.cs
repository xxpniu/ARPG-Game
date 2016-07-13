using System;
using UnityEngine;


public class UIResourcesAttribute:Attribute
{
	public UIResourcesAttribute(string name)
	{
		Name = name;
	}

	public string Name{ private set; get; }
}

public class UUIWindow
{
	protected UUIWindow ()
	{
	}

	protected virtual void OnCreate(){
	}

	protected virtual void OnDestory(){
	}

	protected virtual void OnShow(){
	}

	protected virtual void OnHide(){
	}

	protected virtual void OnUpdate()
	{
	}

	public void ShowWindow()
	{
		
	}

	public void HideWindow()
	{
		
	}

	private void Update(){
	}

	public static void UpdateUI(UUIWindow w)
	{
		w.Update ();
	}

	private GameObject uiRoot;

	public static T Create<T>() where T:UUIWindow, new() 
	{
		return new T ();
	}
}