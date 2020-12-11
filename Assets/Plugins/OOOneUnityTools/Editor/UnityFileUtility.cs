using OOOneTools.Editor;
using UnityEditor;
using UnityEngine;

namespace Plugins.OOOneUnityTools.Editor
{
    public class UnityFileUtility
    {
        public static void CreateUnityFolder(string parentFolderPath, string childPath)
        {
            var fullPath = $"{parentFolderPath}/{childPath}";
            string assetFolder = "Assets";
            fullPath = fullPath.Substring(assetFolder.Length);
            fullPath = Application.dataPath + fullPath;
            if (!CSharpFileUtility.IsFolderExist(fullPath))
            {
                AssetDatabase.CreateFolder(parentFolderPath, childPath);
            }
        }
    }
}