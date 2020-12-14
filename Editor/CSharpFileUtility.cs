using System.IO;

namespace OOOneTools.Editor
{
    public class CSharpFileUtility
    {
        #region Public Methods

        public static string GetFullPath(string fileName, string FileExtension, string newFolderPath)
        {
            var fullPath = newFolderPath + @"\" + fileName + "." + FileExtension;
            return fullPath;
        }

        public static bool IsFileInPath(string folderPath, string fileName, string FileExtension)
        {
            var newFolderPath = ParseSlashToCsharp(folderPath);
            var fullPath = GetFullPath(fileName, FileExtension, newFolderPath);
            var fileExists = File.Exists(fullPath);
            return fileExists;
        }

        public static bool IsFolderExist(string folderPath)
        {
            var newFolderPath = ParseSlashToCsharp(folderPath);
            return Directory.Exists(newFolderPath);
        }

        public static string ParseSlashToCsharp(string beforeParse)
        {
            return beforeParse.Replace("/", @"\");
        }

        #endregion
    }
}