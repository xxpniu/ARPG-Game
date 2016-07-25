using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
//using Assets.Scripts;
//using Assets.Scripts.Tools;
using System;

public class UICreater : EditorWindow
{
    [MenuItem("GAME/UI/INIT_LANGUAGE_FILE")]
    public static void CreateLanguage()
    {
       
    }

    [MenuItem("GAME/GEN_CONST_VALUES")]
    public static void GenConstValues()
    {

    }

    [MenuItem("GAME/UI/AUTO_GEN_WINDOWS_CODE")]
    public static void OpenEditor()
    {
        var window = (UICreater)GetWindow(typeof(UICreater), true, "UICodeCreater");
        //window.Show();
    }

    private static UICreater _ui;
    public void OnGUI()
    { 
        if (Selection.activeGameObject != currentSelect)
        {
            Names = new Dictionary<string, string>();
            Tables = new Dictionary<string, TableComponent>();
            currentSelect = Selection.activeGameObject;
            className = currentSelect.name;
            Init(currentSelect.transform);
        }

        EditorGUILayout.BeginVertical();

        GUILayout.Label("Tag:"+EXPORT_TAG+" Will Be Export(请保证到处元素的唯一性)");
        GUILayout.Space(20);
        if (currentSelect != null && Names != null)
        {
            GUILayout.Label(string.Format("找到{0}个UI控件",Names.Count));
            GUILayout.BeginHorizontal();
            windowsRoot = EditorGUILayout.TextField("Code Path:", windowsRoot);
            if(GUILayout.Button("Select",GUILayout.Width(100)))
            {
                windowsRoot = EditorUtility.SaveFolderPanel("Selet Code Path",Path.Combine(Application.dataPath,"CSharpScripts/UUI/Windows"),"");
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.TextField("ClassName:", className);
            GUILayout.Label("UITemplate File Name:" + className + ".ui.cs");

            createModelFile = EditorGUILayout.ToggleLeft("创建逻辑文件（PS:将覆盖原有逻辑文件，非首次创建建议不要选择）", createModelFile);
            if (showExample = EditorGUILayout.Foldout(showExample, "代码概要"))
            {
                scroll = GUILayout.BeginScrollView(scroll, GUILayout.Height(200));
                EditorGUILayout.BeginVertical();
                foreach (var i in Names)
                {
                    GUILayout.Label(string.Format("public {1} {0}", i.Key, i.Value) + " {set;get;}");
                }
                foreach(var i in Tables)
                {
                    GUILayout.Label("Table:"+ i.Key);
                    foreach(var f in i.Value.Components)
                    {
                        GUILayout.Label(string.Format("   public {1} {0}", f.Key, f.Value) + " {set;get;}");
                    }
                }
                EditorGUILayout.EndVertical();
                GUILayout.EndScrollView();
            }

            var rect = new Rect(position.width - 105, position.height - 25, 100, 20);
            if(GUI.Button(rect,"Gen"))
            {
                if(EditorUtility.DisplayDialog("Save File To:", windowsRoot,"Create","Cancel"))
                {
                    Export();
                }
            }
        }
        EditorGUILayout.EndVertical();


    }

    private  static string tableTemplateField =
@"            public [Type] [Name];";
    private static string tableTemlateFindField =
@"                [Name] = FindChild<[Type]>("+"\"[Name]\""+");";
    private static string tableTemplateClass =
@"        public class [TableName]TableTemplate : TableItemTemplate
        {
            public [TableName]TableTemplate(){}
[TableTemplateField]
            public override void InitTemplate()
            {
[TableTemplateFindField]
            }
        }";
private static string templateFields =
        @"        protected [Type] [Name];";
    private static string templateTableManager=
        @"        protected UITableManager<AutoGenTableItem<[TableName]TableTemplate, [TableName]TableModel>> [TableName]TableManager = new UITableManager<AutoGenTableItem<[TableName]TableTemplate, [TableName]TableModel>>();";
    private static string templateFieldFind=
@"            [Name] = FindChild<[Type]>("+"\"[Name]\""+");";
    private static string templateInitTable =
@"            [TableName]TableManager.InitFrom[TableType]([TableName]);";
    private static string templateFile =
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UGameTools;
using UnityEngine.UI;
//AUTO GenCode Don't edit it.
namespace Windows
{
    [UIResources(" + "\"[ResourceName]\"" + @")]
    partial class [ClassName] : UUIAutoGenWindow
    {
[TableTemplates]

[Fields]

[TableManagers]

        protected override void InitTemplate()
        {
            base.InitTemplate();
[FieldFinds]
[InitTables]
        }
    }
}";
    private static string tableModelClass =
@"        public class [TableName]TableModel : TableItemModel<[TableName]TableTemplate>
        {
            public [TableName]TableModel(){}
            public override void InitModel()
            {
                //todo
            }
        }";
    private static string modelFile =
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;

namespace Windows
{
    partial class [ClassName]
    {
[TableModels]
        protected override void InitModel()
        {
            base.InitModel();
            //Write Code here
        }
        protected override void OnShow()
        {
            base.OnShow();
        }
        protected override void OnHide()
        {
            base.OnHide();
        }
    }
}";

    private void Export()
    {
        var fields = new StringBuilder();
        var fieldFinds = new StringBuilder();
        foreach (var i in Names)
        {
            fields.AppendLine(templateFields.Replace("[Name]", i.Key).Replace("[Type]", i.Value));
            fieldFinds.AppendLine(templateFieldFind.Replace("[Name]", i.Key).Replace("[Type]", i.Value));
        }

        var tableModel = new StringBuilder();
        var tableTemplate = new StringBuilder();
        var tableInt = new StringBuilder();
        var tableManager = new StringBuilder();
        foreach (var i in Tables)
        {
            var tempModel = tableModelClass.Replace("[TableName]", i.Key);
            var tempTemplate = tableTemplateClass.Replace("[TableName]", i.Key);

            var tfields = new StringBuilder();
            var tfieldFinds = new StringBuilder();
            foreach (var f in i.Value.Components)
            {
                tfields.AppendLine(tableTemplateField.Replace("[Name]", f.Key).Replace("[Type]", f.Value));
                tfieldFinds.AppendLine(tableTemlateFindField.Replace("[Name]", f.Key).Replace("[Type]", f.Value));
            }
            tempTemplate = tempTemplate.Replace("[TableTemplateField]", tfields.ToString()).Replace("[TableTemplateFindField]", tfieldFinds.ToString());
            //tempModel.Replace();
            tableModel.AppendLine(tempModel);
            tableTemplate.AppendLine(tempTemplate);
            tableManager.AppendLine(templateTableManager.Replace("[TableName]", i.Key));
            //TableType
            tableInt.AppendLine(templateInitTable.Replace("[TableName]", i.Key)
                .Replace("[TableType]", i.Value.Type == TableTypes.UIGrid ? "Grid" : "Table"));
        }

        if (createModelFile)
        {
            var modeCode = modelFile.Replace("[TableModels]", tableModel.ToString()).Replace("[ClassName]", className);
            File.WriteAllText(Path.Combine(windowsRoot, className + ".cs"), modeCode);
        }

        var templateCode = templateFile.Replace("[ClassName]", className)
            .Replace("[ResourceName]", className)
            .Replace("[Fields]", fields.ToString())
            .Replace("[FieldFinds]", fieldFinds.ToString())
            .Replace("[InitTables]", tableInt.ToString())
            .Replace("[TableTemplates]", tableTemplate.ToString())
            .Replace("[TableManagers]", tableManager.ToString());
        File.WriteAllText(Path.Combine(windowsRoot, className + ".ui.cs"), templateCode);
        AssetDatabase.Refresh();
    }


    private Vector2 scroll;
    private bool showExample = true;
    private string className = string.Empty;
    private string windowsRoot=string.Empty;
    private bool createModelFile = false;

    private static Type[] types = new Type[]{
        typeof(GridLayoutGroup),
        typeof(VerticalLayoutGroup),
        typeof(Button),
        typeof(Slider),
        typeof(Text),
        typeof(Toggle),
        typeof(ToggleGroup),
        typeof(InputField),
        typeof(Dropdown),
        typeof(Scrollbar),
        typeof(ScrollRect),
        typeof(Image),
    };

    private Component GetComponet(Transform root)
    {
        foreach (var i in types)
        {
            var t = root.GetComponent(i);
            if (t == null)
                continue;
            return t;
        }

        return null;
    }

    public void Init(Transform root)
    {
        #region CollectItem
        if (root.gameObject.tag == EXPORT_TAG)
        {
            
            var ui = GetComponet(root);
            if (ui != null)
            {
                Debug.Log(string.Format("Name:{0} Tag:{1}", ui.name, root.tag));
                if (!Names.ContainsKey(ui.name))
                {
                    Names.Add(ui.name, ui.GetType().Name);
					if (typeof(GridLayoutGroup) == ui.GetType())
                    {
                        var table = new TableComponent();
                        table.Name = ui.name;
                        table.Type = TableTypes.UIGrid;
                        for (var i = 0; i < root.childCount; i++)
                        {
                            GetChildExportItems(root.GetChild(i), table.Components);
                        }
                        Tables.Add(table.Name, table);
                        return;
                    }

					if (typeof(VerticalLayoutGroup) == ui.GetType())
                    {
                        var table = new TableComponent();
                        table.Name = ui.name;
                        table.Type = TableTypes.UITable;
                        for (var i = 0; i < root.childCount; i++)
                        {
                            GetChildExportItems(root.GetChild(i), table.Components);
                        }
                        Tables.Add(table.Name, table);
                        return;
                    }
                }
            }
            else
            {
                if (!Names.ContainsKey(root.name))
                    Names.Add(root.name, root.GetType().Name);
            }
        }
        #endregion

        for(var i=0;i<root.childCount;i++)
        {
            Init(root.GetChild(i));
        }
    }

    private void GetChildExportItems(Transform root, Dictionary<string, string> dic)
    {
        if(root.tag == EXPORT_TAG)
        {
            var ui = this.GetComponet(root);
            if(ui!=null)
            {
                if (!dic.ContainsKey(ui.name))
                {
                    dic.Add(ui.name, ui.GetType().Name);
                }
                else {
                    Debug.LogError("name is exists !!-> Name:" + ui.name);
                }
            }
            else
            {
                var trans = root.transform;
                if (!dic.ContainsKey(trans.name))
                {
                    dic.Add(trans.name, trans.GetType().Name);
                }
                else
                {
                    Debug.LogError("name is exists !!-> Name:" + trans.name);
                }
            }
        }

        for(var i=0;i<root.childCount;i++)
        {
            GetChildExportItems(root.GetChild(i), dic);
        }
    }
    private const string EXPORT_TAG = "Export";

    private Dictionary<string, string> Names;
    private Dictionary<string, TableComponent> Tables;
    

    private GameObject currentSelect;


    public class TableComponent
    {
        public TableComponent() {
            Components = new Dictionary<string, string>();
        }
        public string Name { set; get; }

        public Dictionary<string, string> Components { set; get; }

        public TableTypes Type { set; get; }
    }

    public enum TableTypes
    {
        UITable,
        UIGrid
    }
}
