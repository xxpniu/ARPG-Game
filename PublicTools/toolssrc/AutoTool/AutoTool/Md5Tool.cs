using System;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    public static class Md5Tool
    {

        
        public static string GetMd5Hash(string input)
        {
            // Convert the input string to a byte array and compute the hash.
            return GetMd5Hash(Encoding.UTF8.GetBytes(input));
        }

        public static string GetMd5Hash(byte[] input, bool upperCase = false)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(input);
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString(upperCase ? "X2" : "x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static bool VerifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(input);
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool VerifyMd5Hash(byte[] input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(input);
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public static class Ex
    {
        public static string GetTarget(this AutoTool.JsonPath.FilesItem file)
        {
            // "export/andrew"
            // "export/kathy"
            // exportitems
            //Andrew/Gilly/Kathy
            var path = file.path.Split('/');
            switch (path[0])
            {
                case "export":
                case "exportitems":
                    {
                        switch (path[1])
                        {
                            case "kathy": return "Kathy";
                            case "andrew":
                            default:
                                return "Andrew";
                               
                        }
                    }
                case "exportpet":
                    return "Gilly";
                default:
                    throw new Exception("unknow target:" + file.path);
            }
            
        }

        public static string GetFileType(this AutoTool.JsonPath.FilesItem file)
        {
            //action/model/scene/prop

            var path = file.path.Split('/');
            switch (path[0])
            {
                case "export":
                    {
                        switch (path[2])
                        {
                            case "animation": return "action";
                            case "scene": return "scene";
                            case "skin": return "model";
                            default:
                                throw new Exception("unknow type:" + file.path);

                        }
                    }
                case "exportitems": return "prop";
                case "exportpet":
                    {
                        switch (path[2])
                        {
                            case "animator":
                            case "animation":
                                return "action";
                            case "skin": return "model";
                            default:
                                throw new Exception("unknow type:" + file.path);

                        }
                    }
                default:
                    throw new Exception("unknow target:" + file.path);
            }
        }

        public static void Print(this string str, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
        }
    }
}