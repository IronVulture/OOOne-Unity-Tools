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
        private string _childPath;
        private string _fileName;
        private string _override_extension;
        private string _png_extension;
        private string _presetChildPath;
        private string _presetFileName;
        private string _unityFullFolderPath;
        private string _pngFullPath;
        private string _presetFullPath;

        #endregion

        #region Setup/Teardown Methods

        [SetUp]
        public void SetUp()
        {
            _childPath = "QWERT";
            _fileName = "WEjhdfjgh";
            _unityFullFolderPath = $@"{Application.dataPath}\{_childPath}";
            _anime_extension = "anim";
            _override_extension = "overrideController";
            _png_extension = "png";
            _pngFullPath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "png");
            _presetChildPath = "Test111";
            _presetFileName = "asdfeedd";
            _presetFullPath = UnityPathUtility.GetUnityFullPath(_presetChildPath, _presetFileName, "preset");
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
            ShouldFileInPath(fileType, true);
        }

        [Test]
        [TestCase(UnityFileUtility.FileType.AnimatorOverride)]
        [TestCase(UnityFileUtility.FileType.AnimationClip)]
        [TestCase(UnityFileUtility.FileType.Png)]
        public void Create_CustomFile_If_Folder_Not_Exist(UnityFileUtility.FileType fileType)
        {
            ShouldFileInPath(fileType, false);
            CreateAssetFileWithType(fileType);
            ShouldFileInPath(fileType, true);
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
            ShouldFileInPath(fileType, true);
        }

        [Test]
        public void CheckFileExtension()
        {
            var extension = "png";
            string fullPath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "png");
            CreateAssetFileWithType(UnityFileUtility.FileType.Png);
            bool extensionAreEqual = CSharpFileUtility.CheckFileExtension(fullPath, extension);
            Assert.AreEqual(true, extensionAreEqual);
        }

        [Test]
        public void ApplyPresetWhenFileAreValid()
        {
            CreateUnityFolderUseChild();
            CreatePngInChildPath();
            CreatePresetFolder(_presetChildPath);
            UnityFileUtility.CreatePreset(_presetFullPath, _pngFullPath, UnityFileUtility.PresetType.Texture2D);
            ModifyPngImporter(_pngFullPath);

            UnityFileUtility.SetTextureImporterSettings(_presetFullPath, _pngFullPath);

            var preset = AssetDatabase.LoadAssetAtPath<Preset>(_presetFullPath);
            var pngImporter = AssetImporter.GetAtPath(_pngFullPath) as TextureImporter;
            var dataEquals = preset.DataEquals(pngImporter);
            Assert.AreEqual(true, dataEquals);


            UnityFileUtility.DeleteUnityFolder(_presetChildPath);
        }

        [Test]
        public void DontApply_Preset_When_PresetType_NotTexture()
        {
            //創建png檔，取得其importer設定並存入比對用Preset
            CreateUnityFolderUseChild();
            CreatePngInChildPath();
            var beforePngPresetPath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "preset");
            UnityFileUtility.CreatePreset(beforePngPresetPath, _pngFullPath, UnityFileUtility.PresetType.Texture2D);

            CreateAssetFileWithType(UnityFileUtility.FileType.AnimatorOverride);
            var animatorOverridePath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "overrideController");
            CreatePresetFolder(_presetChildPath);
            UnityFileUtility.CreatePreset(_presetFullPath, animatorOverridePath , UnityFileUtility.PresetType.AnimatorController);

            UnityFileUtility.SetTextureImporterSettings(_presetFullPath, _childPath);

            //取得最後的importer資訊並與原本檔案的Preset比對
            var pngImporter_After = AssetImporter.GetAtPath(_pngFullPath) as TextureImporter;
            var presetBefore = AssetDatabase.LoadAssetAtPath<Preset>(beforePngPresetPath);
            var dataEquals = presetBefore.DataEquals(pngImporter_After);
            Assert.AreEqual(true, dataEquals);
            UnityFileUtility.DeleteUnityFolder(_presetChildPath);
        }

        [Test]
        public void DontApply_Preset_When_TargetFile_Extension_Not_Png()
        {
            //Arrange
            CreateUnityFolderUseChild();
            CreateAssetFileWithType(UnityFileUtility.FileType.AnimationClip);
            var animFullPath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "anim");
            CreatePngInChildPath();
            CreatePresetFolder(_presetChildPath);
            UnityFileUtility.CreatePreset(_presetFullPath, _pngFullPath, UnityFileUtility.PresetType.Texture2D);
            var preset = AssetDatabase.LoadAssetAtPath<Preset>(_presetFullPath);

            UnityFileUtility.SetTextureImporterSettings(_presetFullPath, animFullPath);

            var animImporter = AssetImporter.GetAtPath(animFullPath);
            bool importerSettingEqualsPreset = preset.DataEquals(animImporter);
            Assert.AreEqual( false , importerSettingEqualsPreset);
        }

        private static void ModifyPngImporter(string pngPath)
        {
            var importerBefore = AssetImporter.GetAtPath(pngPath) as TextureImporter;
            importerBefore.filterMode = FilterMode.Trilinear;
            importerBefore.SaveAndReimport();
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

        private static void CreatePresetFolder(string presetChildPath)
        {
            UnityFileUtility.CreateUnityFolder(presetChildPath);
        }

        private void CreateUnityFolderUseChild()
        {
            UnityFileUtility.CreateUnityFolder(_childPath);
        }

        private void DeleteUnityFolderUseChild()
        {
            UnityFileUtility.DeleteUnityFolder(_childPath);
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

        private void ShouldFileInPath(UnityFileUtility.FileType fileType, bool exist)
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