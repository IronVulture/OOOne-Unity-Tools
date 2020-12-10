using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace OOOneTools.Editor.Tests
{
    public class FolderPathTest
    {
        [Test]
        public void FolderPathExist()
        {
            FolderPathHandler folderPathHandler = new FolderPathHandler();
            Assert.AreEqual(true, folderPathHandler.IsFolderExist());
        }


    }
}