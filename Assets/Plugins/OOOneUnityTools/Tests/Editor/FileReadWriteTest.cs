using System.IO;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace OOOneTools.Editor.Tests
{
    public class FileReadWriteTest
    {
        private FileHandler _fileHandler;
        private string _testCreateFileName, _testCreateFileFolder;
        private string _fileShouldExist, _fileShouldNotExist,
            _fileShouldExistName, _fileShouldExistFolderPath,
            _fileShouldNotExistName, _fileShouldNotExistFolderPath;


        [SetUp]
        public void SetUp()
        {
            _fileHandler = new FileHandler();

            _fileShouldExist = "Assets/FileHandlerTestFolder/FileHandlerTestFolder2/fileShouldExist.anim";
            _fileShouldNotExist = "Assets/FileHandlerTestFolder/FileHandlerTestFolder2/fileShouldNotExist.anim";

            _fileShouldExistName = "fileShouldExist";
            _fileShouldExistFolderPath = "Assets/FileHandlerTestFolder/FileHandlerTestFolder2";

            _fileShouldNotExistName = "fileShouldNotExist";
            _fileShouldNotExistFolderPath = "Assets/FileHandlerTestFolder/FileHandlerTestFolder2";

            _testCreateFileFolder = "Assets/FileHandlerTestFolder/testCreateAnimationFolder";
            _testCreateFileName = "testCreateAnimationFile";

        }

        //測試檔案是否存在
        [Test]
        public void FileExistence()
        {
            FileShouldExist(_fileShouldExist);
            FileShouldNotExist(_fileShouldNotExist);

            FileShouldExist(_fileShouldExistName + ".anim", _fileShouldExistFolderPath);
            FileShouldNotExist(_fileShouldNotExistName + ".anim", _fileShouldNotExistFolderPath);

            FileShouldExist(_fileShouldExistName, _fileShouldExistFolderPath, ".anim");
            FileShouldNotExist(_fileShouldNotExistName, _fileShouldNotExistFolderPath, ".anim");
        }

        [Test]
        public void CreateAnimationClipFile()
        {
            //創建檔案前檔案不應存在
            FileShouldNotExist(_testCreateFileName, _testCreateFileFolder, ".anim");

            //創建檔案
            _fileHandler.CreateAnimationClipFile(_testCreateFileName, _testCreateFileFolder);

            //創建檔案後檔案應存在
            FileShouldExist(_testCreateFileName, _testCreateFileFolder, ".anim");
        }

        //檔案預期應該存在
        private void FileShouldExist(string filePath)
        {
            Assert.AreEqual(true, _fileHandler.TryGetFile(filePath));
        }
        private void FileShouldExist(string fileNameWithExtension, string folderPath)
        {
            Assert.AreEqual(true, _fileHandler.TryGetFile(fileNameWithExtension, folderPath));
        }
        private void FileShouldExist(string fileName, string folderPath, string fileNameExtensionWithDot)
        {
            Assert.AreEqual(true, _fileHandler.TryGetFile(fileName, folderPath, fileNameExtensionWithDot));
        }

        //檔案預期不應存在
        private void FileShouldNotExist(string filePath)
        {
            Assert.AreEqual(false, _fileHandler.TryGetFile(filePath));
        }
        private void FileShouldNotExist(string fileNameWithExtension, string folderPath)
        {
            Assert.AreEqual(false, _fileHandler.TryGetFile(fileNameWithExtension, folderPath));
        }
        private void FileShouldNotExist(string fileName, string folderPath, string fileNameExtensionWithDot)
        {
            Assert.AreEqual(false, _fileHandler.TryGetFile(fileName, folderPath, fileNameExtensionWithDot));
        }
    }
}