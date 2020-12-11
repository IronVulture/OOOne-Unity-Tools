using OOOneTools.Editor;
using UnityEditor;
using UnityEngine;

namespace Plugins.OOOneUnityTools.Editor
{
    public class UnityFileUtility
    {
        public static void CreateUnityFolder(string path)
        {
            string parentFolderPath = "Assets";
            var fullPath = $"{parentFolderPath}/{path}";
            string assetFolder = "Assets";
            fullPath = fullPath.Substring(assetFolder.Length);
            fullPath = Application.dataPath + fullPath;
            if (!CSharpFileUtility.IsFolderExist(fullPath))
            {
                AssetDatabase.CreateFolder(parentFolderPath, path);
            }
        }
    }
}