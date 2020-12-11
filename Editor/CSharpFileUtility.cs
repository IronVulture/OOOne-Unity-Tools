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
    }
}