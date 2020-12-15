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

        #endregion

        #region Public Methods

        public static void CreateAssetFile(FileType fileType, string childPath, string fileName)
        {
            var fileNotExist = IsFileInPath(UnityPathUtility.GetUnityFullPath(childPath), fileName, fileType) == false;
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
            var path = UnityPathUtility.GetUnityAbsoluteFolderPath(childPath) + @"\" + fileName + ".png";
            var texture2D = Texture2D.blackTexture;
            byte[] bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
            RefreshAsset();
        }

        public static void CreateUnityAsset(string childPath, string fileName, Type type, string extension)
        {
            Object instance = (Object) Activator.CreateInstance(type);
            var path = UnityPathUtility.GetUnityFullPath(childPath, fileName, extension);
            AssetDatabase.CreateAsset(instance, path);
        }

        public static void CreateUnityFolder(string childPath)
        {
            var fullPath = UnityPathUtility.GetUnityAbsoluteFolderPath(childPath);
            if (CSharpFileUtility.IsFolderExist(fullPath) == false)
            {
                Directory.CreateDirectory(fullPath);
                RefreshAsset();
            }
        }


        public static void DeleteUnityFolder(string childPath)
        {
            AssetDatabase.DeleteAsset(UnityPathUtility.GetUnityFolderPath(childPath));
            RefreshAsset();
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
            var csharpFolderPath = CsharpPathUtility.GetCsharpUnityAbsoluteFolderPath(childPath);
            if (CSharpFileUtility.IsFileInPath(csharpFolderPath, fileName, "anim"))
                return false;
            CreateUnityAsset(childPath, fileName, typeof(AnimationClip), "anim");
            RefreshAsset();
            return true;
        }

        public static bool TryCreateAnimatorOverride(string childPath, string fileName)
        {
            if (IsUnityFolderExist(childPath) == false) CreateUnityFolder(childPath);
            var csharpFolderPath = CsharpPathUtility.GetCsharpUnityAbsoluteFolderPath(childPath);
            if (CSharpFileUtility.IsFileInPath(csharpFolderPath, fileName, "overrideController"))
                return false;
            CreateUnityAsset(childPath, fileName, typeof(AnimatorOverrideController), "overrideController");
            RefreshAsset();
            return true;
        }

        public static bool TryCreatePng(string childPath, string fileName)
        {
            var csharpFolderPath = CsharpPathUtility.GetCsharpUnityAbsoluteFolderPath(childPath);
            var isFileInPath = CSharpFileUtility.IsFileInPath(csharpFolderPath, fileName, "png");
            var createPng = !isFileInPath;
            if (createPng) CreatePng(childPath, fileName);
            return createPng;
        }

        #endregion

        #region Private Methods

        private static string GetExtension(FileType fileType)
        {
            if (FileExtension.ContainsKey(fileType))
                return FileExtension[fileType];

            return "";
        }

        private static void RefreshAsset() => AssetDatabase.Refresh();

        #endregion
    }
}