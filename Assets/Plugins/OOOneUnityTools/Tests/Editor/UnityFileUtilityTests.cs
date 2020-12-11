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

        [SetUp]
        public void SetUp()
        {
            _childPath = "QWERT";
            _fileName = "WEjhdfjgh";
            _folderPath = $@"{Application.dataPath}\{_childPath}";
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
            Assert.AreEqual(true, IsFileInPath("overrideController"));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void CreateAnimationOverrideIfFolderExists()
        {
            CreateUnityFolderUseChild();
            CreateAnimationOverride();
            Assert.AreEqual(true, IsFileInPath("overrideController"));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void CreateAnimationClipIfFolderNoExists()
        {
            CreateAnimationClip();
            Assert.AreEqual(true, IsFileInPath("anim"));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void CreateAnimationClipIfFolderExists()
        {
            CreateUnityFolderUseChild();
            CreateAnimationClip();
            Assert.AreEqual(true, IsFileInPath("anim"));
            DeleteUnityFolderUseChild();
        }

        [Test]
        public void CreatePngIfFolderNoExists()
        {
            UnityFileUtility.CreatePng(_childPath, _fileName);
            Assert.AreEqual(true, IsFileInPath("png"));
            DeleteUnityFolderUseChild();
        }

        private void CreateAnimationClip() => UnityFileUtility.CreateAnimationClip(_childPath, _fileName);

        private void CreateAnimationOverride() => UnityFileUtility.CreateAnimationOverride(_childPath, _fileName);

        private void DeleteUnityFolderUseChild() => UnityFileUtility.DeleteUnityFolder(_childPath);

        private void CreateUnityFolderUseChild() => UnityFileUtility.CreateUnityFolder(_childPath);

        private bool IsFileInPath(string fileExtension) =>
            CSharpFileUtility.IsFileInPath(_folderPath, _fileName, fileExtension);
    }
}