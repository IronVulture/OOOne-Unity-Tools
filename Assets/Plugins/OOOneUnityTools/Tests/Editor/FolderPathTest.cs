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

        private void SetUp()
        {
            _folderPathHandler = new FolderPathHandler();
            _targetFolderPath = "Asset/TestFolder";
        }

        [Test]
        public void FolderPathExist()
        {
            SetUp();

            Assert.AreEqual(true, _folderPathHandler.IsFolderExist(_targetFolderPath));
        }

    }
}