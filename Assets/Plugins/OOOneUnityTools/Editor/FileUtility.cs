
using System.IO;
using UnityEditor;
using UnityEngine;

namespace OOOneUnityTools.Editor
{
    public class FileUtility
    {
        public static void CreatTestPngByPath(string path, string fileName)
        {
            string fullPath = path + @"\" + fileName + ".png";

            Texture2D testTexture = new Texture2D(10, 10);
            testTexture.Apply();
            var Bytes = testTexture.EncodeToPNG();
            File.WriteAllBytes(fullPath, Bytes);
            AssetDatabase.Refresh();
        }

    }
}