﻿using System;
using UnityEngine;
using org.vxwo.csharp.json;
using System.Collections.Generic;
using ExcelConfig;

public class UAppliaction:XSingleton<UAppliaction>,ExcelConfig.IConfigLoader
{
	public UAppliaction ()
	{
		new ExcelConfig.ExcelToJSONConfigManager (this);
	}

    public static bool IsEditorMode = false;

	//private ExcelToJSONConfigManager manager;

	#region IConfigLoader implementation

	public List<T> Deserialize<T> () where T : ExcelConfig.JSONConfigBase
	{
		
		var type = typeof(T);
		var atts = type.GetCustomAttributes(typeof(ExcelConfig.ConfigFileAttribute), false) 
			as ExcelConfig.ConfigFileAttribute[];
		
		if(atts.Length>0)
		{
			var name = atts [0].FileName;
			//name = name.Substring(0,name.LastIndexOf('.'));
			//Debug.Log (atts [0].FileName + "->" + name);
			var json = ResourcesManager.Singleton.LoadText("Json/" + name);
			if (json == null)
				return null;
			return JsonTool.Deserialize<List<T>>(json);
		}

		return null;
	}

	#endregion

	public void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
	}

	void Update()
	{
        if (next != null)
        {
            gate = next;
            gate.JoinGate();
            next = null;
           
        }
		if (gate == null)
			return;
		gate.Tick ();
	}

	void Start()
    {
        if (!IsEditorMode)
            GoToMainGate();
    }


    public void GoToGameBattleGate(int levelId)
    {
        var gate = new UGameGate (levelId);
        ChangeGate (gate);
    }

    public void GoToMainGate()
    {
        var mainGate = new GMainGate();
        ChangeGate(mainGate);
    }

	public void ChangeGate(UGate g)
	{
		
		if (gate != null) {
			gate.ExitGate ();
		}

        gate = null;
        next = g;
	}

    private UGate next;

	private UGate gate;

	public UGate GetGate(){ return gate;}
}


