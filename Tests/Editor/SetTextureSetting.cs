using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using OOOneUnityTools.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SetTextureSetting
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CreateTestPNGFileToPathTests()
        {
            const string fileName = "apple";
            var unityFolderPath = Application.dataPath;
            var cSharpFolderPath =  unityFolderPath.Replace("/",@"\");
            var unityFullPath = "Assets/" + fileName + ".png";
            var metaFullPath = "Assets/" + fileName + ".meta";
            var cSharpFullPath = cSharpFolderPath + @"\" + fileName + ".png";
            FileUtility.CreatTestPngByPath(cSharpFolderPath, fileName);
            var texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(unityFullPath);
            Assert.IsNotNull(texture2D);
            File.Delete(unityFullPath);
            File.Delete(metaFullPath);
            AssetDatabase.Refresh();
        }



        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SetTextureSettingWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
