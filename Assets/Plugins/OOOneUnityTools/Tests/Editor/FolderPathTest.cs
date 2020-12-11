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

        //測試檢查資料夾是否存在
        [Test]
        public void FolderPathExist()
        {
            //預期不存在的folderPath
            FolderShouldNotExist(_folderPathNotExist);
            //預期存在的folderPath
            FolderShouldExist(_ExistingFolderPath);
        }

        //測試創建資料夾
        [Test]
        public void CreateFolderIfNotExist()
        {
            //創建資料夾前資料夾不應該存在
            FolderShouldNotExist(_targetFolderPath);
            _folderPathHandler.CreateFolderIfNotExist(_targetFolderPath);
            //創建資料夾後，資料夾應該存在
            FolderShouldExist(_targetFolderPath);
        }

        //測試刪除資料夾
        [Test]
        public void DeleteTargetFolder()
        {
            //刪除前先創建要刪除的資料夾（兩層）
            _folderPathHandler.CreateFolderIfNotExist(_targetFolder2Delete);
            //刪除前，要刪除的內層資料夾應該要存在
            FolderShouldExist(_targetFolder2Delete);

            _folderPathHandler.DeleteTargetFolder(_targetFolder2Delete);

            //刪除後，要刪除的內層資料夾不應該存在
            FolderShouldNotExist(_targetFolder2Delete);
            //刪除後，沒有要刪除的外層資料夾應該存在
            FolderShouldExist(_targetFolder2DeleteBase);
        }

        //檢查資料夾是否如預期的存在
        private void FolderShouldExist(string folderPath)
        {
            Assert.AreEqual(true, _folderPathHandler.IsFolderExist(folderPath));
        }

        //檢查資料夾是否如預期的不存在
        private void FolderShouldNotExist(string folderPath)
        {
            Assert.AreEqual(false, _folderPathHandler.IsFolderExist(folderPath));
        }

        [TearDown]
        public void TearDown()
        {
            //刪除「測試刪除資料夾」產生的資料夾
            _folderPathHandler.DeleteTargetFolder(_targetFolder2DeleteBase);
            //刪除「測試創建資料夾」產生的資料夾
            _folderPathHandler.DeleteTargetFolder(_targetFolderPath = baseFolderPath + "/Test2");
        }

    }
}