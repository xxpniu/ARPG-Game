using System;
using GameLogic.Utility;
using UnityEngine.SceneManagement;
using UnityEngine;
using Proto;
using UGameTools;
using Google.Protobuf;
using EConfig;

public class UReplayGate:UGate
{
    public UReplayGate(byte[] replayerData,int mapID)
    {
        var replayer = new GameLogic.Utility.NotifyMessagePool();
        replayer.LoadFormBytes(replayerData);
        Replayer = replayer;
        var data = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<MapData>(mapID);
        level = data.LevelName;
    }
    private readonly string level;
    private readonly NotifyMessagePool Replayer;

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

    private NotifyMessagePool.Frame frame;
   
    #endregion
    private readonly NotifyPlayer player = new NotifyPlayer();
    private void Process(IMessage notify)
    {
        player.Process(notify);
    }
}