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
        private string _pngPath;

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
            _pngPath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "png");
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
            // _pngPath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "png");
            CreateUnityFolderUseChild();
            CreatePngInChildPath();
            var presetFullPath = "";
            Preset preset = CreatePreset(_presetFileName, _pngPath, UnityFileUtility.FileType.Png, out presetFullPath);

            //寫入Preset之前先改寫原本png的Importer設定值
            ModifyPngImporter(_pngPath);

            UnityFileUtility.SetTextureImporterSettings(presetFullPath, _pngPath);

            var pngImporter = AssetImporter.GetAtPath(_pngPath) as TextureImporter;
            var dataEquals = preset.DataEquals(pngImporter);
            Assert.AreEqual(true, dataEquals);
            UnityFileUtility.DeleteUnityFolder(_presetChildPath);
        }

        [Test]
        public void DontApply_Preset_When_PresetType_NotTexture()
        {
            //創建png檔，取得其importer設定並存入比對用Preset
            // _pngPath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "png");
            CreateUnityFolderUseChild();
            CreatePngInChildPath();
            ModifyPngImporter(_pngPath);
            var presetForCompare_Name = "tmpPresetName";
             var presetBeforeFullPath = "";
            Preset presetBefore = CreatePreset(presetForCompare_Name, _pngPath, UnityFileUtility.FileType.Png, out presetBeforeFullPath);

            //創建AnimatorOvderride檔案，使用其import Settings作為Preset
            var animatorOverridePath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, "overrideController");
            CreateAssetFileWithType(UnityFileUtility.FileType.AnimatorOverride);
            var presetFullPath = "";
            Preset preset = CreatePreset(_presetFileName, animatorOverridePath, UnityFileUtility.FileType.AnimatorOverride, out presetFullPath);

            //把Preset寫入importer
            UnityFileUtility.SetTextureImporterSettings(presetFullPath, _pngPath);

            //取得最後的importer資訊並與原本檔案的Preset比對
            var pngImporter = AssetImporter.GetAtPath(_pngPath) as TextureImporter;
            var dataEquals = presetBefore.DataEquals(pngImporter);
            Assert.AreEqual(true, dataEquals);
            UnityFileUtility.DeleteUnityFolder(_presetChildPath);
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

        private Preset CreatePreset(string presetFileName, string fileFullPath, UnityFileUtility.FileType fileType, out string presetFullPath)
        {
            // string presetChildPath = unityFileUtilityTests._presetChildPath;
            var preset = new Preset(AssetImporter.GetAtPath(fileFullPath));
            if (fileType == UnityFileUtility.FileType.Png)
            {
                var textureImporterForPreset = AssetImporter.GetAtPath(fileFullPath) as TextureImporter;
                // textureImporterForPreset.filterMode = FilterMode.Trilinear;
                preset = new Preset(textureImporterForPreset);
            }
            else
            {
                var assetImporterForPreset = AssetImporter.GetAtPath(fileFullPath);
                preset = new Preset(assetImporterForPreset);
            }
            var presetExtension = UnityFileUtility.GetExtension(UnityFileUtility.FileType.Preset);
            presetFullPath = UnityPathUtility.GetUnityFullPath(_presetChildPath, presetFileName, presetExtension);
            CreatePresetFolder(_presetChildPath);
            AssetDatabase.CreateAsset(preset, presetFullPath);
            return preset;
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