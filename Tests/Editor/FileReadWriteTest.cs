using NUnit.Framework;

namespace OOOneTools.Editor.Tests
{
    public class FileReadWriteTest
    {
        private FileHandler _fileHandler;
        private string _filePathWithExt;

        [SetUp]
        private void SetUp()
        {
            _fileHandler = new FileHandler();
            _filePathWithExt = "Asset/TestFolder/yabai.anim";
        }

        [Test]
        public void FileExist()
        {
            Assert.AreEqual(false, _fileHandler.TryGetFile(_filePathWithExt));
        }


        [Test]
        public void CreateAnimationClipFile()
        {

        }
    }
}
