using System.IO;
using NUnit.Framework;
using Plugins.OOOneUnityTools.Editor;
using UnityEngine;

namespace OOOneTools.Editor.Tests
{
    public class UnityFileUtilityTests
    {
        private string _childPath;
        private string _fileName;
        private string _folderPath;
        private string _anime_extension;
        private string _override_extension;
        private string _png_extension;

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

        [Test]
        public void CreateUnityFolderIfNotExist()
        {
            CreateUnityFolderUseChild();
            Assert.IsTrue(UnityFileUtility.IsUnityFolderExist(_childPath));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void CreateUnityFolderInFolderIfNotExist()
        {
            var path = _childPath + "/" + _childPath;
            UnityFileUtility.CreateUnityFolder(path);
            Assert.IsTrue(UnityFileUtility.IsUnityFolderExist(path));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void DontCreateFolderIfExist()
        {
            CreateUnityFolderUseChild();
            Assert.IsFalse(UnityFileUtility.IsUnityFolderExist(_childPath + " 1"));
            DeleteUnityFolderUseChild();
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
            Assert.AreEqual(true, IsFileInPath(_override_extension));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void CreateAnimationOverrideIfFolderExists()
        {
            CreateUnityFolderUseChild();
            CreateAnimationOverride();
            Assert.AreEqual(true, IsFileInPath(_override_extension));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void CreateAnimationClipIfFolderNoExists()
        {
            CreateAnimationClip();
            Assert.AreEqual(true, IsFileInPath(_anime_extension));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void CreateAnimationClipIfFolderExists()
        {
            CreateUnityFolderUseChild();
            CreateAnimationClip();
            Assert.AreEqual(true, IsFileInPath(_anime_extension));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void CreatePngIfFolderNoExists()
        {
            UnityFileUtility.TryCreatePng(_childPath, _fileName);
            Assert.AreEqual(true, IsFileInPath(_png_extension));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void CreatePngIfFolderExists()
        {
            CreateUnityFolderUseChild();
            UnityFileUtility.TryCreatePng(_childPath, _fileName);
            Assert.AreEqual(true, IsFileInPath(_png_extension));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void NotCreateAnimationClipIfFileAndFolderExist()
        {
            CreateUnityFolderUseChild();
            CreateAnimationClip();
            string path = GetUnityFullPath(_folderPath, _fileName, _anime_extension);
            var oldModifyTime = File.GetCreationTime(path);
            var isFileCreateSuccess = CreateAnimationClip();
            var newModifyTime = File.GetCreationTime(path);
            var fileCountInFolder = Directory.GetFiles(_folderPath).Length;
            Assert.AreEqual(true, IsFileInPath(_anime_extension));
            Assert.AreEqual(2, fileCountInFolder);
            Assert.AreEqual(oldModifyTime, newModifyTime);
            Assert.AreEqual(false, isFileCreateSuccess);
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void NotCreateAnimationOverrideIfFileAndFolderExist()
        {
            CreateUnityFolderUseChild();
            CreateAnimationOverride();
            string path = GetUnityFullPath(_folderPath, _fileName, _override_extension);
            var oldModifyTime = File.GetCreationTime(path);
            var isFileCreateSuccess = CreateAnimationOverride();
            var newModifyTime = File.GetCreationTime(path);
            var fileCountInFolder = Directory.GetFiles(_folderPath).Length;
            Assert.AreEqual(true, IsFileInPath(_override_extension));
            Assert.AreEqual(2, fileCountInFolder);
            Assert.AreEqual(oldModifyTime, newModifyTime);
            Assert.AreEqual(false, isFileCreateSuccess);
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void NotCreatePngIfFileAndFolderExist()
        {
            CreateUnityFolderUseChild();
            UnityFileUtility.TryCreatePng(_childPath, _fileName);
            string path = GetUnityFullPath(_folderPath, _fileName, _png_extension);
            var oldModifyTime = File.GetCreationTime(path);
            var isFileCreateSuccess = UnityFileUtility.TryCreatePng(_childPath, _fileName);
            var newModifyTime = File.GetCreationTime(path);
            var fileCountInFolder = Directory.GetFiles(_folderPath).Length;
            Assert.AreEqual(true, IsFileInPath(_png_extension));
            Assert.AreEqual(2, fileCountInFolder);
            Assert.AreEqual(oldModifyTime, newModifyTime);
            Assert.AreEqual(false, isFileCreateSuccess);
            DeleteUnityFolderUseChild();
        }


        private string GetUnityFullPath(string folderPath, string fileName, object extension)
        {
            return _folderPath + "/" + _fileName + "." +extension;
        }

        private bool CreateAnimationClip() => UnityFileUtility.TryCreateAnimationClip(_childPath, _fileName);

        private bool CreateAnimationOverride() => UnityFileUtility.TryCreateAnimationOverride(_childPath, _fileName);

        private void DeleteUnityFolderUseChild() => UnityFileUtility.DeleteUnityFolder(_childPath);

        private void CreateUnityFolderUseChild() => UnityFileUtility.CreateUnityFolder(_childPath);

        private bool IsFileInPath(string fileExtension) =>
            CSharpFileUtility.IsFileInPath(_folderPath, _fileName, fileExtension);
    }
}