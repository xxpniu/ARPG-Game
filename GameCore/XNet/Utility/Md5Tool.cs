using System;
using System.Security.Cryptography;
using System.Text;

namespace XNet.Libs.Utility
{
    /// <summary>
    /// Md5 tool
    /// </summary>
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
}