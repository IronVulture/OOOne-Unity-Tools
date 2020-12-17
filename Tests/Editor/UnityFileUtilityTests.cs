using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace OOOneUnityTools.Editor.Tests
{
    public class UnityFileUtilityTests
    {
        #region Private Variables

        private string _anime_extension;
        private readonly string _animeExtension = "anim";
        private string _childPath;
        private string _fileName;
        private string _override_extension;
        private readonly string _overrideExtension = "overrideController";
        private string _png_extension;
        private readonly string _pngExtension = "png";
        private string _pngFullPath;
        private string _presetChildPath;
        private readonly string _presetExtension = "preset";
        private string _presetFileName;
        private string _presetFullPath;
        private string _unityFullFolderPath;

        #endregion

        #region Setup/Teardown Methods

        [SetUp]
        public void SetUp()
        {
            _childPath = "QWERT";
            _fileName = "WEjhdfjgh";
            _unityFullFolderPath = $@"{Application.dataPath}\{_childPath}";
            _anime_extension = _animeExtension;
            _override_extension = _overrideExtension;
            _png_extension = _pngExtension;
            _presetChildPath = "Test111";
            _presetFileName = "asdfeedd";
            _pngFullPath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, _pngExtension);
            _presetFullPath = UnityPathUtility.GetUnityFullPath(_presetChildPath, _presetFileName, _presetExtension);
        }

        #endregion

        #region Test Methods

        [Test]
        public void CreateUnityFolderIfNotExist()
        {
            CreateUnityFolderUseChild();
            ShouldFolderExist(true, _childPath);
        }

        [Test]
        public void CreateUnityFolderInFolderIfNotExist()
        {
            var path = _childPath + "/" + _childPath;
            UnityFileUtility.CreateUnityFolder(path);
            ShouldFolderExist(true, path);
        }

        [Test]
        public void DontCreateFolderIfExist()
        {
            CreateUnityFolderUseChild();
            ShouldFolderExist(false, _childPath + " 1");
        }

        [Test]
        public void DeleteUnityFolder()
        {
            CreateUnityFolderUseChild();
            DeleteUnityFolderUseChild();
            ShouldFolderExist(false, _childPath);
        }


        [Test]
        [TestCase(UnityFileUtility.FileType.AnimatorOverride)]
        [TestCase(UnityFileUtility.FileType.AnimationClip)]
        [TestCase(UnityFileUtility.FileType.Png)]
        public void Create_CustomFile_If_Folder_Exist(UnityFileUtility.FileType fileType)
        {
            CreateUnityFolderUseChild();
            CreateAssetFileWithType(fileType);
            ShouldFileInPath(true, fileType);
        }

        [Test]
        [TestCase(UnityFileUtility.FileType.AnimatorOverride)]
        [TestCase(UnityFileUtility.FileType.AnimationClip)]
        [TestCase(UnityFileUtility.FileType.Png)]
        public void Create_CustomFile_If_Folder_Not_Exist(UnityFileUtility.FileType fileType)
        {
            ShouldFileInPath(false, fileType);
            CreateAssetFileWithType(fileType);
            ShouldFileInPath(true, fileType);
        }

        [Test]
        [TestCase(UnityFileUtility.FileType.AnimatorOverride)]
        [TestCase(UnityFileUtility.FileType.AnimationClip)]
        [TestCase(UnityFileUtility.FileType.Png)]
        public void Dont_Create_CustomFile_If_File_Exist(UnityFileUtility.FileType fileType)
        {
            var extension = UnityFileUtility.GetExtension(fileType);
            var cSharpFullPath = UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_childPath, _fileName, extension);
            Assert.AreEqual(true, NotCreateWhenFileExist(fileType, cSharpFullPath));
            ShouldFileInPath(true, fileType);
        }

        [Test]
        public void CheckFileExtension()
        {
            CreateAssetFileWithType(UnityFileUtility.FileType.Png);
            var extensionAreEqual = CSharpFileUtility.CheckFileExtension(_pngFullPath, _pngExtension);
            Assert.AreEqual(true, extensionAreEqual);
        }

        [Test]
        public void ApplyPresetWhenFileAreValid()
        {
            CreateUnityFolderUseChild();
            CreatePngInChildPath();
            CreatePresetFolder(_presetChildPath);
            CreatePreset(_presetFullPath, _pngFullPath, UnityFileUtility.FileType.Png);
            ModifyPngImporter(_pngFullPath);
            SetTextureImporterSetting(_pngFullPath);
            Assert.AreEqual(true, DataEquals(_presetFullPath, _pngFullPath));
        }

        [Test]
        public void DontApply_Preset_When_PresetType_NotTexture()
        {
            //創建png檔，取得其importer設定並存入比對用Preset
            CreateUnityFolderUseChild();
            CreatePngInChildPath();
            CreatePreset(GetUnityPathByExtension(_presetExtension), _pngFullPath, UnityFileUtility.FileType.Png);
            CreateAssetFileWithType(UnityFileUtility.FileType.AnimatorOverride);
            CreatePresetFolder(_presetChildPath);
            CreatePreset(_presetFullPath, GetUnityPathByExtension(_overrideExtension),
                UnityFileUtility.FileType.AnimatorOverride);
            SetTextureImporterSetting(_childPath);
            //取得最後的importer資訊並與原本檔案的Preset比對
            Assert.AreEqual(true, DataEquals(GetUnityPathByExtension(_presetExtension), _pngFullPath));
        }

        [Test]
        public void DontApply_Preset_When_TargetFile_Extension_Not_Png()
        {
            //Arrange
            CreateUnityFolderUseChild();
            CreateAssetFileWithType(UnityFileUtility.FileType.AnimationClip);
            var animFullPath = GetUnityPathByExtension(_animeExtension);
            CreatePngInChildPath();
            CreatePresetFolder(_presetChildPath);
            CreatePreset(_presetFullPath, _pngFullPath, UnityFileUtility.FileType.Png);
            SetTextureImporterSetting(animFullPath);
            Assert.AreEqual(false, DataEquals(_presetFullPath, animFullPath));
        }

        [Test]
        public void DontApply_Preset_when_TargetFile_Not_Exist()
        {
            CreateUnityFolderUseChild();
            CreatePngInChildPath();
            CreatePresetFolder(_presetChildPath);
            CreatePreset(_presetFullPath, _pngFullPath, UnityFileUtility.FileType.Png);
            DeleteUnityFolderUseChild();
            CreateUnityFolderUseChild();
            SetTextureImporterSetting(_pngFullPath);
            ShouldFileInPath(false, UnityFileUtility.FileType.Png);
        }

        #endregion

        #region Public Methods

        public static void ShouldEqualResult(string expected, string result)
        {
            Assert.AreEqual(expected, result);
        }

        [TearDown]
        public void TearDown()
        {
            DeleteUnityFolderUseChild();
            DeletePresetFolder();
        }

        #endregion

        #region Private Methods

        private bool CreateAssetFileWithType(UnityFileUtility.FileType fileType)
        {
            return UnityFileUtility.CreateAssetFile(fileType, _childPath, _fileName);
        }

        private void CreatePngInChildPath()
        {
            UnityFileUtility.CreateTestPng(_childPath, _fileName, TextureColor.white);
        }

        private void CreatePreset(string presetFullPath, string fileFullPath, UnityFileUtility.FileType fileType)
        {
            UnityFileUtility.CreatePreset(presetFullPath, fileFullPath, fileType);
        }

        private static void CreatePresetFolder(string presetChildPath)
        {
            UnityFileUtility.CreateUnityFolder(presetChildPath);
        }

        private void CreateUnityFolderUseChild()
        {
            UnityFileUtility.CreateUnityFolder(_childPath);
        }

        private bool DataEquals(string presetPath, string assetFullPath)
        {
            return UnityFileUtility.DataEquals(presetPath, assetFullPath);
        }

        private void DeletePresetFolder()
        {
            UnityFileUtility.DeleteUnityFolder(_presetChildPath);
        }

        private void DeleteUnityFolderUseChild()
        {
            UnityFileUtility.DeleteUnityFolder(_childPath);
        }

        private AssetImporter GetImporter(string assetFullPath)
        {
            return UnityFileUtility.GetImporter(assetFullPath);
        }

        private string GetUnityPathByExtension(string extension)
        {
            return UnityPathUtility.GetUnityFullPath(_childPath, _fileName, extension);
        }

        private Preset LoadPresetAtPath(string presetPath)
        {
            return UnityFileUtility.LoadPresetAtPath(presetPath);
        }

        private static void ModifyPngImporter(string pngPath)
        {
            var importerBefore = AssetImporter.GetAtPath(pngPath) as TextureImporter;
            importerBefore.filterMode = FilterMode.Trilinear;
            importerBefore.SaveAndReimport();
        }

        private bool NotCreateWhenFileExist(UnityFileUtility.FileType fileType, string cSharpFullPath)
        {
            CreateAssetFileWithType(fileType);
            var modifyTimeBefore = File.GetLastWriteTime(cSharpFullPath);
            var isCreated = CreateAssetFileWithType(fileType);
            var modifyTimeAfter = File.GetLastWriteTime(cSharpFullPath);
            var modifyTimeIsEqual = modifyTimeAfter == modifyTimeBefore;
            return modifyTimeIsEqual && isCreated == false;
        }

        private void SetTextureImporterSetting(string texturePath)
        {
            UnityFileUtility.SetTextureImporterSetting(_presetFullPath, texturePath);
        }

        private void ShouldFileInPath(bool exist, UnityFileUtility.FileType fileType)
        {
            var isFileInPath = UnityFileUtility.IsFileInPath(_unityFullFolderPath, _fileName, fileType);
            Assert.AreEqual(exist, isFileInPath);
        }

        private void ShouldFolderExist(bool expected, string path)
        {
            Assert.AreEqual(expected, UnityFileUtility.IsUnityFolderExist(path));
        }

        #endregion
    }
}