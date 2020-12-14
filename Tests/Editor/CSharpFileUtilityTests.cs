using NUnit.Framework;

namespace OOOneTools.Editor.Tests
{
    public class CSharpFileUtilityTests
    {
        private string _beforeParsePath;


        [SetUp]
        public void SetUp()
        {
            _beforeParsePath = "Assets/New Folder";
        }

        [Test]
        public void IsFolderExist()
        {

            var isFolderExist = CSharpFileUtility.IsFolderExist(_beforeParsePath);
            Assert.IsTrue(isFolderExist);
        }

        [Test]
        public void ParseSlashToCsharpTest()
        {
            var afterParse = @"Assets\New Folder";
            var actual = CSharpFileUtility.ParseSlashToCsharp(_beforeParsePath);
            Assert.AreEqual(afterParse, actual);
        }

        [Test]
        public void IsFileInPath()
        {
            var isFileInPath = CSharpFileUtility.IsFileInPath(_beforeParsePath, "TestFile", "txt");
            Assert.AreEqual(true, isFileInPath);
        }

    }
}