using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Plugins.OOOneUnityTools.Tests.Editor;
using UnityEngine;
using UnityEngine.TestTools;

namespace OOOneTools.Editor.Tests
{
    public class FileReadWriteTest
    {
        [Test]
        public void FileExist()
        {
            var fileHandler = new FileHandler();
            Assert.AreEqual(false, fileHandler.TryGetFile());
        }
    }
}
