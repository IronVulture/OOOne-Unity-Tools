using System.IO;
using OOOneTools.Editor;
using UnityEditor;
using UnityEngine;

namespace Plugins.OOOneUnityTools.Editor
{
    public class UnityFileUtility
    {
        public static void CreateUnityFolder(string path)
        {
            var fullPath = GetFullPath(path);
            if (CSharpFileUtility.IsFolderExist(fullPath) == false)
            {
                Directory.CreateDirectory(fullPath);
                AssetDatabase.Refresh();
            }
        }


        public static void DeleteUnityFolder(string childPath)
        {
            AssetDatabase.DeleteAsset(GetAssetsPath(childPath));
            AssetDatabase.Refresh();
        }

        private static string GetAssetsPath(string childPath)
        {
            return "Assets/" + childPath;
        }

        private static string GetFullPath(string path)
        {
            var fullPath = $"{Application.dataPath}/{path}";
            return fullPath;
        }
    }
}