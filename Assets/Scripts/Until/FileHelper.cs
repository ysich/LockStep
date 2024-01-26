/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-22 16:24:44
-- 概述:
---------------------------------------------------------------------------------------*/

using System;
using System.IO;

namespace Until
{
    public static class FileHelper
    {
        public static string ReadTextFromFile(string path, string defaultValue = "")
        {
            string result = defaultValue;

            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                using (StreamReader reader = fileInfo.OpenText())
                {
                    result = reader.ReadToEnd();
                    reader.Close();
                }
            }

            return result;
        }
        public static void CreateDirectory(string path)
        {
            if (Directory.Exists(path)) return;

            Directory.CreateDirectory(path);
        }
        public static void CreateDirectoryFromFile(string path)
        {
            path = path.Replace("\\", "/");
            int index = path.LastIndexOf("/");
            string dir = path.Substring(0, index);
            CreateDirectory(dir);
        }
        
        public static void SaveTextToFile(string text, string path)
        {
            CreateDirectoryFromFile(path);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            SaveBytesToPath(path, bytes);
        }
        public static void SaveBytesToPath(string path, byte[] bytes)
        {
            CreateDirectoryFromFile(path);

            try
            {
                FileStream stream = new FileStream(path, FileMode.Create);
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
            finally
            {
            }
        }

    }
}