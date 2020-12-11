using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace OOOne.Tools.Editor
{
    public static class FileUtility
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
    }
}