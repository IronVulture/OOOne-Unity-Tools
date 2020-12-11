#region

using System.IO;
using NUnit.Framework;
using OOOne.Tools.Editor;
using UnityEngine;

#endregion

namespace OOOneTools.Editor.Tests
{
    public class FileUtilityTests
    {
        private FileUtility _fileUtility;
        private string _fileName;
        private string _absoluteFolderPath;
        private string _pngFileExtension;
        private string _unityAssetsFolderPath;


        [SetUp]
        public void SetUp()
        {
            _fileUtility = new FileUtility();
            _fileName = "apple";
            _pngFileExtension = ".png";
            _absoluteFolderPath = Application.dataPath;
            _unityAssetsFolderPath = "Assets/";
        }
        

        [Test]
        public void Get_All_OverrideController_By_Folder()
        {
            var folderPath = "Assets/TTTT";
            var hasFile = _fileUtility.TryGetAllOverrideControllerByFolder(folderPath
                                                                           , out var overrideControllers);
            Assert.AreEqual(true , hasFile);
            Assert.NotNull(overrideControllers);
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


        [Test]
        public void CreateTexturePresetToPathTests()
        {
            var presetPath = _unityAssetsFolderPath;
            const string presetFileName = "testPreset";
            const string presetFileExtension = ".preset";
            FileUtility.CreateTestTexturePreset(presetPath, presetFileName);
            var absoluteFullPath = _absoluteFolderPath + presetFileName + presetFileExtension;
            var presetFileExist = File.Exists(absoluteFullPath);
            Assert.IsFalse(presetFileExist);
            FileUtility.DeleteFileWithMetaByPath(_absoluteFolderPath, presetFileName, presetFileExtension);
        }
    }
}