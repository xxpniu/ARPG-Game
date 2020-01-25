using ExcelOut.Libs.EX;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Excel;
using System.Text.RegularExpressions;
using org.vxwo.csharp.json;

namespace ExcelOut
{
    class Program
    {
        static void Main(string[] args)
        {
            //var table = new ExcelTable();
            //table.Cols.Add(new ExcelTableCol { Name = "Fix21", Type = "Int" });
            //table.Cols.Add(new ExcelTableCol { Name = "Fix22", Type = "Int" });
            //table.Cols.Add(new ExcelTableCol { Name = "Fix23", Type = "Int" });
            //table.Cols.Add(new ExcelTableCol { Name = "Fix24", Type = "Int" });
            //table.Cols.Add(new ExcelTableCol { Name = "Fix25", Type = "Int" });

            //table.Cols.Add(new ExcelTableCol { Name = "Fix2_1", Type = "Int" });
            //table.Cols.Add(new ExcelTableCol { Name = "Fix2_2", Type = "Int" });
            //table.Cols.Add(new ExcelTableCol { Name = "Fix2_3", Type = "Int" });
            //table.Cols.Add(new ExcelTableCol { Name = "Fix2_4", Type = "Int" });
            //table.Cols.Add(new ExcelTableCol { Name = "Fix2_5", Type = "Int" });
            //table.ProcessColsArray();
            //return;
            //args

            //args =@"dir:../ExcelConfig namespace:ExcelConfig exportJson:../ exportCs:../src/NetSources/Config.cs ex:*.xlsx exportType:server".Split(' ');

            // dir: namespace: class: exportJson: exportCs: ex:.xls
            string dir = string.Empty;
            string nameSpace = string.Empty;
            string exportJson = string.Empty;
            string exportCs = string.Empty;
            string ex = "*.xlsx";
            string mode = "java";
            int exportType = 0;
            bool isDebug = false;

            #region parmas
            foreach (var i in args)
            {
                if (i.StartsWith("dir:"))
                {
                    dir = i.Replace("dir:", "");
                }
                if (i.StartsWith("namespace:"))
                {
                    nameSpace = i.Replace("namespace:", "");
                }
                if (i.StartsWith("exportJson:"))
                {
                    exportJson = i.Replace("exportJson:", "");
                }
                if (i.StartsWith("exportCs:"))
                {
                    exportCs = i.Replace("exportCs:", "");
                }
                if (i.StartsWith("ex:"))
                {
                    ex = i.Replace("ex:", "");
                }
                if (i.StartsWith("mode:"))
                {
                    mode = i.Replace("mode:", "");
                }
                if (i.StartsWith("exportType:"))
                {
                    var ty = i.Replace("exportType:", "");
                    if ("server".Equals(ty.ToLower()))
                    {
                        exportType = (int)(TypeOfTable.Server);
                    }
                    else if ("client".Equals(ty.ToLower()))
                    {
                        exportType = (int)(TypeOfTable.Client);
                    }
                    else
                    {
                        exportType = (int)(TypeOfTable.ALL);
                    }
                }

                if (i.StartsWith("debug:"))
                {
                    var b = i.Replace("debug:", "");
                    isDebug = "true".Equals(b);
                }
            }
            #endregion

            Console.WriteLine(string.Format("dir:{0} nameSpace:{1}  exportJson:{2} exportCs:{3} ex:{4} mode:{5}",
            dir, nameSpace, exportJson, exportCs, ex, mode
            ));
            Process(exportCs, dir, ex, exportJson, nameSpace, mode, exportType, isDebug);

        }

        public static void Process(string exportCs,
            string dir, 
            string find, 
            string jsonDir, 
            string namesp, 
            string mode,
            int exportType,
            bool debug)
        {

            if (string.IsNullOrEmpty(exportCs) || string.IsNullOrEmpty(dir)) return;

            var files = System.IO.Directory.GetFiles(dir, find, System.IO.SearchOption.TopDirectoryOnly);
            var outTables = new List<ExcelTable>();

            Dictionary<string,ForeignRelation> relation;
            Dictionary<string, JsonValue> tables;
            ReadTables(outTables, files, jsonDir,exportType, debug, out tables,out relation);

            foreach (var v in relation)
            {
                var i = v.Value;
                Console.WriteLine("BeingCheck:" + i);
                if (!tables.ContainsKey(i.ForeignTableName))
                {
                    Console.WriteLine("Ig:" + i.ForeignTableName);
                    continue;
                }
                var td = tables[i.TableName];
                var fd = tables[i.ForeignTableName];
                for (var index = 0; index < td.Count; index++)
                {
                    var row = td.GetAt(index);
                    var rowC = row[i.Key];
                    //Console.WriteLine(string.Format("Json:{0}", JsonWriter.Write(rowC)));
                    for (var indexR = 0;indexR<rowC.Count;indexR++)
                    {
                        var temp = rowC.GetAt(indexR);
                        //Console.WriteLine(string.Format("Json:{0}", JsonWriter.Write(temp)));
                        var have = false;
                        for (var tempIndex = 0; tempIndex < fd.Count; tempIndex++)
                        {
                            var value = fd.GetAt(tempIndex)[i.ForeignKey];
                            //Console.WriteLine(string.Format("Json:{0}", JsonWriter.Write(value)));
                            if (temp.IsInt())
                            {
                                if (temp.AsInt() <= 0) have = true;
                                else if (value.AsInt() == temp.AsInt()) have = true;
                            }
                            if (temp.IsString())
                            {
                                if (value.AsString() == temp.AsString()) have = true;
                            }
                        }

                        if (!have)
                        {
                            var tt = JsonWriter.Write(temp);
                            if (temp.IsInt())
                            {
                                tt =""+ temp.AsInt();
                            }
                            if (temp.IsString())
                            {
                                tt = temp.AsString();
                            }
                            var error = string.Format("No found {0} in {1}",tt, i);
                            Console.WriteLine(error);
                            throw new Exception(error);
                        }
                    }
                }
            }
            if (mode == "csharp")
            {
                var file = Libs.EX.ExcelTool.GetAllClass(outTables, namesp,debug);
                System.IO.File.WriteAllText(exportCs, file);
            }
            if (mode == "java")
            {
                var javaFiles = ExcelOut.ExcelToolForJava.GetAllClass(outTables, namesp,debug);
                foreach (var i in javaFiles)
                {
                    var fileName = Path.Combine(exportCs, i.fileName + ".java");
                    Console.WriteLine("Export:" + fileName);
                    System.IO.File.WriteAllText(fileName, i.code);
                }
            }
            Console.WriteLine("导出成功！！");
        }

        private static Regex RegexName = new System.Text.RegularExpressions.Regex("([^\\(]*)\\(([^\\)]*)\\)");

        
        private static void ReadTables(List<ExcelTable> outTables,
          string[] files,
          string jsonDir,int type,bool debug,
          out Dictionary<string, JsonValue> tableData, 
          out Dictionary<string,ForeignRelation> rlist)
        {
            var outList = new Dictionary<string, ExcelTable>();
            rlist = new Dictionary<string, ForeignRelation>();
            tableData = new Dictionary<string, JsonValue>();
            #region readTable and Data
            foreach (var filePath in files)
            {
                Console.WriteLine("Parse:" + filePath);
                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                //...
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //...
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                //DataSet result = excelReader.AsDataSet();
                //...
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = false;
                DataSet result = excelReader.AsDataSet();
                var _base = result.Tables["__Base"];
                var exTables = new List<ExcelTable>();

                #region Table

                if (_base!=null && _base.Rows != null)
                {
                    for (var i = 0; i < _base.Rows.Count; i++)
                    {
                        var table = new Libs.EX.ExcelTable();

                        var row = _base.Rows[i];
                        table.TableName = row[1].ToString();
                        if (debug)
                            Console.WriteLine("Export Table:" + table.TableName);
                        if (string.IsNullOrEmpty(table.TableName)) break;
                        if (!ExcelTool.IsCshapName(table.TableName))
                        {
                            throw new Exception("Not A Name Of csharp!!! [" + table.TableName+"]");
                        }
                        table.ExportType = (TypeOfTable)int.Parse(row[0].ToString());
                        table.ClassName = row[2].ToString();
                        table.FileName = row[3].ToString();
                        table.Description = row[4].ToString();
                        if (table.ExportType != TypeOfTable.ALL && table.ExportType != (TypeOfTable)type) continue;
                        exTables.Add(table);

                    }
                }
                #endregion

                var reg = new System.Text.RegularExpressions.Regex("[\\d]+");

                foreach (var table in exTables)
                {
                    #region Read col
                    if (debug)
                        Console.WriteLine("Read Table:" + table.TableName);
                    var data = result.Tables[table.TableName];
                    var dataTable = new DataTable();
                    for (var col = 1; col < data.Columns.Count; col++)
                    {

                        var colnum = new ExcelTableCol();
                        colnum.ColIndex = col - 1;
                        colnum.Comment = data.Rows[2][col].ToString();
                        colnum.Name = data.Rows[0][col].ToString();
                        colnum.Type = data.Rows[1][col].ToString();

                        var m = RegexName.Match(colnum.Name);
                        if (m.Success)
                        {
                            colnum.Name = m.Groups[1].Value;

                            var r = m.Groups[2].Value;
                            Regex arrReg = ExcelTable.RegOfArray;
                            var cN = colnum.Name;
                            var am = arrReg.Match(cN);
                            if (am.Success)
                            {
                                cN = am.Groups[1].Value;
                            }

                            var re = new ForeignRelation
                            {
                                TableName = table.TableName,
                                Key = cN,
                                ForeignTableName = r.Split('.')[0],
                                ForeignKey = r.Split('.')[1],
                            };
                            if (debug)
                            {
                                Console.WriteLine("Foreign:" + re.ToString());
                            }

                            var tK = string.Format("{0}.{1}", re.TableName, re.Key);
                            if (!rlist.ContainsKey(tK))
                                rlist.Add(tK, re);
                        }
                        

                        if (string.IsNullOrEmpty(colnum.Name)
                            || colnum.Name == "0" || colnum.Name == "-1")
                            break;

                        if (!ExcelTool.IsCshapName(colnum.Name))
                        {
                            throw new Exception(string.Format("[{0}] Not a csharp name!", colnum.Name));
                        }
                        table.Cols.Add(colnum);
                        var coln = data.Columns[col];
                        dataTable.Columns.Add(new DataColumn
                        {
                            AllowDBNull = true,
                            ColumnName = coln.ColumnName
                        });
                        if (debug)
                            Console.WriteLine(string.Format("{1}-{0} {2}", colnum.Name, colnum.ColIndex, colnum.Type));

                    }
                    #endregion
                    table.ProcessColsArray();
                    #region Read row
                    for (var i = 3; i < data.Rows.Count; i++)
                    {
                        if (data.Rows[i][1] == null)
                            continue;
                        var row = dataTable.NewRow();
                        for (var col = 1; col < data.Columns.Count && col < dataTable.Columns.Count + 1; col++)
                        {
                            row[col - 1] = data.Rows[i][col];
                        }

                        dataTable.Rows.Add(row);
                    }
                    #endregion
                    #region Save Json File
                    JsonValue values;
                    var json = ExcelTool.GetExcelData(table, dataTable,debug, out  values);
                    if (debug)
                        Console.WriteLine( string.Format("Write json data of {0} path:[{1}]", table.TableName,table.FileName));
                    File.WriteAllText(Path.Combine(jsonDir, table.FileName), json);
                    #endregion
                    outList.Add( table.ClassName, table);
                    tableData.Add(table.TableName, values);
                }
                //myConn.Close();


                excelReader.Close();
            }
            #endregion

            foreach (var i in outList)
            {
                outTables.Add(i.Value);
            }
        }
    }

}
