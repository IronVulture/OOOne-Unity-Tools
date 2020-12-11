using System;
using System.IO;
using OOOneTools.Editor;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace OOOne.Tools.Editor
{
    public class FileUtility
    {
        
        public static void CreatTestPngByPath(string path, string fileName)
        {
            var fullPathBeforeParse = path + "/" + fileName + ".png";
            var fullPathAfterParse = ParsePathUnityToCsharp(fullPathBeforeParse);
            var testTexture = new Texture2D(10, 10);
            testTexture.Apply();
            var Bytes = testTexture.EncodeToPNG();
            File.WriteAllBytes(fullPathAfterParse, Bytes);
            AssetDatabase.Refresh();
        }

        public static void DeleteFileWithMetaByPath(string folderPath, string fileName, string fileExtension)
        {
            var pngPathBeforeParse = folderPath + "/" + fileName + fileExtension;
            var pngPathAfterParse = ParsePathUnityToCsharp(pngPathBeforeParse);
            var metaPathBeforeParse = folderPath + "/" + fileName + ".meta";
            var metaPathAfterParse = ParsePathUnityToCsharp(metaPathBeforeParse);
            File.Delete(pngPathAfterParse);
            File.Delete(metaPathAfterParse);
            AssetDatabase.Refresh();
        }

        public static string ParsePathUnityToCsharp(string pathBeforeParse)
        {
            var pathAfterParse = pathBeforeParse.Replace("/", @"\");
            return pathAfterParse;
        }

        public static void CreateTestTexturePreset(string presetPath, string presetFileName)
        {
            const string presetFileExtension = ".preset";
            var testTexture2D = new Texture2D(10, 10);
            var preset = new Preset(testTexture2D);
            AssetDatabase.CreateAsset(preset, presetPath + presetFileName + presetFileExtension);
        }
         public bool TryGetAllOverrideControllerByFolder(string                           folderPath ,
                                                        out AnimatorOverrideController[] animatorOverrideControllers)
        {
            var folderPathHandler = new FolderPathHandler();
            var isFolderExist     = folderPathHandler.IsFolderExist(folderPath);
            var objects           = TryGetAsset<AnimatorOverrideController>(folderPath);
            // var preset              = objects as AnimatorOverrideController[];
            animatorOverrideControllers = new AnimatorOverrideController[3];
            return isFolderExist;
        }
        // https://forum.unity.com/threads/loadallassetsatpath-not-working-or-im-using-it-wrong.110326/
        /// <summary>
        /// Tries the get an asset.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="optionalName">Optional name: if no name provided returns the first asset.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T TryGetAsset<T>(string optionalName = "") where T : UnityEngine.Object
        {
            // Gets all files with the Directory System.IO class
            string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
            T        asset = null;

            // move through all files
            foreach (var file in files)
            {
                // use the GetRightPartOfPath utility method to cut the path so it looks like this: Assets/folderblah
                string path = GetRightPartOfPath(file, "Assets");

                // Then I try and load the asset at the current path.
                asset = AssetDatabase.LoadAssetAtPath<T>(path);

                // check the asset to see if it's not null
                if (asset)
                {
                    // if the optional name is nothing then we skip this step
                    if (optionalName != "")
                    {
                        if (asset.name == optionalName)
                        {

                            Debug.Log("Found the database at path: " + path + "with name: " + asset.name);
                            break;
                        }
                    }
                    else
                    {
                        Debug.Log("Found the database at path: " + path);
                        break;
                    }
                }

            }

            return asset;
        }
        private static string GetRightPartOfPath(string path, string after)
        {
            var parts      = path.Split(Path.DirectorySeparatorChar);
            int afterIndex = Array.IndexOf(parts, after);

            if (afterIndex == -1)
            {
                return null;
            }

            return string.Join(Path.DirectorySeparatorChar.ToString(),
                               parts, afterIndex, parts.Length - afterIndex);
        }
    }
}