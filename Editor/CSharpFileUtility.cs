using System;
using System.IO;
using System.Security.Cryptography;
using Plugins.OOOneUnityTools.Editor;

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

        public static bool IsFileInPath(string folderPath, string fileName, string fileExtension)
        {
            var newFolderPath = ParseSlashToCsharp(folderPath);
            var fullPath = GetFullPath(fileName, fileExtension, newFolderPath);
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

        public static bool IsFileAreEqual(string pathA, string pathB)
        {
            string hash;
            string hash2;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(pathA))
                {
                    hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
                using (var stream = File.OpenRead(pathB))
                {
                    hash2 = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }

            return hash == hash2;
        }

        public static void CopyFile(string sourcePath, string targetPath)
        {
            File.Copy(sourcePath, targetPath);
            UnityFileUtility.RefreshAsset();
        }
    }
}