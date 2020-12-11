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
        private string _parentFolderPath;
        private string _childPath;
        private string _fullPath;

        [SetUp]
        public void SetUp()
        {
            _parentFolderPath = "Assets";
            _childPath = "QWERT";
            _fullPath = _parentFolderPath + "/" + _childPath;
        }

        [Test]
        public void CreateUnityFolderIfNotExistTests()
        {
            UnityFileUtility.CreateUnityFolder(_parentFolderPath, _childPath);
            Assert.IsTrue(CSharpFileUtility.IsFolderExist(_fullPath));
        }

        [Test]
        public void DontCreateFolderIfExistTest()
        {
            UnityFileUtility.CreateUnityFolder(_parentFolderPath, _childPath);
            Assert.IsFalse(CSharpFileUtility.IsFolderExist(_fullPath + " 1"));

        }
    }
}