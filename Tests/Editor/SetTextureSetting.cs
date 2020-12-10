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
        private string fileName = "apple";
        private string unityFolderPath;
        private string cSharpFolderPath;
        private string unityFullPath;
        private string metaFullPath;
        private string cSharpFullPath;

        [SetUp]
        public void SetUp()
        {
            unityFolderPath = Application.dataPath;
            cSharpFolderPath =  unityFolderPath.Replace("/",@"\");
            unityFullPath = unityFolderPath + fileName + ".png";
            metaFullPath = unityFolderPath + fileName + ".meta";
            cSharpFullPath = cSharpFolderPath + @"\" + fileName + ".png";
        }
        // A Test behaves as an ordinary method
        [Test]
        public void CreateTestPNGFileToPathTests()
        {
            FileUtility.CreatTestPngByPath(cSharpFolderPath, fileName);
            var texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(unityFullPath);
            Assert.IsNotNull(texture2D);
            FileUtility.DeleteFileWithMetaByPath();
            File.Delete(unityFullPath);
            File.Delete(metaFullPath);
            AssetDatabase.Refresh();
        }

        [Test]
        public void DeleteTestsPngFromPathTests()
        {

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
