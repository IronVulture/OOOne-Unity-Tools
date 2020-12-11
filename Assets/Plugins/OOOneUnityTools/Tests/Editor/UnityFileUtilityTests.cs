using System.Collections;
using System.IO;
using NUnit.Framework;
using Plugins.OOOneUnityTools.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace OOOneTools.Editor.Tests
{
    public class UnityFileUtilityTests
    {
        private string _childPath;

        [SetUp]
        public void SetUp()
        {
            _childPath = "QWERT";
        }

        [Test]
        public void CreateUnityFolderIfNotExist()
        {
            UnityFileUtility.CreateUnityFolder(_childPath);
            Assert.IsTrue(UnityFileUtility.IsUnityFolderExist(_childPath));
            UnityFileUtility.DeleteUnityFolder(_childPath);
        }

        [Test]
        public void CreateUnityFolderInFolderIfNotExist()
        {
            var path = _childPath + "/" + _childPath;
            UnityFileUtility.CreateUnityFolder(path);
            Assert.IsTrue(UnityFileUtility.IsUnityFolderExist(path));
            UnityFileUtility.DeleteUnityFolder(_childPath);
        }

        [Test]
        public void DontCreateFolderIfExist()
        {
            UnityFileUtility.CreateUnityFolder(_childPath);
            Assert.IsFalse(UnityFileUtility.IsUnityFolderExist(_childPath + " 1"));
            UnityFileUtility.DeleteUnityFolder(_childPath);
        }

        [Test]
        public void DeleteUnityFolder()
        {
            UnityFileUtility.CreateUnityFolder(_childPath);
            UnityFileUtility.DeleteUnityFolder(_childPath);
            Assert.AreEqual(false, UnityFileUtility.IsUnityFolderExist(_childPath));
        }

        [Test]
        public void CreateAnimationOverrideIfNoExists()
        {
            var fileName = "WEjhdfjgh";
            UnityFileUtility.CreateAnimationOverride(_childPath, fileName);
            var folderPath = $@"{Application.dataPath}\{_childPath}";
            var isFileInPath = CSharpFileUtility.IsFileInPath(folderPath, fileName, "overrideController");
            Assert.AreEqual(true, isFileInPath);
            UnityFileUtility.DeleteUnityFolder(_childPath);
        }
    }
}