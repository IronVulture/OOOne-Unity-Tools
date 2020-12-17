using System.IO;
using NUnit.Framework;
using Plugins.OOOneUnityTools.Editor;
using UnityEngine;

namespace OOOneTools.Editor.Tests
{
    public class UnityFileUtilityTests
    {
    #region Public Variables

        public const  string _bb = "";
        public static string tes;

        public         string _aa = "";

    #endregion

    #region Private Variables

        private const  string Djsak = "";
        private static string _fsd;
        private        string _anime_extension;
        private        string _childPath;
        private        string _fileName;
        private        string _override_extension;
        private        string _pngExtension;
        private        string _unityFullFolderPath;

    #endregion

    #region Setup/Teardown Methods

        [SetUp]
        private void SetUp()
        {
            _childPath           = "QWERT";
            _fileName            = "WEjhdfjgh";
            _unityFullFolderPath = $@"{Application.dataPath}\{_childPath}";
            _anime_extension     = "anim";
            _override_extension  = "overrideController";
            _pngExtension        = "png";
        }

        [TearDown]
        private void TearDown()
        {
            DeleteUnityFolderUseChild();
        }

    #endregion

    #region Test Methods

        [Test]
        public void CreateUnityFolderIfNotExist()
        {
            CreateUnityFolderUseChild();
            ShouldFolderExist(true , _childPath);
        }

        [Test]
        public void CreateUnityFolderInFolderIfNotExist()
        {
            var path = _childPath + "/" + _childPath;
            UnityFileUtility.CreateUnityFolder(path);
            ShouldFolderExist(true , path);
        }

        [Test]
        public void DontCreateFolderIfExist()
        {
            CreateUnityFolderUseChild();
            ShouldFolderExist(false , _childPath + " 1");
        }

        [Test]
        public void DeleteUnityFolder()
        {
            CreateUnityFolderUseChild();
            DeleteUnityFolderUseChild();
            ShouldFolderExist(false , _childPath);
        }

        [Test]
        public void CreateAnimationOverrideIfFolderNoExists()
        {
            CreateAnimatorOverride();
            ShouldFileInPath(_override_extension);
        }

        [Test]
        public void CreateAnimationOverrideIfFolderExists()
        {
            CreateUnityFolderUseChild();
            CreateAnimatorOverride();
            ShouldFileInPath(_override_extension);
        }

        [Test]
        public void CreateAnimationClipIfFolderNoExists()
        {
            CreateAnimationClip();
            ShouldFileInPath(_anime_extension);
        }

        [Test]
        public void CreateAnimationClipIfFolderExists()
        {
            CreateUnityFolderUseChild();
            CreateAnimationClip();
            Assert.AreEqual(true , IsFileInPath(_anime_extension));
        }

        [Test]
        public void CreatePngIfFolderNoExists()
        {
            CreatePng();
            ShouldPngInPath();
        }

        [Test]
        public void CreatePngIfFolderExists()
        {
            CreateUnityFolderUseChild();
            CreatePng();
            ShouldPngInPath();
        }

        [Test]
        [TestCase(UnityFileUtility.FileType.AnimatorOverride)]
        public void CreateCustomFileIfFolderExist(UnityFileUtility.FileType fileType)
        {
            CreateUnityFolderUseChild();
            UnityFileUtility.CreateAssetFile(fileType , _childPath , _fileName);
            ShouldFileInPath(fileType , true);
        }

        [Test]
        public void NotCreateAnimationClipIfFileAndFolderExist()
        {
            CreateUnityFolderUseChild();
            CreateAnimationClip();
            var path                = GetUnityFullPath(_anime_extension);
            var oldModifyTime       = File.GetCreationTime(path);
            var isFileCreateSuccess = CreateAnimationClip();
            var newModifyTime       = File.GetCreationTime(path);

            Assert.AreEqual(true , IsFileInPath(_anime_extension));
            ShouldFileCountCorrect();
            Assert.AreEqual(oldModifyTime , newModifyTime);
            Assert.AreEqual(false ,         isFileCreateSuccess);
        }

        [Test]
        public void NotCreateAnimationOverrideIfFileAndFolderExist()
        {
            CreateUnityFolderUseChild();
            CreateAnimatorOverride();
            var path                = GetUnityFullPath(_override_extension);
            var oldModifyTime       = File.GetCreationTime(path);
            var isFileCreateSuccess = CreateAnimatorOverride();
            var newModifyTime       = File.GetCreationTime(path);
            Assert.AreEqual(true , IsFileInPath(_override_extension));
            ShouldFileCountCorrect();
            Assert.AreEqual(oldModifyTime , newModifyTime);
            Assert.AreEqual(false ,         isFileCreateSuccess);
        }

        [Test]
        public void NotCreatePngIfFileAndFolderExist()
        {
            CreateUnityFolderUseChild();
            UnityFileUtility.TryCreatePng(_childPath , _fileName);
            var path                = GetUnityFullPath(_pngExtension);
            var oldModifyTime       = File.GetCreationTime(path);
            var isFileCreateSuccess = UnityFileUtility.TryCreatePng(_childPath , _fileName);
            var newModifyTime       = File.GetCreationTime(path);
            Assert.AreEqual(true , IsFileInPath(_pngExtension));
            ShouldFileCountCorrect();
            Assert.AreEqual(oldModifyTime , newModifyTime);
            Assert.AreEqual(false ,         isFileCreateSuccess);
        }

        [Test]
        public void GetUnityPath()
        {
            ShouldEqualResult("Assets" , UnityFileUtility.GetUnityPath());
        }

        [Test]
        public void GetUnityAbsolutePath()
        {
            ShouldEqualResult(Application.dataPath , UnityFileUtility.GetAbsolutePath());
        }

        [Test]
        public void GetUnityFolderPath()
        {
            ShouldEqualResult("Assets/" + _childPath , UnityFileUtility.GetUnityFolderPath(_childPath));
        }

        [Test]
        public void GetUnityAbsoluteFolderPath()
        {
            ShouldEqualResult($"{Application.dataPath}/{_childPath}" ,
                              UnityFileUtility.GetUnityAbsoluteFolderPath(_childPath));
        }

        [Test]
        public void GetUnityFullPath()
        {
            var extension = "overrideController";
            var expected  = $"Assets/{_childPath}/{_fileName}.{extension}";
            ShouldEqualResult(expected , UnityFileUtility.GetUnityFullPath(_childPath , _fileName , extension));
        }

        [Test]
        public void GetUnityAbsoluteFullPath()
        {
            var extension = "overrideController";
            var expected  = $"{Application.dataPath}/{_childPath}/{_fileName}.{extension}";
            ShouldEqualResult(expected , UnityFileUtility.GetUnityAbsoluteFullPath(_childPath , _fileName , extension));
        }

    #endregion

    #region Private Methods

        private bool CreateAnimationClip() => UnityFileUtility.TryCreateAnimationClip(_childPath , _fileName);

        private bool CreateAnimatorOverride()
        {
            return UnityFileUtility.TryCreateAnimatorOverride(_childPath , _fileName);
        }

        private void CreatePng() => UnityFileUtility.CreatePng(_childPath , _fileName);

        private void CreateUnityFolderUseChild() => UnityFileUtility.CreateUnityFolder(_childPath);

        private void DeleteUnityFolderUseChild() => UnityFileUtility.DeleteUnityFolder(_childPath);

        private string GetUnityFullPath(string extension) => _unityFullFolderPath + "/" + _fileName + "." + extension;

        private bool IsFileInPath(string fileExtension) =>
            CSharpFileUtility.IsFileInPath(_unityFullFolderPath , _fileName , fileExtension);

        private static void ShouldEqualResult(string expected , string result)
        {
            Assert.AreEqual(expected , result);
        }

        private void ShouldFileCountCorrect()
        {
            Assert.AreEqual(2 , Directory.GetFiles(_unityFullFolderPath).Length);
        }

        private void ShouldFileInPath(UnityFileUtility.FileType fileType , bool exist)
        {
            var isFileInPath = UnityFileUtility.IsFileInPath(_unityFullFolderPath , _fileName , fileType);
            Assert.AreEqual(exist , isFileInPath);
        }

        private void ShouldFileInPath(string overrideExtension)
        {
            Assert.AreEqual(true , IsFileInPath(overrideExtension));
        }

        private void ShouldFolderExist(bool expected , string path)
        {
            Assert.AreEqual(expected , UnityFileUtility.IsUnityFolderExist(path));
        }

        private void ShouldPngInPath()
        {
            Assert.AreEqual(true , IsFileInPath(_pngExtension));
        }

    #endregion
    }
}