using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public sealed class EditorToolsItemMenu
{

    private static bool ShowSave()
    {
        for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
        {
            var s = EditorSceneManager.GetSceneAt(i);
            if (s.isDirty)
            {
                return EditorUtility.DisplayDialog("提示", "放弃保存？", "确定", "取消");
            }
        }

        return true;
    }
    [MenuItem("GAME/UI/INIT_LANGUAGE_FILE")]
    public static void CreateLanguage()
    {

    }
       

    [MenuItem("GAME/UI/GEN_CONST_VALUES")]
    public static void GenConstValues()
    {

    }

    [MenuItem("GAME/Go_To_EditScene &e")]
    public static void GoToEditorScene()
    {
        if (!ShowSave())
            return;
        
        if (EditorApplication.isPlaying)
        {
            EditorApplication.Beep();
            return;
        }

        var editor ="Assets/EditorReleaseMagic.unity";
        EditorSceneManager.OpenScene(editor);
        EditorApplication.isPlaying = true;
    }

    [MenuItem("GAME/Go_To_StarScene &s")]
    public static void GoToStarScene()
    {

        if (!ShowSave())
            return;
        if (EditorApplication.isPlaying)
        {
            EditorApplication.Beep();
            return;
        }
        var editor ="Assets/Welcome.unity";
        EditorSceneManager.OpenScene(editor);
        EditorApplication.isPlaying = true;

    }
    
}


