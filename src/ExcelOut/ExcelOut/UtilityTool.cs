using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using org.vxwo.csharp.json;

namespace ExcelOut.Libs.EX
{
    public class ExcelTable
    {
        public ExcelTable()
        {
            Cols = new List<ExcelTableCol>();
        }
        public string TableName { set; get; }
        public string ClassName { set; get; }
        public string FileName { set; get; }
        public string Description { set; get; }
        public List<ExcelTableCol> Cols { set; get; }
    }

    public class ExcelTableCol
    {
        public string Name { set; get; }
        public string Type { set; get; }
        public string Comment { set; get; }
        public int ColIndex { set; get; }
    }

    public class ExcelTool
    {
        public static string GetTypeByString(string str)
        {
            switch (str)
            {
                case "Int": return "int";
                case "String": return "String";
                case "Float": return "float";
                case "DateTime": return "DateTime";
            }
            return "object";
        }

        public static string GetPropety(ExcelTableCol col)
        {
            if(string.IsNullOrEmpty(col.Name)) return string.Empty;
            if (!IsCshapName(col.Name)) throw new Exception(string.Format("列名[{0}]不符合C#命名规则，请确定！！！", col.Name));
            string str = @"        
        /// <summary>
        /// {0}
        /// </summary>
        [ExcelConfigColIndex({3})]
        public {1} {2} ";
            return string.Format(str, col.Comment, GetTypeByString(col.Type), col.Name, col.ColIndex) + "{ set; get; }\n";
        }

        public static string GetClass(ExcelTable table)
        {
            if (!IsCshapName(table.ClassName))
                throw new Exception(string.Format("类名:[{0}]不符合C#命名规则，请确定！！！", table.ClassName));


            var str = "\n" + "    /// <summary>\n" +
            "    /// {3}\n" +
            "    /// </summary>\n" +
            "    [ConfigFile(\"{0}\",\"{2}\")]\n" +
            "    public class {1}:JSONConfigBase";
            var sb = new StringBuilder();
            foreach (var i in table.Cols)
            {
                if (i.Name == "ID") continue;
                sb.Append(GetPropety(i));
            }
            var cl = string.Format(str, table.FileName, table.ClassName, table.TableName, table.Description);
            cl = cl + "    {\n" + sb.ToString() + "\n    }\n";
            return cl;
        }

        public static string GetAllClass(List<ExcelTable> tables, string namesp)
        {
            if (!string.IsNullOrEmpty(namesp) && !IsCshapName(namesp)) throw new Exception(string.Format("命名空间[{0}]不符合C#命名规则，请确定！！！", namesp));
            var str = @"
/*
#############################################
       
       *此代码为工具自动生成
       *请勿单独修改该文件

#############################################
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelConfig;
";


            var sb = new StringBuilder();
            foreach (var i in tables)
            {
                sb.Append(GetClass(i));
            }

            if (!string.IsNullOrEmpty(namesp))
            {
                str += string.Format("namespace {0}\n", namesp);
            }
            return str + "{\n" + sb.ToString() + "\n }\n";

        }

        public static string GetExcelData(ExcelTable table, DataTable data)
        {
            var jv = new JsonValue();
            for (var i = 0; i < data.Rows.Count; i++)
            {
                var row = new JsonValue();
                var ig = false;
                foreach (var col in table.Cols)
                {
                    var v = data.Rows[i][col.ColIndex];
                    if (col.Name == "ID")
                    {

                        if (v is DBNull|| Convert.ToInt32(v) == 0)
                        {
                            ig = true;
                            break;
                        }
                    }
                    
                    switch (col.Type)
                    {
                        case "Int":
                            var valInt = 0;
                            try
                            {
                                valInt = Convert.ToInt32(v);
                            }
                            catch { }
                            row[col.Name] = valInt;

                            break;
                        case "Float":
                            var valFloat = 0f;
                            try
                            {
                                valFloat = Convert.ToSingle(v);
                            }
                            catch { }
                            row[col.Name] = valFloat;
                            break;
                        case "String":
                            row[col.Name] = v == null ? "" : v.ToString();
                            break;
                        case "DateTime":
                            row[col.Name] = v == null ? "" : v.ToString();
                            break;
                    }
                }
                if(!ig)
                jv.Append(row);
                //BREAK;
            }
            return JsonWriter.Write(jv);
        }

        #region Check named
        public static bool IsCshapName(string name)
        {
            foreach (var i in CShapKeys)
            {
                if (i.Equals(name))
                {
                    return false;
                }
            }

            return regex.IsMatch(name);
        }

        private static System.Text.RegularExpressions.Regex regex =
           new System.Text.RegularExpressions.Regex("^[@A-Za-z_]{1}[^ ]*$");

        private static string[] CShapKeys = new string[]{
           "abstract","as","base", "bool",
           "break","byte","case","catch","char","checked","class","const","continue","decimal",
           "default","delegate","do","double",
           "else","enum","event","explicit","extern",
           "false","finally","static","float","for",
           "foreach","goto","if","implicit","in","int",
           "interface","internal", "is","lock","long",
           "namespace","new","null","object","operator",
           "out","override","params","private","protected",
           "public","readonly","ref","return","sbyte",
           "sealed","short","sizeof","stackalloc","static",
           "string","struct","switch","this","throw","true","try",
           "typeof","uint","ulong","unchecked","unsafe",
           "ushort","using","virtual",
           "void","volatile","while"
        };

        #endregion
    }

}
