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
        private string _presetChildPath = "Test111";
        private string _presetFileName = "asdfeedd";
        private string _unityFullFolderPath;

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
            var pngPath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "png");
            CreateUnityFolderUseChild();
            CreatePngInChildPath();
            CreatePreset(_presetChildPath, _presetFileName, pngPath, UnityFileUtility.FileType.Png);
            var presetFullPath = UnityPathUtility.GetUnityFullPath(_presetChildPath, _presetFileName, "preset");
            var preset = AssetDatabase.LoadAssetAtPath<Preset>(presetFullPath);

            //寫入Preset之前先改寫原本png的Importer設定值
            var textureImporterForPng = AssetImporter.GetAtPath(pngPath) as TextureImporter;
            textureImporterForPng.filterMode = FilterMode.Trilinear;
            textureImporterForPng.SaveAndReimport();

            UnityFileUtility.ApplyPresetWhenFileAreValid(_presetChildPath, _presetFileName, _childPath, _fileName, UnityFileUtility.FileType.Png);

            var pngImporter = AssetImporter.GetAtPath(pngPath) as TextureImporter;
            var dataEquals = preset.DataEquals(pngImporter);
            Assert.AreEqual(true, dataEquals);
            UnityFileUtility.DeleteUnityFolder(_presetChildPath);
        }

        [Test]
        public void DontApply_Preset_When_PresetType_NotTexture()
        {
            //創建png檔，取得其importer設定並存入比對用Preset
            var pngPath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "png");
            CreateUnityFolderUseChild();
            CreatePngInChildPath();
            var importerBefore = AssetImporter.GetAtPath(pngPath) as TextureImporter;
            importerBefore.filterMode = FilterMode.Point;
            var presetForCompare_Name = "tmpPresetName";
            CreatePreset(_presetChildPath, presetForCompare_Name, pngPath, UnityFileUtility.FileType.Png);
            var presetBefore = AssetDatabase.LoadAssetAtPath<Preset>(
                UnityPathUtility.GetUnityFullPath(_presetChildPath, presetForCompare_Name, "preset"));

            //創建AnimatorOvderride檔案，使用其import Settings作為Preset
            var animatorOverridePath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "overrideController");
            CreateAssetFileWithType(UnityFileUtility.FileType.AnimatorOverride);
            CreatePreset(_presetChildPath, _presetFileName, animatorOverridePath, UnityFileUtility.FileType.AnimatorOverride);
            var preset =
                AssetDatabase.LoadAssetAtPath<Preset>(
                    UnityPathUtility.GetUnityFullPath(_presetChildPath, _presetFileName, "preset"));

            // Debug.Log("preset.GetPresetType().GetManagedTypeName(): " + preset.GetPresetType().GetManagedTypeName());

            //把Preset寫入importer
            UnityFileUtility.ApplyPresetWhenFileAreValid(_presetChildPath, _presetFileName, _childPath, _fileName, UnityFileUtility.FileType.Png);

            //取得最後的importer資訊並比對
            var pngImporter = AssetImporter.GetAtPath(pngPath) as TextureImporter;
            var dataEquals = presetBefore.DataEquals(pngImporter);
            Assert.AreEqual(true, dataEquals);
            UnityFileUtility.DeleteUnityFolder(_presetChildPath);
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

        private static void CreatePreset(string presetChildPath, string presetFileName,
            string fileFullPath, UnityFileUtility.FileType fileType)
        {
            var preset = new Preset(AssetImporter.GetAtPath(fileFullPath));
            if (fileType == UnityFileUtility.FileType.Png)
            {
                var textureImporterForPreset = AssetImporter.GetAtPath(fileFullPath) as TextureImporter;
                // textureImporterForPreset.filterMode = FilterMode.Trilinear;
                preset = new Preset(textureImporterForPreset);
            }

            var presetExtension = "preset";
            var presetFullPath = UnityPathUtility.GetUnityFullPath(presetChildPath, presetFileName, presetExtension);
            CreatePresetFolder(presetChildPath);
            AssetDatabase.CreateAsset(preset, presetFullPath);
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