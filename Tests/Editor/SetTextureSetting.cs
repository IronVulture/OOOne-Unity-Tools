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
        private string fileName;
        private string folderPath;
        private string assetPath;
        private string pngPathBeforeParse;
        private string pngPathAfterParse;
        private string metaPathBeforeParse;
        private string metaPathAfterParse;


        [SetUp]
        public void SetUp()
        {
            fileName = "apple";
            folderPath = Application.dataPath;
            assetPath = "Assets/" + fileName + ".png";
            pngPathBeforeParse = folderPath + "/" + fileName + ".png";
            pngPathAfterParse = pngPathBeforeParse.Replace("/", @"\");
            metaPathBeforeParse = folderPath + "/" + fileName + ".meta";
            metaPathAfterParse = metaPathBeforeParse.Replace("/", @"\");
        }

        [Test]
        public void ParsePathUnityToCsharpTests()
        {
            var pathBeforeParse = "Assets/Plugins/OOOneUnityTools/Editor/FileUtility.cs";
            var expectedResult = @"Assets\Plugins\OOOneUnityTools\Editor\FileUtility.cs";
            var pathAfterParse = FileUtility.ParePathUnityToCsharp(pathBeforeParse);
            Assert.AreEqual(expectedResult, pathAfterParse);
        }


        [Test]
        public void DeleteTestsPngFromPathTests()
        {
            FileUtility.CreatTestPngByPath(folderPath, fileName);
            FileUtility.DeleteFileWithMetaByPath(folderPath, fileName);
            var texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            var metaFileExist = File.Exists(metaPathAfterParse);
            var pngFileExist = File.Exists(pngPathAfterParse);
            Assert.IsFalse(metaFileExist);
            Assert.IsFalse(pngFileExist);
        }


        // A Test behaves as an ordinary method
        [Test]
        public void CreateTestPNGFileToPathTests()
        {
            FileUtility.CreatTestPngByPath(folderPath, fileName);
            var texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            Assert.IsNotNull(texture2D);
            FileUtility.DeleteFileWithMetaByPath(folderPath, fileName);
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