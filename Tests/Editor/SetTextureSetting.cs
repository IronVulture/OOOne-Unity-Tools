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
        private string _fileName;
        private string _absoluteFolderPath;
        private string _pngFileExtension;

        [SetUp]
        public void SetUp()
        {
            _fileName = "apple";
            _pngFileExtension = ".png";
            _absoluteFolderPath = Application.dataPath;
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
            var metaPathBeforeParse = _absoluteFolderPath + "/" + _fileName + ".meta";
            var metaPathAfterParse = FileUtility.ParsePathUnityToCsharp(metaPathBeforeParse);
            var pngPathBeforeParse = _absoluteFolderPath + "/" + _fileName + _pngFileExtension;
            var pngPathAfterParse = FileUtility.ParsePathUnityToCsharp(pngPathBeforeParse);
            FileUtility.CreatTestPngByPath(_absoluteFolderPath, _fileName);
            FileUtility.DeleteFileWithMetaByPath(_absoluteFolderPath, _fileName, ".png");
            var metaFileExist = File.Exists(metaPathAfterParse);
            var pngFileExist = File.Exists(pngPathAfterParse);
            Assert.IsFalse(metaFileExist);
            Assert.IsFalse(pngFileExist);
        }


        // A Test behaves as an ordinary method
        [Test]
        public void CreateTestPNGFileToPathTests()
        {
            var unityAssetsFolderPath = "Assets/";
            var unityFullPath = unityAssetsFolderPath + _fileName + _pngFileExtension;
            FileUtility.CreatTestPngByPath(_absoluteFolderPath, _fileName);
            var texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(unityFullPath);
            Assert.IsNotNull(texture2D);
            FileUtility.DeleteFileWithMetaByPath(_absoluteFolderPath, _fileName, ".png");
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