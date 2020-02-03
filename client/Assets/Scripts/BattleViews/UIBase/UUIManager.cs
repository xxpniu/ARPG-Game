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
    protected RectTransform _rect;

    public RectTransform Rect
    {
        get{ 
            if (_rect)
                return _rect;
            else
            {
                _rect = this.uiRoot.GetComponent<RectTransform>();
                return _rect;
            }
        } 
    }

	public static void Destory(UUIElement el)
    {
		el.OnDestory ();
	}

    protected T FindChild<T>(string name) where T: Component
    {
        return uiRoot.transform.FindChild<T>(name);
    }
}

public class UUIManager:XSingleton<UUIManager>
{
	public void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
		//DontDestroyOnLoad (top);
        eventMask .SetActive(false);
	}

	private Dictionary<string,UUIWindow> _window=new Dictionary<string, UUIWindow> ();
    private Dictionary<int,UUITip> _tips= new Dictionary<int, UUITip> ();

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
            
	}

    public void UpdateUIData()
    {
        foreach (var i in _window)
        {
            UUIWindow.UpdateUIData(i.Value);
        }
    }

    public void UpdateUIData<T>()  where T: UUIWindow, new()
    {
        var ui=  GetUIWindow<T>();
        if (ui != null)
            UUIWindow.UpdateUIData(ui);
    }
    private Queue<UUITip> _tipDelTemp = new Queue<UUITip>();

    void LateUpdate()
    {
        foreach (var i in _tips)
        {
            if (i.Value.CanDestory)
            {
                _tipDelTemp.Enqueue(i.Value);
            }
            else
            {
                i.Value.LateUpdate();
            }
        }

        while (_tipDelTemp.Count > 0)
        {
            var tip = _tipDelTemp.Dequeue();
            _tips.Remove(tip.InstanceID);
            UUITip.Destory(tip);
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

    public T CreateWindow<T>() where T : UUIWindow, new()
    {
        var ui = GetUIWindow<T>();
        if (ui == null)
        {
            ui = UUIWindow.Create<T>(this.BaseCanvas.transform);
            _addTemp.Enqueue(ui);
        }
        return ui;
    }

    public T CreateTip<T>() where T:UUITip, new()
    {
        var tip=  UUITip.Create<T>(this.top.transform);
        this._tips.Add(tip.InstanceID, tip);
        return tip;
    }

    public bool TryToGetTip<T>(int id,out T tip)  where T:UUITip
    {
        UUITip t;
        if (_tips.TryGetValue(id, out t))
        {
            tip = t as T;
            return true;
        }
        tip = null;
        return false;
    }

	public void ShowMask(bool show)
    {
        var image= BackImage.GetComponent<ImageColor>();
        if (show)
        {
            image.Show();
            BackImage.transform.FindChild<AutoValueScrollbar>("Scrollbar").Reset(1);
        }
        else
        {
            image.Hide();
        }
    }

	public void ShowLoading(float p)
	{
		BackImage.transform.FindChild<Scrollbar> ("Scrollbar").size =  p;
        BackImage.transform.FindChild<Text>("Text").text = string.Empty;
	}

    public void ShowLoading(float start,float durtion,string text)
    {
        BackImage.transform.FindChild<Scrollbar> ("Scrollbar").size =  start;
        BackImage.transform.FindChild<Text>("Text").text = text;
    }

    private float? duration;

	public Image BackImage;
	public GameObject top;
    public Canvas BaseCanvas;

    private Transform rectTop;

    public Vector2 OffsetInUI(Vector3 position)
    {
        var pos = Camera.main.WorldToScreenPoint(position) ;
        //return new Vector2(pos.x/rectTop.localScale.x, pos.y/rectTop.localScale.y);
        return new Vector2(pos.x, pos.y);
        //return Vector2.zero;
    }

    public void HideAll()
    {
        foreach (var i in _window)
        {
            if (i.Value.IsVisable)
                i.Value.HideWindow();
        }
    }

    public GameObject eventMask;

    /// <summary>
    /// 当前mask
    /// </summary>
    public int maskCount = 0;

    public void MaskEvent()
    {
        maskCount++;
        eventMask.SetActive(maskCount > 0);
    }

    public void UnMaskEvent()
    {
        maskCount--;
        eventMask.SetActive(maskCount > 0);
    }
}