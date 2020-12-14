using System;
using System.Collections.Generic;
using System.IO;
using OOOneTools.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.OOOneUnityTools.Editor
{
    public class UnityFileUtility
    {
        #region Public Variables

        public enum FileType
        {
            AnimatorOverride,
        }

        #endregion

        #region Private Variables

        private static readonly Dictionary<FileType, string> FileExtension = new Dictionary<FileType, string>()
        {
            {FileType.AnimatorOverride, "overrideController"},
        };

        private static string UnityAbsolutePath = Application.dataPath;

        #endregion

        #region Public Methods

        public static void CreateAssetFile(FileType fileType, string childPath, string fileName)
        {
            var fileNotExist = IsFileInPath(GetUnityFullPath(childPath), fileName, fileType) == false;
            if (fileNotExist)
            {
                if (fileType == FileType.AnimatorOverride)
                    CreateUnityAsset(childPath, fileName, typeof(AnimatorOverrideController), GetExtension(fileType));

                RefreshAsset();
            }
        }

        public static void CreatePng(string childPath, string fileName)
        {
            if (IsUnityFolderExist(childPath) == false) CreateUnityFolder(childPath);
            var path = GetFullPath(childPath) + @"\" + fileName + ".png";
            var texture2D = Texture2D.blackTexture;
            byte[] bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
            RefreshAsset();
        }

        public static void CreateUnityAsset(string childPath, string fileName, Type type, string extension)
        {
            Object instance = (Object) Activator.CreateInstance(type);
            var path = GetExtensionPath(childPath, fileName, extension);
            AssetDatabase.CreateAsset(instance, path);
        }

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

        public static string GetAbsolutePath()
        {
            return UnityAbsolutePath;
        }

        public static string GetUnityAbsoluteFolderPath(string childPath)
        {
            return GetAbsolutePath() + "/" + childPath;
        }

        public static string GetUnityFolderPath(string childPath)
        {
            return CombineUnityPath(GetUnityPath(), childPath);
        }

        public static string GetUnityFullPath(string childPath) => $@"{Application.dataPath}\{childPath}";

        public static string GetUnityFullPath(string childPath, string fileName, string extension)
        {
            return CombineUnityFullPath(childPath, fileName, extension);
        }

        public static string GetUnityPath()
        {
            return UnityPath;
        }

        public static bool IsFileInPath(string unityFullFolderPath, string fileName, FileType fileType)
        {
            var slashToCsharp = CSharpFileUtility.ParseSlashToCsharp(unityFullFolderPath);
            return CSharpFileUtility.IsFileInPath(slashToCsharp, fileName, GetExtension(fileType));
        }

        public static bool IsUnityFolderExist(string childPath)
        {
            return CSharpFileUtility.IsFolderExist("Assets/" + childPath);
        }

        public static bool TryCreateAnimationClip(string childPath, string fileName)
        {
            if (IsUnityFolderExist(childPath) == false) CreateUnityFolder(childPath);
            var csharpFolderPath = ParseChildPathToCsharpFolderPath(childPath);
            if (CSharpFileUtility.IsFileInPath(csharpFolderPath, fileName, "anim"))
                return false;
            CreateUnityAsset(childPath, fileName, typeof(AnimationClip), "anim");
            RefreshAsset();
            return true;
        }

        public static bool TryCreateAnimatorOverride(string childPath, string fileName)
        {
            if (IsUnityFolderExist(childPath) == false) CreateUnityFolder(childPath);
            var csharpFolderPath = ParseChildPathToCsharpFolderPath(childPath);
            if (CSharpFileUtility.IsFileInPath(csharpFolderPath, fileName, "overrideController"))
                return false;
            CreateUnityAsset(childPath, fileName, typeof(AnimatorOverrideController), "overrideController");
            RefreshAsset();
            return true;
        }

        public static bool TryCreatePng(string childPath, string fileName)
        {
            var csharpFolderPath = ParseChildPathToCsharpFolderPath(childPath);
            var isFileInPath = CSharpFileUtility.IsFileInPath(csharpFolderPath, fileName, "png");
            var createPng = !isFileInPath;
            if (createPng) CreatePng(childPath, fileName);
            return createPng;
        }

        #endregion

        #region Private Methods

        private static string CombineUnityFullPath(string childPath, string fileName, string extension)
        {
            var path = CombineUnityPath(GetUnityFolderPath(childPath), fileName);
            var unityFullPath = $"{path}.{extension}";
            return unityFullPath;
        }

        private static string CombineUnityPath(string basePath, string childPath)
        {
            return basePath + "/" + childPath;
        }

        private static string GetAssetsPath(string childPath) => "Assets/" + childPath;

        private static string GetExtension(FileType fileType)
        {
            if (FileExtension.ContainsKey(fileType))
                return FileExtension[fileType];

            return "";
        }

        private static string GetExtensionPath(string childPath, string fileName, string extension)
        {
            var path = $"Assets/{childPath}/{fileName}.{extension}";
            return path;
        }

        private static string GetFullPath(string path)
        {
            var fullPath = $"{Application.dataPath}/{path}";
            return fullPath;
        }

        private static string ParseChildPathToCsharpFolderPath(string childPath)
        {
            return CSharpFileUtility.ParseSlashToCsharp(Application.dataPath + "/" + childPath);
        }

        private static void RefreshAsset() => AssetDatabase.Refresh();

        #endregion

        private const string UnityPath = "Assets";
    }
}