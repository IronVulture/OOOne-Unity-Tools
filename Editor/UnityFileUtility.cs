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
            AnimationClip,
            Png
        }

        #endregion

        #region Private Variables

        private static readonly Dictionary<FileType, string> FileExtension = new Dictionary<FileType, string>()
        {
            {FileType.AnimatorOverride, "overrideController"},
            {FileType.AnimationClip, "anim"},
            {FileType.Png, "png"},
        };

        #endregion

        #region Public Methods

        public static bool CreateAssetFile(FileType fileType, string childPath, string fileName)
        {
            var fileNotExist = IsFileInPath(UnityPathUtility.GetUnityFullPath(childPath), fileName, fileType) == false;
            if (fileNotExist)
            {
                if (IsUnityFolderExist(childPath) == false) CreateUnityFolder(childPath);
                switch (fileType)
                {
                    case FileType.AnimatorOverride:
                        CreateUnityAsset(childPath, fileName, typeof(AnimatorOverrideController), GetExtension(fileType));
                        break;
                    case FileType.AnimationClip:
                        CreateUnityAsset(childPath, fileName, typeof(AnimationClip), GetExtension(fileType));
                        break;
                    case FileType.Png:
                        CreatePng(childPath, fileName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
                }

                RefreshAsset();
            }

            return fileNotExist;
        }

        public static void CreatePng(string childPath, string fileName)
        {
            var path = UnityPathUtility.GetUnityAbsoluteFullPath(childPath, fileName, GetExtension(FileType.Png));
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
            RefreshAsset();
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

        #endregion

        #region Private Methods

        public static string GetExtension(FileType fileType)
        {
            if (FileExtension.ContainsKey(fileType))
                return FileExtension[fileType];

            return "";
        }

        private static void RefreshAsset() => AssetDatabase.Refresh();

        #endregion
    }
}