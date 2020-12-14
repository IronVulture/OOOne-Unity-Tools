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
        private string _folderPath;
        private string _override_extension;
        private string _png_extension;

        #endregion

        #region Setup/Teardown Methods

        [SetUp]
        public void SetUp()
        {
            _childPath = "QWERT";
            _fileName = "WEjhdfjgh";
            _folderPath = $@"{Application.dataPath}\{_childPath}";
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
            Assert.IsTrue(UnityFileUtility.IsUnityFolderExist(_childPath));
        }

        [Test]
        public void CreateUnityFolderInFolderIfNotExist()
        {
            var path = _childPath + "/" + _childPath;
            UnityFileUtility.CreateUnityFolder(path);
            Assert.IsTrue(UnityFileUtility.IsUnityFolderExist(path));
        }

        [Test]
        public void DontCreateFolderIfExist()
        {
            CreateUnityFolderUseChild();
            Assert.IsFalse(UnityFileUtility.IsUnityFolderExist(_childPath + " 1"));
        }

        [Test]
        public void DeleteUnityFolder()
        {
            CreateUnityFolderUseChild();
            DeleteUnityFolderUseChild();
            Assert.AreEqual(false, UnityFileUtility.IsUnityFolderExist(_childPath));
        }

        [Test]
        public void CreateAnimationOverrideIfFolderNoExists()
        {
            CreateAnimationOverride();
            ShouldFileInPath(_override_extension);
        }

        [Test]
        public void CreateAnimationOverrideIfFolderExists()
        {
            CreateUnityFolderUseChild();
            CreateAnimationOverride();
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
            Assert.AreEqual(true, IsFileInPath(_anime_extension));
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
        public void NotCreateAnimationClipIfFileAndFolderExist()
        {
            CreateUnityFolderUseChild();
            CreateAnimationClip();
            var path = GetUnityFullPath(_anime_extension);
            var oldModifyTime = File.GetCreationTime(path);
            var isFileCreateSuccess = CreateAnimationClip();
            var newModifyTime = File.GetCreationTime(path);
            var fileCountInFolder = Directory.GetFiles(_folderPath).Length;
            Assert.AreEqual(true, IsFileInPath(_anime_extension));
            Assert.AreEqual(2, fileCountInFolder);
            Assert.AreEqual(oldModifyTime, newModifyTime);
            Assert.AreEqual(false, isFileCreateSuccess);
        }

        [Test]
        public void NotCreateAnimationOverrideIfFileAndFolderExist()
        {
            CreateUnityFolderUseChild();
            CreateAnimationOverride();
            var path = GetUnityFullPath(_override_extension);
            var oldModifyTime = File.GetCreationTime(path);
            var isFileCreateSuccess = CreateAnimationOverride();
            var newModifyTime = File.GetCreationTime(path);
            var fileCountInFolder = Directory.GetFiles(_folderPath).Length;
            Assert.AreEqual(true, IsFileInPath(_override_extension));
            Assert.AreEqual(2, fileCountInFolder);
            Assert.AreEqual(oldModifyTime, newModifyTime);
            Assert.AreEqual(false, isFileCreateSuccess);
        }

        [Test]
        public void NotCreatePngIfFileAndFolderExist()
        {
            CreateUnityFolderUseChild();
            UnityFileUtility.TryCreatePng(_childPath, _fileName);
            var path = GetUnityFullPath(_png_extension);
            var oldModifyTime = File.GetCreationTime(path);
            var isFileCreateSuccess = UnityFileUtility.TryCreatePng(_childPath, _fileName);
            var newModifyTime = File.GetCreationTime(path);
            var fileCountInFolder = Directory.GetFiles(_folderPath).Length;
            Assert.AreEqual(true, IsFileInPath(_png_extension));
            Assert.AreEqual(2, fileCountInFolder);
            Assert.AreEqual(oldModifyTime, newModifyTime);
            Assert.AreEqual(false, isFileCreateSuccess);
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

        private bool CreateAnimationClip() => UnityFileUtility.TryCreateAnimationClip(_childPath, _fileName);

        private bool CreateAnimationOverride() => UnityFileUtility.TryCreateAnimationOverride(_childPath, _fileName);

        private void CreatePng() => UnityFileUtility.CreatePng(_childPath, _fileName);

        private void CreateUnityFolderUseChild() => UnityFileUtility.CreateUnityFolder(_childPath);

        private void DeleteUnityFolderUseChild() => UnityFileUtility.DeleteUnityFolder(_childPath);

        private string GetUnityFullPath(string extension) => _folderPath + "/" + _fileName + "." + extension;

        private bool IsFileInPath(string fileExtension) =>
            CSharpFileUtility.IsFileInPath(_folderPath, _fileName, fileExtension);

        private void ShouldFileInPath(string overrideExtension)
        {
            Assert.AreEqual(true, IsFileInPath(overrideExtension));
        }

        private void ShouldPngInPath()
        {
            Assert.AreEqual(true, IsFileInPath(_png_extension));
        }

        #endregion
    }
}