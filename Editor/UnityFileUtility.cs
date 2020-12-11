using System;
using System.IO;
using OOOneTools.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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
                RefreshAsset();
            }
        }


        public static void DeleteUnityFolder(string childPath)
        {
            AssetDatabase.DeleteAsset(GetAssetsPath(childPath));
            RefreshAsset();
        }

        public static void CreateAnimationClip(string childPath, string fileName)
        {
            if (IsUnityFolderExist(childPath) == false) CreateUnityFolder(childPath);
            CreateUnityAsset(childPath, fileName, typeof(AnimationClip), "anim");
            RefreshAsset();
        }

        public static void CreateAnimationOverride(string childPath, string fileName)
        {
            if (IsUnityFolderExist(childPath) == false) CreateUnityFolder(childPath);
            CreateUnityAsset(childPath, fileName, typeof(AnimatorOverrideController), "overrideController");
            RefreshAsset();
        }

        private static void CreateUnityAsset(string childPath, string fileName, Type type, string extension)
        {
            Object overrideController = (Object) Activator.CreateInstance(type);
            var path = GetExtensionPath(childPath, fileName, extension);
            AssetDatabase.CreateAsset(overrideController, path);
        }

        private static string GetExtensionPath(string childPath, string fileName, string extension)
        {
            var path = $"Assets/{childPath}/{fileName}.{extension}";
            return path;
        }


        private static string GetAssetsPath(string childPath) => "Assets/" + childPath;
        private static void RefreshAsset() => AssetDatabase.Refresh();

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