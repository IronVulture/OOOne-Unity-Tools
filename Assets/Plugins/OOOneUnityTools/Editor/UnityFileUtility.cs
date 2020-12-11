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

        public static void CreateAnimationClips(string childPath, string fileName)
        {
            if (IsUnityFolderExist(childPath) == false)
                CreateUnityFolder(childPath);

            var animationClip = new AnimationClip();
            var path = $"Assets/{childPath}/{fileName}.anim";
            AssetDatabase.CreateAsset(animationClip, path);
            AssetDatabase.Refresh();
        }

        public static void CreateAnimationOverride(string childPath, string fileName)
        {
            if (IsUnityFolderExist(childPath) == false)
                CreateUnityFolder(childPath);

            var overrideController = new AnimatorOverrideController();
            var path = $"Assets/{childPath}/{fileName}.overrideController";
            AssetDatabase.CreateAsset(overrideController, path);
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

        public static bool IsUnityFolderExist(string childPath)
        {
            return CSharpFileUtility.IsFolderExist("Assets/" + childPath);
        }

    }
}