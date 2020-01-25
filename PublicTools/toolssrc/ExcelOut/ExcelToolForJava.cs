using ExcelOut.Libs.EX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOut
{

    public class ExcelToolForJava
    {

        public struct JaveFile
        {
            public string fileName;
            public string code;
        }
        public static string GetTypeByString(string str,bool isArray = false)
        {
            switch (str)
            {
                case "Int": return  !isArray?"int": "Integer";
                case "String": return "String";
                case "Float": return !isArray?"float": "Float";
                case "DateTime": return "java.util.Date";
            }
            return "Object";
        }

        public static string GetPropety(ExcelTableCol col)
        {
            if (string.IsNullOrEmpty(col.Name)) return string.Empty;
            string type = string.Format(col.IsArray ? "java.util.List<{0}>" : "{0}", GetTypeByString(col.Type,col.IsArray));
            string code = @"	/**
	 * {0}
	 * */
	@ColIndex(index={3})
	private {1} {2} {4};
";
            var field= string.Format(code, col.Comment, type, col.Name,
                col.ColIndex,
                col.IsArray? string.Format( "= new java.util.ArrayList<{0}>();", GetTypeByString(col.Type,col.IsArray)) :"");

           var setAndGet = @"	public [TYPE] get[NAME]() 
	{
		return this.[NAME];
	}

	public void set[NAME]([TYPE] [NAME])
	{
		this.[NAME] = [NAME];
	}";
           return  field + setAndGet.Replace("[NAME]", col.Name)
                .Replace("[TYPE]",type);
        }

        public static string GetClass(ExcelTable table,string namesp,string category)
        {
            var str = @"package {4};
import excelconfig.ColIndex;
import excelconfig.ConfigFile;
import excelconfig.ExcelConfigBase;
import tt.config.annotation.Config;
/**
*{3}
*/
" + "@ConfigFile(file=\"{0}\",table=\"{2}\") \n"+
"@Config(value =\"{5}\",category=\"{6}\")" +
@"
public class {1} extends ExcelConfigBase 
";
            var sb = new StringBuilder();
            foreach (var i in table.Cols)
            {
                if (i.Name == "ID") continue;
                if (i.IsArray) continue;
                sb.AppendLine(GetPropety(i));
            }

            foreach (var i in table.Arrays)
            {
                sb.AppendLine(GetPropety(i));
            }

            var temp =  
                string.Format(str, table.FileName, table.ClassName, table.TableName, table.Description,namesp,table.ClassName, category)
                +
                @"{"+sb.ToString()+@"
        }";
            return temp;
        }

        public static List<JaveFile> GetAllClass(List<ExcelTable> tables, string namesp,bool debug)
        {
            var list = new List<JaveFile>();
            foreach (var i in tables)
            {
                list.Add(new JaveFile { fileName = i.ClassName, code = GetClass(i, namesp,"excel") });
            }
            return list;
        }
    }

}
