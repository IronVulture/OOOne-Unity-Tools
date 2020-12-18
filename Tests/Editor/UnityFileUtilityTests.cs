using System.Collections.Generic;
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

        private readonly string _animeExtension = "anim";
        private string _childPath;
        private string _fileName;
        private readonly string _overrideExtension = "overrideController";
        private readonly string _pngExtension = "png";
        private string _pngFullPath;
        private string _presetChildPath;
        private readonly string _presetExtension = "preset";
        private string _presetFileName;
        private string _presetFullPath;
        private string _unityFullFolderPath;
        private TextureImporter _mainTextureTextureImporter;

        #endregion

        #region Setup/Teardown Methods

        [SetUp]
        public void SetUp()
        {
            _childPath = "QWERT";
            _fileName = "WEjhdfjgh";
            _unityFullFolderPath = $@"{Application.dataPath}\{_childPath}";
            _presetChildPath = "Test111";
            _presetFileName = "asdfeedd";
            _pngFullPath = UnityPathUtility.GetUnityFullPath(_childPath, _fileName, _pngExtension);
            _presetFullPath = UnityPathUtility.GetUnityFullPath(_presetChildPath, _presetFileName, _presetExtension);
        }

        [TearDown]
        public void TearDown()
        {
            DeleteUnityFolderUseChild();
            DeletePresetFolder();
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
            CreateUnityFolderUseChild();
            CreatePngInChildPath();
            CreatePreset(GetUnityPathByExtension(_presetExtension), _pngFullPath, UnityFileUtility.FileType.Png);
            CreateAssetFileWithType(UnityFileUtility.FileType.AnimatorOverride);
            CreatePresetFolder(_presetChildPath);
            CreatePreset(_presetFullPath, GetUnityPathByExtension(_overrideExtension),
                UnityFileUtility.FileType.AnimatorOverride);
            SetTextureImporterSetting(_childPath);
            Assert.AreEqual(true, DataEquals(GetUnityPathByExtension(_presetExtension), _pngFullPath));
        }

        [Test]
        public void DontApply_Preset_When_TargetFile_Extension_Not_Png()
        {
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

        [Test]
        public void DontApply_Preset_when_Preset_Not_Exist()
        {
            CreateUnityFolderUseChild();
            CreatePngInChildPath();
            var assetImporter = GetImporter(_pngFullPath);
            CreatePreset(GetUnityPathByExtension(_presetExtension), _pngFullPath, UnityFileUtility.FileType.Png);
            SetTextureImporterSetting(_pngFullPath);
            Assert.AreEqual(true, DataEquals(GetUnityPathByExtension(_presetExtension), assetImporter));
        }

        [Test]
        [TestCase("_Normal")]
        [TestCase("_Normal", "_Rim")]
        public void Set_SecondaryTexture_When_All_Texture_Exists(params string[] secTextureNameList)
        {
            var mainTexturePath = "Assets/AJDJ/Cat1.png";
            var secondaryAssetPath = "Assets/AJDJ/Cat2.png";
            var secTextureDatas = GetSecTextureDatas(secTextureNameList, secondaryAssetPath);
            UnityFileUtility.SetSecondaryTexture(mainTexturePath, secTextureDatas);
            ShouldSecTextureEqual(secTextureNameList, mainTexturePath, secondaryAssetPath);
            ResetMainImporterSecTexture();
        }

        #endregion

        #region Public Methods

        public static void ShouldEqualResult(string expected, string result)
        {
            Assert.AreEqual(expected, result);
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

        private bool DataEquals(string presetPath, AssetImporter assetImporter)
        {
            return UnityFileUtility.DataEquals(presetPath, assetImporter);
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

        private static List<SecTextureData> GetSecTextureDatas(string[] secTextureNameList, string secondaryAssetPath)
        {
            var secTextureDatas = new List<SecTextureData>();
            for (var i = 0; i < secTextureNameList.Length; i++)
            {
                var secTextureName = secTextureNameList[i];
                var secTextureData = new SecTextureData {AssetPath = secondaryAssetPath, Name = secTextureName};
                secTextureDatas.Add(secTextureData);
            }

            return secTextureDatas;
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

        private void ResetMainImporterSecTexture()
        {
            _mainTextureTextureImporter.secondarySpriteTextures = new SecondarySpriteTexture[0];
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

        private void ShouldSecTextureEqual(string[] secTextureNameList, string mainTexturePath,
            string secondaryAssetPath)
        {
            _mainTextureTextureImporter = AssetImporter.GetAtPath(mainTexturePath) as TextureImporter;
            var secondarySpriteTextures = _mainTextureTextureImporter.secondarySpriteTextures;
            var secTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(secondaryAssetPath);
            for (var i = 0; i < secTextureNameList.Length; i++)
            {
                var secondarySpriteTexture = secondarySpriteTextures[i];
                var nameEqual = secondarySpriteTexture.name == secTextureNameList[i];
                var textureEqual = secondarySpriteTexture.texture == secTexture;
                var secondaryTextureEqual = nameEqual && textureEqual;
                Assert.AreEqual(true, secondaryTextureEqual);
            }
        }

        #endregion
    }
}