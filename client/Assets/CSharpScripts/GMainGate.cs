using System;
using EngineCore.Simulater;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GMainGate:UGate
{
    #region implemented abstract members of UGate

    public override void JoinGate()
    {
        operat=SceneManager.LoadSceneAsync("Main");
    }

    private AsyncOperation operat;

    public override void ExitGate()
    {
       
        var ui = UUIManager.Singleton.GetUIWindow<Windows.UUIMain>();
        if (ui == null)
            return;
        ui.HideWindow();
    }

    public override void Tick()
    {
        if (operat != null)
        {
            if (operat.isDone)
            {
                operat = null;
                var ui = UUIManager.Singleton.CreateWindow<Windows.UUIMain>();
                ui.ShowWindow();
                data = GameObject.FindObjectOfType<MainData>();
                var character = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.CharacterData>(4);
                var res = ResourcesManager.Singleton.LoadResourcesWithExName<GameObject>(character.ResourcesPath);
                var obj = GameObject.Instantiate(res, data.pos[0].position, Quaternion.identity);

            }
        }
    }

    private MainData data;

    public override GTime GetTime()
    {
        return new GTime(Time.time, Time.deltaTime);
    }

    #endregion

    public GMainGate()
    {
        
    }
}