using System;
using GameLogic.Utility;
using UnityEngine.SceneManagement;
using UnityEngine;
using Proto;
using UGameTools;
using Google.Protobuf;
using EConfig;
using EngineCore.Simulater;

public class UReplayGate : UGate
{
    public void Init(byte[] replayerData, int mapID)
    {
        var replayer = new NotifyMessagePool();
        replayer.LoadFormBytes(replayerData);
        Replayer = replayer;
        var data = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<MapData>(mapID);
        level = data.LevelName;
    }

    private string level;
    private NotifyMessagePool Replayer;
    private float startTime = -1f;
    private NotifyMessagePool.Frame frame;
    private NotifyPlayer player;
    private UPerceptionView PerView;

    protected override void JoinGate()
    {
        StartCoroutine(Load());
    }

    private GTime GetTime() => (PerView as ITimeSimulater).Now;

    private System.Collections.IEnumerator Load()
    {
        UUIManager.Singleton.ShowMask(true);
        UUIManager.Singleton.HideAll();
        UUIManager.Singleton.ShowLoading(0);
        var operation = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
        PerView = UPerceptionView.Create();
        while (!operation.isDone)
        {
            UUIManager.Singleton.ShowLoading(operation.progress);
            yield return null;
        }
        UUIManager.Singleton.ShowMask(false);
        startTime = GetTime().Time;
        player = new NotifyPlayer(PerView);
    }

    protected override void Tick()
    {
        if (startTime < 0) return;
        if (frame == null) Replayer.NextFrame(out frame);

        if (frame == null) return;

        if (frame.time > GetTime().Time - startTime) return;
        foreach (var i in frame.GetNotify()) Process(i);
        frame = null;

    }

    private void Process(IMessage notify)
    {
        player.Process(notify);
    }
}