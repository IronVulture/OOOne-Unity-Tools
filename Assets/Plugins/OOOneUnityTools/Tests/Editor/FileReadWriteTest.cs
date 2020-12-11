using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace OOOneTools.Editor.Tests
{
    public class FileReadWriteTest
    {
        private FileHandler _fileHandler;
        private string _filePathWithExt;
        private string _fileShouldExist, _fileShouldNotExist;

        [SetUp]
        public void SetUp()
        {
            _fileHandler = new FileHandler();
            _fileShouldExist = "Assets/FileHandlerTestFolder/FileHandlerTestFolder2/fileShouldExist.anim";
            _fileShouldNotExist = "Assets/FileHandlerTestFolder/FileHandlerTestFolder2/fileShouldNotExist.anim";
            _filePathWithExt = "Asset/TestFolder/yabai.anim";
        }

        //測試檔案是否存在
        [Test]
        public void FileExistence()
        {
            FileShouldExist(_fileShouldExist);
            FileShouldNotExist(_fileShouldNotExist);
        }


        [Test]
        public void CreateAnimationClipFile()
        {
        }

        //檔案預期應該存在
        private void FileShouldExist(string filePath)
        {
            Assert.AreEqual(true, _fileHandler.TryGetFile(filePath));
        }

        //檔案預期不應存在
        private void FileShouldNotExist(string filePath)
        {
            Assert.AreEqual(false, _fileHandler.TryGetFile(filePath));
        }
    }
}