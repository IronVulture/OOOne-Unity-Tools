using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class ToolTests
    {
        [TestCase()]
        public void CreateAnim()
        {
            var fileManager = new FileManager();
            var    folderPath = "Assets/Apple";
            var    fileName   = "ApplePie.anim";
            string fullPath   = Path.Combine(folderPath , fileName);

            fileManager.CreateAsset(fullPath);
            fileManager.CreateFile(fullPath);
            var    fileExist  = fileManager.IsFileExist(fullPath);
            Assert.AreEqual(true , fileExist);
        }
    }
}