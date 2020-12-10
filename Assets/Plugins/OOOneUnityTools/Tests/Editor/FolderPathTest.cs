using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace OOOneTools.Editor.Tests
{
    public class FolderPathTest
    {
        private string _targetFolderPath;
        private FolderPathHandler _folderPathHandler;

        [SetUp]
        private void SetUp()
        {
            _folderPathHandler = new FolderPathHandler();
            _targetFolderPath = "Asset/TestFolder";
        }

        [Test]
        public void FolderPathExist()
        {
            Assert.AreEqual(true, _folderPathHandler.IsFolderExist(_targetFolderPath));
        }

        [Test]
        public void CreateFolderIfNotExist()
        {
            Assert.AreEqual(false, _folderPathHandler.IsFolderExist(_targetFolderPath));
            _folderPathHandler.CreateFolderIfNotExist(_targetFolderPath);
            Assert.AreEqual(true, _folderPathHandler.IsFolderExist(_targetFolderPath));
        }

    }
}