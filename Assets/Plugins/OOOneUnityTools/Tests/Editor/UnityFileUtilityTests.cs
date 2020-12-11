using System.Collections;
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
            Assert.IsTrue(IsUnityFolderExist(_childPath));
        }
        [Test]
        public void CreateUnityFolderInFolderIfNotExist()
        {
            var path = _childPath+"/"+_childPath;
            UnityFileUtility.CreateUnityFolder(path);
            Assert.IsTrue(IsUnityFolderExist(path));
        }

        [Test]
        public void DontCreateFolderIfExist()
        {
            UnityFileUtility.CreateUnityFolder(_childPath);
            Assert.IsFalse(IsUnityFolderExist(_childPath + " 1"));
        }

        private bool IsUnityFolderExist(string childPath)
        {
            return CSharpFileUtility.IsFolderExist("Assets/" + childPath);
        }
    }
}