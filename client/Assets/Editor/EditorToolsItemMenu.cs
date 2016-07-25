using System;
using UnityEditor;
using UnityEditor.SceneManagement;

public sealed class EditorToolsItemMenu
{
    [MenuItem("GAME/UI/INIT_LANGUAGE_FILE")]
    public static void CreateLanguage()
    {

    }

    [MenuItem("GAME/UI/GEN_CONST_VALUES")]
    public static void GenConstValues()
    {

    }

    [MenuItem("GAME/Go_To_EditScene")]
    public static void GoToEditorScene()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.Beep();
            return;
        }

        var editor ="Assets/EditorReleaseMagic.unity";
        EditorSceneManager.OpenScene(editor);
    }

    [MenuItem("GAME/Go_To_StarScene")]
    public static void GoToStarScene()
    {
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


