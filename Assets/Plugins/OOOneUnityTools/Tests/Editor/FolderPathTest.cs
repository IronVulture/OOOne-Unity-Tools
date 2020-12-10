using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace OOOneTools.Editor.Tests
{
    public class FolderPathTest
    {
        private string _targetFolderPath;
        private FolderPathHandler _folderPathHandler;
        private string _folderPathNotExist;
        private string _ExistingFolderPath;
        private string _targetFolder2DeleteBase, _targetFolder2Delete;
        private const string baseFolderPath = "Assets";

        [SetUp]
        public void SetUp()
        {
            _folderPathHandler = new FolderPathHandler();
            _targetFolderPath = baseFolderPath + "/Test2/Test3";
            _folderPathNotExist = baseFolderPath + "/ThereIsNoFolder";
            _ExistingFolderPath = baseFolderPath + "/Plugins";
            _targetFolder2DeleteBase = baseFolderPath + "/Folder2Delete1";
            _targetFolder2Delete = baseFolderPath + "/Folder2Delete1/Folder2Delete2";
        }

        [Test]
        public void FolderPathExist()
        {
            //預期不存在的folderPath
            Assert.AreEqual(false, _folderPathHandler.IsFolderExist(_folderPathNotExist));
            //預期存在的folderPath
            Assert.AreEqual(true, _folderPathHandler.IsFolderExist(_ExistingFolderPath));
        }

        [Test]
        public void CreateFolderIfNotExist()
        {
            Assert.AreEqual(false, _folderPathHandler.IsFolderExist(_targetFolderPath));
            _folderPathHandler.CreateFolderIfNotExist(_targetFolderPath);
            Assert.AreEqual(true, _folderPathHandler.IsFolderExist(_targetFolderPath));
        }

        [Test]
        public void DeleteTargetFolder()
        {
            _folderPathHandler.CreateFolderIfNotExist(_targetFolder2Delete);
            Assert.AreEqual(true, _folderPathHandler.IsFolderExist(_targetFolder2Delete));
            _folderPathHandler.DeleteTargetFolder(_targetFolder2Delete);
            Assert.AreEqual(false, _folderPathHandler.IsFolderExist(_targetFolder2Delete));
            Assert.AreEqual(true, _folderPathHandler.IsFolderExist(_targetFolder2DeleteBase));
        }

        [TearDown]
        public void TearDown()
        {
            _folderPathHandler.DeleteTargetFolder(_targetFolder2DeleteBase);
            _folderPathHandler.DeleteTargetFolder(_targetFolderPath = baseFolderPath + "/Test2");
        }

    }
}