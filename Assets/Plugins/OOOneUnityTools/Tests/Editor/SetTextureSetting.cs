using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using OOOne.Tools.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace OOOne.Editor.Tests
{
    public class SetTextureSetting
    {
        private string fileName;
        private string absoluteFolderPath;
        private string unityAssetsFolderPath;
        private string pngPathBeforeParse;
        private string pngPathAfterParse;
        private string metaPathBeforeParse;
        private string metaPathAfterParse;
        private string fileExtension;
        private string targetPath;
        private string unityFullPath;


        [SetUp]
        public void SetUp()
        {
            fileName = "apple";
            fileExtension = ".png";
            unityAssetsFolderPath = "Assets/";
            absoluteFolderPath = Application.dataPath;
            unityFullPath = unityAssetsFolderPath + fileName + fileExtension;
            pngPathBeforeParse = absoluteFolderPath + "/" + fileName + fileExtension;
            pngPathAfterParse = FileUtility.ParsePathUnityToCsharp(pngPathBeforeParse);
        }




        [Test]
        public void ParsePathUnityToCsharpTests()
        {
            const string pathBeforeParse = "Assets/Plugins/OOOneUnityTools/Editor/FileUtility.cs";
            const string expectedResult = @"Assets\Plugins\OOOneUnityTools\Editor\FileUtility.cs";
            var pathAfterParse = FileUtility.ParsePathUnityToCsharp(pathBeforeParse);
            Assert.AreEqual(expectedResult, pathAfterParse);
        }


        [Test]
        public void DeleteTestsPngFromPathTests()
        {
            FileUtility.CreatTestPngByPath(absoluteFolderPath, fileName);
            FileUtility.DeleteFileWithMetaByPath(absoluteFolderPath, fileName, ".png");
            var metaFileExist = File.Exists(metaPathAfterParse);
            var pngFileExist = File.Exists(pngPathAfterParse);
            Assert.IsFalse(metaFileExist);
            Assert.IsFalse(pngFileExist);
        }


        // A Test behaves as an ordinary method
        [Test]
        public void CreateTestPNGFileToPathTests()
        {
            FileUtility.CreatTestPngByPath(absoluteFolderPath, fileName);
            var texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(unityFullPath);
            Assert.IsNotNull(texture2D);
            FileUtility.DeleteFileWithMetaByPath(absoluteFolderPath, fileName, ".png");
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