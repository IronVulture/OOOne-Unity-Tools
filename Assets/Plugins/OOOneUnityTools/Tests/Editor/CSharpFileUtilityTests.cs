using System.Collections;
using NUnit.Framework;
using UnityEditor;
using OOOne.Tools.Editor;

namespace OOOneTools.Editor.Tests
{
    public class CSharpFileUtilityTests
    {
        [Test]
        public void ParseSlashToCsharpTest()
        {
            var beforeParse = "Assets/New Folder";
            var afterParse = @"Assets\New Folder";
            var actual = CSharpFileUtility.ParseSlashToCsharp(beforeParse);
            Assert.AreEqual(afterParse, actual);
        }

        [Test]
        public void IsFolderExist()
        {
            var path = "Assets/New Folder";
            var isFolderExist = CSharpFileUtility.IsFolderExist(path);
            Assert.IsTrue(isFolderExist);
        }
    }
}