using System;
using System.IO;

namespace OOOneTools.Editor
{
    public class CSharpFileUtility
    {
        public static bool IsFolderExist(string folderPath)
        {
            string newFolderPath = ParseSlashToCsharp(folderPath);
            return Directory.Exists(newFolderPath);
        }

        public static string ParseSlashToCsharp(string beforeParse)
        {
            return beforeParse.Replace("/", @"\");
        }

        public static bool IsFileInPath(string folderPath, string fileName, string FileExtension)
        {
            var newFolderPath = ParseSlashToCsharp(folderPath);
            var fullPath = GetFullPath(fileName, FileExtension, newFolderPath);
            var fileExists = File.Exists(fullPath);
            return fileExists;
        }

        public static string GetFullPath(string fileName, string FileExtension, string newFolderPath)
        {
            var fullPath = newFolderPath + @"\" + fileName + "." + FileExtension;
            return fullPath;
        }
    }
}