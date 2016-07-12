using System;
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
	public UUIWindow ()
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
}