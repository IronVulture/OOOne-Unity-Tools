using System.IO;
using NUnit.Framework;
using Plugins.OOOneUnityTools.Editor;
using UnityEngine;

namespace OOOneTools.Editor.Tests
{
    public class UnityFileUtilityTests
    {
    #region Private Variables

        private string _anime_extension;
        private string _childPath;
        private string _fileName;
        private string _override_extension;
        private string _png_extension;
        private string _unityFullFolderPath;

    #endregion

    #region Setup/Teardown Methods

        [SetUp]
        public void SetUp()
        {
            _childPath           = "QWERT";
            _fileName            = "WEjhdfjgh";
            _unityFullFolderPath = $@"{Application.dataPath}\{_childPath}";
            _anime_extension     = "anim";
            _override_extension  = "overrideController";
            _png_extension       = "png";
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
        [TestCase(UnityFileUtility.FileType.AnimationClip)]
        [TestCase(UnityFileUtility.FileType.Png)]
        public void Create_CustomFile_If_Folder_Exist(UnityFileUtility.FileType fileType)
        {
            CreateUnityFolderUseChild();
            UnityFileUtility.CreateAssetFile(fileType , _childPath , _fileName);
            ShouldFileInPath(fileType , true);
        }

        [Test]
        [TestCase(UnityFileUtility.FileType.AnimatorOverride)]
        [TestCase(UnityFileUtility.FileType.AnimationClip)]
        [TestCase(UnityFileUtility.FileType.Png)]
        public void Create_CustomFile_If_Folder_Not_Exist(UnityFileUtility.FileType fileType)
        {
            ShouldFileInPath(fileType , false);
            UnityFileUtility.CreateAssetFile(fileType , _childPath , _fileName);
            ShouldFileInPath(fileType , true);
        }

    #endregion

    #region Public Methods

        [TearDown]
        public void TearDown()
        {
            DeleteUnityFolderUseChild();
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

        public static void ShouldEqualResult(string expected , string result)
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
            Assert.AreEqual(true , IsFileInPath(_png_extension));
        }

    #endregion
    }
}