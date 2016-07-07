using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoParser
{
    class ParserProto
    {
        public List<ProtoStruct> Structs = new List<ProtoStruct>();
        public List<ProtoEnum> Enums = new List<ProtoEnum>();
        private ProtoStruct CurrentStruct;
        private ProtoEnum CurrentEnum;
        private HashSet<string> searchedImprot = new HashSet<string>();
        private HashSet<string> searchenums = new HashSet<string>();
        public string Package = string.Empty;
        private string protoDir = string.Empty;
        private string Comm = string.Empty;
        public void ReadLine(string line)
        {
            var code = line.Trim();

            if (code.StartsWith("{")) return;//开始
            if (code.StartsWith("/"))
            {
                Comm = code.Replace("/", "");
                return;//注释
            }
            if (code.StartsWith("}"))//结束
            {
                CurrentEnum = null;
                CurrentStruct = null;
                return;
            }
            

            #region Enum
            if (CurrentEnum != null)
            {
                int indexEqual = code.IndexOf("=");
                string enumFileName, enumFileValue, enumFieldNote;
                if (indexEqual > -1)
                {
                    enumFileName = code.Split('=')[0].Trim();
                    enumFileValue = code.Substring(indexEqual + 1).Split("//".ToArray())[0].Trim("\t ;".ToArray());
                    var tempNotes = code.Split("//".ToArray());
                    var tempSb = new StringBuilder();
                    for (var i = 1; i < tempNotes.Length; i++)
                    {
                        tempSb.Append(tempNotes[i].Trim("\t".ToArray()));
                    }
                    enumFieldNote = tempSb.ToString();
                    CurrentEnum.AddField(new ProtoEnumField { Name = enumFileName, Value = enumFileValue, Note = enumFieldNote });
                }
            }
            #endregion

            var args = code.Split(" \t\\;".ToArray());
            //Console.WriteLine(code);
            var notes = code.Split("//".ToArray());
            var tempFieldSb = new StringBuilder();
            for (var i = 1; i < notes.Length; i++)
                tempFieldSb.Append(notes[i].Trim());
            //Console.WriteLine(tempFieldSb.ToString());
            switch (args[0])
            {
                case "package":
                    Package = args[1].Trim();
                    break;
                case "import":
                    var importFile = args[1].Trim("\'\" \t\\;".ToArray());
                    LoadEnumTypes(importFile);
                    break;
                case "message":
                    CurrentStruct = new ProtoStruct(args[1], Comm);
                    Structs.Add(CurrentStruct);
                    break;
                case "enum":
                    CurrentEnum = new ProtoEnum(args[1],  Comm);
                    Enums.Add(CurrentEnum);
                    break;
                case "optional":
                case "required":
                case "repeated":
                    var fieldType = args[1];
                    var fieldName = args[2].Split("\t\\;".ToArray())[0].Trim();
                    var fieldNote = string.Empty;
                    fieldNote = tempFieldSb.ToString();
                    if (CurrentStruct == null) throw new Exception("解析错误:" + code);
                    CurrentStruct.AddField(new ProtoField
                    {
                        IsArray = args[0] == "repeated",
                        Name = fieldName,
                        Type = fieldType,
                        Note = fieldNote
                    });
                    break;
            }
            Comm = string.Empty;
        }
        private void LoadEnumTypes(string importFile)
        {
            var importName = importFile.Split('.')[0];
            if (searchedImprot.Contains(importName)) return;
            searchedImprot.Add(importName);

            var path = Path.Combine(protoDir, importFile);
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var content = line.Trim();
                    var args = content.Split(' ');
                    if (args.Length < 2) continue;
                    switch (args[0])
                    {
                        case "import":
                            var fileName = args[1].Trim(" \t;\"\'".ToArray());
                            LoadEnumTypes(fileName);
                            break;
                        case "enum":
                            if (!searchenums.Contains(args[1]))
                                searchenums.Add(args[1]);
                            break;
                    }
                }
            }
            //EnumTypes
        }

        public ParserProto(string dir)
        {
            protoDir = dir;
        }

        public void CompieFile(string file)
        {
            var path = Path.Combine(protoDir, file);
            var name = Path.GetFileName(path);
            LoadEnumTypes(name);
            using (var reader = new StreamReader(path,Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    ReadLine(reader.ReadLine());
                }
            }


        }

        public void TOcsFile(string filePath)
        {
            var usinghead = 
@"/************************************************/
//本代码自动生成，切勿手动修改
/************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace [NAMESPACE]
{
[CODES]}";
            var sb = new StringBuilder();
            foreach(var i in Enums)
            {
                sb.AppendLine(GenEnumCsCode(i));
            }
            foreach(var i in Structs)
            {
                sb.AppendLine(GenStructCsCode(i));
            }

            if (string.IsNullOrEmpty(Package))
                Package = "Proto";

            var result = usinghead.Replace("[NAMESPACE]",Package).Replace("[CODES]", sb.ToString());
            File.WriteAllText(Path.Combine(protoDir, filePath), result);
        }



        private string GenEnumCsCode(ProtoEnum en)
        {
            var template =
                @"   /// <summary>
    /// [NODE]
    /// </summary>
    public enum [NAME]
    {
[FIELDS]
    }";
            var sb = new StringBuilder();
            foreach(var i in en.Fields)
            {
                var templateField = @"        /// <summary>
        /// [NOTE]
        /// </summary>
        [FIELD]";
                templateField = templateField.Replace("[NOTE]", i.Note);
                if(!string.IsNullOrEmpty(i.Value))
                {
                    templateField=templateField.Replace("[FIELD]", string.Format("{0}={1},", i.Name, i.Value));
                }
                else
                {
                    templateField = templateField.Replace("[FIELD]", string.Format("{0},", i.Name));
                }
                sb.AppendLine(templateField);
            }

            return template.Replace("[NAME]", en.Name).Replace("[FIELDS]",sb.ToString()).Replace("[NODE]",en.Node);
        }
        private string GenStructCsCode(ProtoStruct cstruct)
        {
            
            #region template
            var template =
@"    /// <summary>
    /// [NODE]
    /// </summary>
    public class [NAME] : Proto.ISerializerable
    {
        public [NAME]()
        {
			[INITS]
        }
[FIELDS]
        public void ParseFormBinary(BinaryReader reader)
        {
[BRS]             
        }

        public void ToBinary(BinaryWriter writer)
        {
[BWS]            
        }

    }";
            #endregion

            var structCode = template.Replace("[NAME]", cstruct.Name);
            var sbFields = new StringBuilder();
            var sbInits = new StringBuilder();
            var sbBr = new StringBuilder();
            var sbBw = new StringBuilder();

            foreach(var i in cstruct.Fields)
            {
                sbFields.AppendLine(GenFileCsCode(i));
                if(i.IsArray)
                {
                    sbInits.AppendLine(
@"            [NAME] = new List<[TYPE]>();"
                        .Replace("[NAME]", i.Name).Replace("[TYPE]", i.Type));
                }
                else
                {
                    switch(i.Type)
                    {
                        case ProtoTypes.STRING:
                            sbInits.AppendLine("            [NAME] = string.Empty;".Replace("[NAME]", i.Name));
                            break;
                        case ProtoTypes.INT_32:
                        case ProtoTypes.BOOL:
                        case ProtoTypes.BYTE:
                        case ProtoTypes.CHAR:
                        case ProtoTypes.FLOAT:
                        case ProtoTypes.SHORT:
                        case ProtoTypes.INT_64:
                            break;
                        default:
                            if (!searchenums.Contains(i.Type))
                            {
                                 sbInits.AppendLine((@"[NAME] = new [TYPE]();".Replace("[NAME]", i.Name)
                                    .Replace("[TYPE]", i.Type)));
                            }
                            break;
                    }
                }
                sbBr.AppendLine(GenFieldBrCode(i));
                sbBw.AppendLine(GenFieldBwCode(i));
            }
            return structCode.Replace("[INITS]", sbInits.ToString())
                .Replace("[FIELDS]",sbFields.ToString())
                .Replace("[BRS]",sbBr.ToString())
                .Replace("[BWS]",sbBw.ToString())
                .Replace("[NODE]",cstruct.Node);
        }

        #region genread
        public string GenFieldBrCode(ProtoField field )
        {
            if (field.IsArray)
            {
                var arrayTemp = @"            int [NAME]_Len = reader.ReadInt32();
            while([NAME]_Len-->0)
            {
[DEFINE]
[READER]
                [NAME].Add([NAME]_Temp );
            }".Replace("[NAME]",field.Name);
                var type = field.Type;
                var name = "[NAME]_Temp".Replace("[NAME]", field.Name);
                arrayTemp = arrayTemp.Replace("[DEFINE]", "                " + DefineField(name, field.Type));
                arrayTemp = arrayTemp.Replace("[READER]", "                " + ReadField(name, type));
                return arrayTemp;
            }
            else
            {
                return "            " + ReadField(field.Name, field.Type);
            }
        }
        private string DefineField(string name, string type)
        {
            switch (type)
            {
                case ProtoTypes.INT_32:
                    return "int [NAME] = 0;".Replace("[NAME]", name);
                case ProtoTypes.BOOL:
                    return "bool [NAME] = flase;".Replace("[NAME]", name);
                case ProtoTypes.BYTE:
                    return "byte [NAME];".Replace("[NAME]", name);
                case ProtoTypes.CHAR:
                    return "char [NAME];".Replace("[NAME]", name);
                case ProtoTypes.FLOAT:
                    return "float [NAME] = 0f;".Replace("[NAME]", name);
                case ProtoTypes.SHORT:
                    return "short [NAME] = 0;".Replace("[NAME]", name);
                case ProtoTypes.INT_64:
                    return "long [NAME] = 0;".Replace("[NAME]", name);
                case ProtoTypes.STRING:
                    return @"string [NAME] = string.Empty;"
                        .Replace("[NAME]", name);
                default:
                    if (searchenums.Contains(type))
                    {
                        //Enum
                        return "[TYPE] [NAME];".Replace("[NAME]", name)
                            .Replace("[TYPE]", type);
                    }
                    else
                    {
                        return @"[TYPE] [NAME] = new [TYPE]();".Replace("[NAME]", name)
                            .Replace("[TYPE]", type);
                    }
            }
        }
        private string ReadField(string name, string type)
        {
            switch (type)
            {
                case ProtoTypes.INT_32:
                    return "[NAME] = reader.ReadInt32();".Replace("[NAME]", name);
                case ProtoTypes.BOOL:
                    return "[NAME] = reader.ReadBoolean();".Replace("[NAME]", name);
                case ProtoTypes.BYTE:
                    return "[NAME] = reader.ReadByte();".Replace("[NAME]", name);
                case ProtoTypes.CHAR:
                    return "[NAME] = reader.ReadChar();".Replace("[NAME]", name);
                case ProtoTypes.FLOAT:
                    return "[NAME] = reader.ReadSingle();".Replace("[NAME]", name);
                case ProtoTypes.SHORT:
                    return "[NAME] = reader.ReadInt16();".Replace("[NAME]", name);
                case ProtoTypes.INT_64:
                    return "[NAME] = reader.ReadInt64();".Replace("[NAME]", name);
                case ProtoTypes.STRING:
                    return @"[NAME] = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));"
                        .Replace("[NAME]", name);
                default:
                    if (searchenums.Contains(type))
                    {
                        //Enum
                        return "[NAME] = ([TYPE])reader.ReadInt32();".Replace("[NAME]", name)
                            .Replace("[TYPE]", type);
                    }
                    else
                    {
                        return @"[NAME] = new [TYPE]();[NAME].ParseFormBinary(reader);".Replace("[NAME]", name)
                            .Replace("[TYPE]", type);
                    }
            }
        }
        #endregion

        #region genwrite

        private string GenFieldBwCode(ProtoField field )
        {
            if(field.IsArray)
            {
                var arrayTemplate = @"            writer.Write([NAME].Count);
            foreach(var i in [NAME])
            {
[CODE]               
            }".Replace("[NAME]",field.Name);
                return arrayTemplate.Replace("[CODE]", "                " + WriteField("i", field.Type));
            }
            else
            {

                return "            " + WriteField(field.Name, field.Type);
            }
        }
        private string WriteField(string name, string type)
        {
            switch (type)
            {
                case ProtoTypes.INT_32:
                case ProtoTypes.BOOL:
                case ProtoTypes.BYTE:
                case ProtoTypes.CHAR:
                case ProtoTypes.FLOAT:
                case ProtoTypes.SHORT:
                case ProtoTypes.INT_64:
                    return "writer.Write([NAME]);".Replace("[NAME]", name);
                case ProtoTypes.STRING:
                    return @"var [NAME]_bytes = Encoding.UTF8.GetBytes([NAME]);writer.Write([NAME]_bytes.Length);writer.Write([NAME]_bytes);"
                        .Replace("[NAME]", name);
                default:
                    if (searchenums.Contains(type))
                    {
                        //Enum
                        return "writer.Write((int)[NAME]);".Replace("[NAME]", name);
                    }
                    else
                    {
                        return @"[NAME].ToBinary(writer);".Replace("[NAME]", name);
                    }
            }
        }
        #endregion
        private string GenFileCsCode(ProtoField field)
        {
            var arraryTemplate = @"        /// <summary>
        /// [NOTE]
        /// </summary>
        public List<[TYPE]> [NAME] { set; get; }";
            string template = @"        /// <summary>
        /// [NOTE]
        /// </summary>
        public [TYPE] [NAME] { set; get; }";
            var resultType = string.Empty;
            switch (field.Type)
            {
                case ProtoTypes.STRING:
                    resultType = "string";
                    break;
                case ProtoTypes.INT_32:
                     resultType = "int";
                    break;
                case ProtoTypes.BOOL:
                     resultType = "bool";
                    break;
                case ProtoTypes.BYTE:
                     resultType = "byte";
                    break;
                case ProtoTypes.CHAR:
                     resultType = "char";
                    break;
                case ProtoTypes.FLOAT:
                     resultType = "float";
                    break;
                case ProtoTypes.SHORT:
                     resultType = "short";
                    break;
                case ProtoTypes.INT_64:
                     resultType = "long";
                    break;
                default:
                    resultType = field.Type;
                    break;
            }
            var resultTemplate = field.IsArray ? arraryTemplate : template;
            var code = resultTemplate.Replace("[NOTE]", field.Note)
                .Replace("[TYPE]", resultType).Replace("[NAME]", field.Name);
            return code;
        }
    }
}
