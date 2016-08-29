using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

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
    [MenuItem("GAME/Level/Export_Level_Grid")]
    public static void ExportLevelGrids()
    {
        var astart = GameObject.FindObjectOfType<AstarGridBase>();
        if (astart == null)
        {
            EditorUtility.DisplayDialog("No Found", "No Foun AstarGridBase!", "OK");
            return;
        }

        var grid = new Proto.MapGridData();
        var list = new List<Proto.MapNode>();

        for (var x = 0; x < astart.grid.maxX; x++)
        {
            for (var z = 0; z < astart.grid.maxZ; z++)
            {
                var n = new Proto.MapNode();
                var node = astart.grid.grid[x, 0, z];
                n.X = node.x;
                n.Y = node.y;
                n.Z = node.z;
                n.IsWalkable = node.isWalkable;
                list.Add(n);
            }
        }

        grid.Nodes = list;
        grid.MaxX = astart.grid.maxX;
        grid.MaxY = astart.grid.maxY;
        grid.MaxZ = astart.grid.maxZ;
        grid.Offset = new Proto.Vector3{ x = astart.grid.offsetX, y = astart.grid.offsetY, z = astart.grid.offsetZ };
        grid.Size = new Proto.Vector3{ x = astart.grid.sizeX, y = astart.grid.sizeY, z = astart.grid.sizeZ };

        var monsters = GameObject.FindObjectsOfType<MonsterGroupPosition>();
        var player = GameObject.FindObjectOfType<PlayerBornPosition>();
        foreach (var i in monsters)
        {
            var node= astart.grid.GetNodeFromVector3((int)i.transform.position.x, (int)i.transform.position.y, (int)i.transform.position.z);
            if (node == null)
                continue;
            grid.Monsters.Add(new Proto.MapMonsterGroup
                { 
                    Pos = new Proto.MapNode
                    {
                        X = node.x, Y = node.y, Z = node.z
                    },
                    CanBeBoss = i.CanBeBoss
                });
        }

        if (player != null)
        {
            var node = astart.grid.GetNodeFromVector3(
                           (int)player.transform.position.x, 
                           (int)player.transform.position.y,
                           (int)player.transform.position.z);
            if (node != null)
            {
                grid.Born = new Proto.MapNode
                {
                    X = node.x, Y = node.y, Z = node.z
                };
            }
        }

        using (var mem = new MemoryStream())
        {
            using (var bw = new BinaryWriter(mem))
            {
                grid.ToBinary(bw);
            }

            var fileName= EditorUtility.SaveFilePanel("Save grid", Application.dataPath, astart.name, "bin");
            File.WriteAllBytes(fileName, mem.ToArray());
        }
    }
}


