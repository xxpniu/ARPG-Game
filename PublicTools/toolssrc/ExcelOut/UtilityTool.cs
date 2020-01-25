using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using org.vxwo.csharp.json;
using System.Text.RegularExpressions;

namespace ExcelOut.Libs.EX
{
    public enum TypeOfTable {
         ALL =0,
         Client = 1,
         Server =2
    }

    public class ExcelTable
    {
        public ExcelTable()
        {
            Cols = new List<ExcelTableCol>();
        }

        public static Regex RegOfArray =  new Regex("(^[^0-9]+)[0-9]+$");

        public TypeOfTable ExportType { set; get; }
        public string TableName { set; get; }
        public string ClassName { set; get; }
        public string FileName { set; get; }
        public string Description { set; get; }
        public List<ExcelTableCol> Cols { set; get; }
        public List<ExcelTableCol> Arrays { set; get; }

        public void ProcessColsArray()
        {
            Dictionary<string, string> names = new Dictionary<string, string>();
            Dictionary<string, string> des = new Dictionary<string, string>();
            Arrays = new List<ExcelTableCol>();
            Cols = Cols.OrderBy(t => t.ColIndex).ToList();
            foreach (var i in Cols)
            {
                var m = RegOfArray.Match(i.Name);
                if (m.Success)
                {
                    i.ArrayName = m.Groups[1].Value;
                    i.IsArray = true;
                    if (names.ContainsKey(i.ArrayName))
                    {
                        if (names[i.ArrayName] != i.Type)
                        {
                            throw new Exception(string.Format("Col:{0} belong array {1}. but type {2} not as {3}", i.Name, i.ArrayName, i.Type, names[i.ArrayName]));
                        }
                    }
                    else {
                        names.Add(i.ArrayName, i.Type);
                        des.Add(i.ArrayName, i.Comment);
                    }
                }
            }

            foreach (var i in names)
            {
                Arrays.Add(new ExcelTableCol
                {
                    ArrayName = i.Key,
                    IsArray = true,
                    Name = i.Key,
                    Type = i.Value,
                    Comment =  des[i.Key]
                });
            }
        }
    }

    public class ExcelTableCol
    {
        public string Name { set; get; }
        public string Type { set; get; }
        public string Comment { set; get; }
        public int ColIndex { set; get; }
        public bool IsArray { set; get; }
        public string ArrayName { set; get; }
    }

    public class ForeignRelation 
    {
        public string TableName { set; get; }
        public string Key { set; get; }
        public string ForeignTableName { set; get; }
        public string ForeignKey { set; get; }

        public override string ToString()
        {
            return string.Format("{0}.{1}->{2}.{3}", TableName, Key, ForeignTableName, ForeignKey);
        }
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

        public static string GetPropety(ExcelTableCol col,int index )
        {
            if (string.IsNullOrEmpty(col.Name)) return string.Empty;
            if (!IsCshapName(col.Name)) throw new Exception(string.Format("列名[{0}]不符合C#命名规则，请确定！！！", col.Name));
            string str = @"        
        /// <summary>
        /// {0}
        /// </summary>
        [ExcelConfigColIndex({3})]
        public {1} {2} ";

            var des = string.Empty;
            if (!string.IsNullOrEmpty(col.Comment))
            {
                des = col.Comment.Replace("\n", "<br/>");
            }
            return string.Format(str, 
                des, 
                string.Format( col.IsArray?"List<{0}>":"{0}", GetTypeByString(col.Type)), col.Name, col.ColIndex) + "{ set; get; }\n";
        }

        public static string GetClass(ExcelTable table)
        {
            if (!IsCshapName(table.ClassName))
                throw new Exception(string.Format("类名:[{0}]不符合C#命名规则，请确定！！！", table.ClassName));


            var str = "\n" + "    /// <summary>\n" +
            "    /// {3}\n" +
            "    /// </summary>\n" +
            "    [ConfigFile(\"{0}\",\"{2}\")]\n" +
            "    [global::System.Serializable]\n"+
            "    public class {1}:JSONConfigBase";
            var sb = new StringBuilder();
            table.ProcessColsArray();
            int proIndex = 2;
            foreach (var i in table.Cols)
            {
                if (i.Name == "ID") continue;
                if (i.IsArray) continue;
                sb.Append(GetPropety(i,proIndex++));
            }
            foreach (var i in table.Arrays)
            {
                sb.Append(GetPropety(i,proIndex++));
            }

            var des = string.Empty;
            if (!string.IsNullOrEmpty(table.Description))
            {
                des = table.Description.Replace("\n", "<br/>");
            }
            var cl = string.Format(str, table.FileName, table.ClassName, table.TableName, des);
            cl = cl + "    {\n" + sb.ToString() + "\n    }\n";
            return cl;
        }

        public static string GetAllClass(List<ExcelTable> tables, string namesp,bool debug)
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

        public static string GetExcelData(ExcelTable table, DataTable data,bool debug, out JsonValue values)
        {
            values = new JsonValue();
            var sb = new StringBuilder();
            sb.Append("[\n");
            bool isFir = true;
            var hashSet = new HashSet<int>();
            for (var i = 0; i < data.Rows.Count; i++)
            {
                var row = new JsonValue();
               
                var ig = false;
                foreach (var col in table.Cols)
                {
                    if (col.IsArray) continue;
                    var v = data.Rows[i][col.ColIndex];
                    if (col.Name == "ID")
                    {
                        if (v is DBNull || Convert.ToInt32(v) <= 0)
                        {
                            ig = true;
                            break;
                        }

                        var id = Convert.ToInt32(v);

                        if (hashSet.Contains(id))
                        {
                            throw new Exception("Id:" + id + " is existed in table[" + table.ClassName+"]");
                        }
                        hashSet.Add(id);
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
                foreach (var acol in table.Arrays)
                {
                    var ar = new JsonValue();

                    foreach (var col in table.Cols)
                    {
                        if (!col.IsArray) continue;
                        var reg = new Regex("^" + acol.Name + "[0-9]+$");
                        if (!reg.Match(col.Name).Success) continue;
                        var v = data.Rows[i][col.ColIndex];
                        switch (acol.Type)
                        {
                            case "Int":
                                var valInt = 0;
                                try
                                {
                                    valInt = Convert.ToInt32(v);
                                }
                                catch { }
                                ar.Append( valInt);

                                break;
                            case "Float":
                                var valFloat = 0f;
                                try
                                {
                                    valFloat = Convert.ToSingle(v);
                                }
                                catch { }
                                ar.Append(valFloat);
                                break;
                            case "String":
                                ar.Append(v == null ? "" : v.ToString());
                                break;
                            case "DateTime":
                                ar.Append(v == null ? "" : v.ToString());
                                break;
                        }
                    }
                    row[acol.Name] = ar;
                }
                var rowJson = JsonWriter.Write(row);
                if (ig)
                {
                    continue;
                }
                values.Append(row);
                if (!isFir)
                {
                    sb.Append(',');
                    sb.AppendLine();
                }
                isFir = false;
                sb.Append("\t");
                sb.Append(rowJson);
                if (debug)
                    Console.WriteLine(rowJson);
            }
             sb.Append("\n]");

            return sb.ToString();
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

            foreach (var i in exchars)
            {
                if (name.IndexOf(i) > -1) return false;
            }

            return regex.IsMatch(name);
        }


        private static char[] exchars = new char[] 
        {
            ' ','\n','\r','\t','[',']','(',')','%','*','&','+','-','^','!','~','>','<'
        };
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