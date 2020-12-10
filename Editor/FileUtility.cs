﻿using System.IO;
using UnityEditor;
using UnityEngine;

namespace OOOneUnityTools.Editor
{
    public class FileUtility
    {
        public static void CreatTestPngByPath(string path, string fileName)
        {
            string fullPathBeforeParse = path + "/" + fileName + ".png";
            Debug.Log($"{fullPathBeforeParse}");
            string fullPathAfterParse = fullPathBeforeParse.Replace("/", @"\");
            Texture2D testTexture = new Texture2D(10, 10);
            testTexture.Apply();
            var Bytes = testTexture.EncodeToPNG();
            File.WriteAllBytes(fullPathAfterParse, Bytes);
            AssetDatabase.Refresh();
        }

        public static void DeleteFileWithMetaByPath(string folderPath, string fileName)
        {
            string pngPathBeforeParse = folderPath + "/" + fileName + ".png";
            string pngPathAfterParse = pngPathBeforeParse.Replace("/", @"\");
            string metaPathBeforeParse = folderPath + "/" + fileName + ".meta";
            string metaPathAfterParse = metaPathBeforeParse.Replace("/", @"\");
            File.Delete(pngPathAfterParse);
            File.Delete(metaPathAfterParse);
            AssetDatabase.Refresh();
        }
    }
}