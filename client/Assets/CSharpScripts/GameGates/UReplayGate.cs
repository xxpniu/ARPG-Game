using System;
using GameLogic.Utility;
using UnityEngine.SceneManagement;
using UnityEngine;
using Proto;
using UGameTools;

public class UReplayGate:UGate
{
    public UReplayGate(byte[] replayerData,int mapID)
    {
        var replayer = new GameLogic.Utility.NotifyMessagePool();
        replayer.LoadFormBytes(replayerData);
        Replayer = replayer;
        var data = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.LevelData>(mapID);
        level = data.LevelResouceName;
    }
    private string level;
    private NotifyMessagePool Replayer;

    private AsyncOperation operation;

    #region implemented abstract members of UGate
    public override void JoinGate()
    {
        UUIManager.Singleton.ShowMask (true);
        UUIManager.Singleton.ShowLoading (0);
        operation = SceneManager.LoadSceneAsync (level, LoadSceneMode.Single);
        UUIManager.Singleton.HideAll();
    }
    public override void ExitGate()
    {
        //throw new NotImplementedException();
    }

    private float startTime = 0f;
    public override void Tick()
    {
        if (operation != null)
        {
            if (!operation.isDone)
            {
                UUIManager.Singleton.ShowLoading(operation.progress);
                return;
            }
            operation = null;
          
            UUIManager.Singleton.ShowMask(false);
            startTime = GetTime().Time;
        }

        if (frame == null)
        {
            Replayer.NextFrame(out frame);
        }

        if (frame != null)
        {
            if (frame.time > GetTime().Time - startTime)
                return;
            foreach (var i in frame.GetNotify())
                Process(i);
            frame = null;
        }
    }

    private GameLogic.Utility.NotifyMessagePool.Frame frame;


    public override EngineCore.Simulater.GTime GetTime()
    {
        return new EngineCore.Simulater.GTime(Time.time, Time.deltaTime);
    }
    #endregion
    private NotifyPlayer player = new NotifyPlayer();
    private void Process(ISerializerable notify)
    {
        player.Process(notify);
    }
}