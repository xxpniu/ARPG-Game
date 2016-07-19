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

namespace ExcelOut
	{
	class Program
		{
		static void Main(string[] args)
			{
			//args

			//args =@"dir:../ExcelConfig namespace:ExcelConfig exportJson:../client/Assets/Resources/Json exportCs:../src/NetSources/Config.cs ex:*.xlsx".Split(' ');

						 // dir: namespace: class: exportJson: exportCs: ex:.xls
			string dir = string.Empty;
			string nameSpace = string.Empty;
			string exportJson = string.Empty;
			string exportCs = string.Empty;
			string ex = "*.xlsx";
			string mode = "needcode";
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
				}
			#endregion

			Console.WriteLine(string.Format("dir:{0} nameSpace:{1}  exportJson:{2} exportCs:{3} ex:{4} mode:{5}",
			dir, nameSpace, exportJson, exportCs, ex, mode
			));

			Process(exportCs, dir, ex, exportJson, nameSpace, mode);
			}

		public static void Process(string exportCs, string dir, string find, string jsonDir, string namesp, string mode)
			{

			if (string.IsNullOrEmpty(exportCs) || string.IsNullOrEmpty(dir)) return;

			var files = System.IO.Directory.GetFiles(dir, find, System.IO.SearchOption.TopDirectoryOnly);
			var outTables = new List<ExcelTable>();


			ReadTables(outTables, files, jsonDir);
			var file = Libs.EX.ExcelTool.GetAllClass(outTables, namesp);
			if (mode == "needcode")
				System.IO.File.WriteAllText(exportCs, file);
			Console.WriteLine("导出成功！！");
			}


		private static void ReadTablesUseOLEDB(List<ExcelTable> outTables, string[] files, string jsonDir)
			{
			#region readTable and Data
			foreach (var filename in files)
				{
				Console.WriteLine("gen:" + filename);
				string strCon = string.Format(
				"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=NO;IMEX=1;'",
				filename);
				var bastTableName = "__Base$";
				//__Base$
				OleDbConnection myConn = new OleDbConnection(strCon);
				string strCom = string.Format(" SELECT * FROM [{0}]", bastTableName);
				myConn.Open();
				OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
				var ds = new DataSet();
				myCommand.Fill(ds);

				var exTables = new List<ExcelTable>();

				#region Table
				for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
					var table = new Libs.EX.ExcelTable();
					var row = ds.Tables[0].Rows[i];

					table.TableName = row[0].ToString();
					table.ClassName = row[1].ToString();
					table.FileName = row[2].ToString();
					table.Description = row[3].ToString();

					exTables.Add(table);
					}
				#endregion

				foreach (var table in exTables)
					{
					var selectStr = string.Format("SELECT * FROM [{0}$]", table.TableName);
					myCommand = new OleDbDataAdapter(selectStr, myConn);
					ds = new DataSet();
					myCommand.Fill(ds);
					#region Read col
					var dataTable = new DataTable();
					for (var col = 1; col < ds.Tables[0].Columns.Count; col++)
						{
						try
							{

							var colnum = new Libs.EX.ExcelTableCol();
							colnum.ColIndex = col - 1;
							colnum.Comment = ds.Tables[0].Rows[2][col].ToString();
							colnum.Name = ds.Tables[0].Rows[0][col].ToString();
							colnum.Type = ds.Tables[0].Rows[1][col].ToString();
							if (string.IsNullOrEmpty(colnum.Name)) continue;
							table.Cols.Add(colnum);
							var coln = ds.Tables[0].Columns[col];

							dataTable.Columns.Add(new DataColumn
								{
								AllowDBNull = true,
								ColumnName = coln.ColumnName
								});
							}
						catch (Exception ex)
							{
							Console.WriteLine("错误了！", "一般出现这个情况都是因为存在影藏列！" + ex.ToString());
							return
							;
							}
						}
					#endregion
					#region Read row
					for (var i = 3; i < ds.Tables[0].Rows.Count; i++)
						{
						if (ds.Tables[0].Rows[i][1] == null) continue;
						var row = dataTable.NewRow();
						for (var col = 1; col < ds.Tables[0].Columns.Count && col < dataTable.Columns.Count + 1; col++)
							{
							row[col - 1] = ds.Tables[0].Rows[i][col];
							}

						dataTable.Rows.Add(row);
						}
					#endregion
					#region Save Json File
					var json = Libs.EX.ExcelTool.GetExcelData(table, dataTable);
					System.IO.File.WriteAllText(System.IO.Path.Combine(jsonDir, table.FileName), json);
					#endregion
					outTables.Add(table);
					}
				myConn.Close();

				}
			#endregion
			}

		private static void ReadTables(List<ExcelTable> outTables, string[] files, string jsonDir)
			{
			#region readTable and Data
			foreach (var filePath in files)
				{
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
				for (var i = 0; i < _base.Rows.Count; i++)
					{
					var table = new Libs.EX.ExcelTable();
					var row = _base.Rows[i];

					table.TableName = row[0].ToString();
					table.ClassName = row[1].ToString();
					table.FileName = row[2].ToString();
					table.Description = row[3].ToString();

					exTables.Add(table);
					}
				#endregion
				foreach (var table in exTables)
					{
					#region Read col
					var data = result.Tables[table.TableName];
					var dataTable = new DataTable();
					for (var col = 1; col < data.Columns.Count; col++)
						{
						try
							{

							var colnum = new Libs.EX.ExcelTableCol();
							colnum.ColIndex = col - 1;
							colnum.Comment = (string)data.Rows[2][col];
							colnum.Name = data.Rows[0][col].ToString();
							colnum.Type = data.Rows[1][col].ToString();
							if (string.IsNullOrEmpty(colnum.Name)) continue;
							table.Cols.Add(colnum);
							var coln = data.Columns[col];

							dataTable.Columns.Add(new DataColumn
								{
								AllowDBNull = true,
								ColumnName = coln.ColumnName
								});
							}
						catch (Exception ex)
							{
							Console.WriteLine("错误了！", "一般出现这个情况都是因为存在影藏列！" + ex.ToString());
							return
							;
							}
						}
					#endregion
					#region Read row
					for (var i = 3; i < data.Rows.Count; i++)
						{
						if (data.Rows[i][1] == null) continue;
						var row = dataTable.NewRow();
						for (var col = 1; col < data.Columns.Count && col < dataTable.Columns.Count + 1; col++)
							{
							row[col - 1] = data.Rows[i][col];
							}

						dataTable.Rows.Add(row);
						}
					#endregion
					#region Save Json File
					var json = Libs.EX.ExcelTool.GetExcelData(table, dataTable);
					System.IO.File.WriteAllText(System.IO.Path.Combine(jsonDir, table.FileName), json);
					#endregion
					outTables.Add(table);
					}
				//myConn.Close();


				excelReader.Close();
				}
			#endregion
			}
		}

	}
