using System.Collections;
using System.Collections.Generic;
using System.IO;
using org.vxwo.csharp.json;
using UnityEngine;
using UnityEngine.SceneManagement;
using XNet.Libs.Utility;

public class StartType : MonoBehaviour
{
    private class UnityLoger : Loger
    {
        #region implemented abstract members of Loger
        public override void WriteLog(DebugerLog log)
        {
            switch (log.Type)
            {
                case LogerType.Error:
                    Debug.LogError(log);
                    break;
                case LogerType.Log:
                    Debug.Log(log);
                    break;
                case LogerType.Waring:
                case LogerType.Debug:
                    Debug.LogWarning(log);
                    break;
            }

        }
        #endregion   
    }


    // Start is called before the first frame update
    IEnumerator Start()
    {
        Debuger.Loger = new UnityLoger();
        Application.targetFrameRate = 30;

        yield return new WaitForEndOfFrame();
  
#if UNITY_SERVER
        scene = "Server";
#else
#if !UNITY_EDITOR
        scene = "Application";
#endif
#endif

        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        Destroy(this);
    }

    [Header("Type:Server Application")]
    public string scene;
}
