using System.IO;
using NUnit.Framework;
using Plugins.OOOneUnityTools.Editor;

namespace OOOneTools.Editor.Tests
{
    public class CSharpFileUtilityTests
    {
        #region Private Variables

        private string _beforeParsePath;
        private string _childPath;
        private string _fileName;
        private string _pngExtension;
        private string _sourcePath;
        private string _sourcePathSecond;
        private string _targetPath;
        private string _whiteCatPath;

        #endregion

        #region Setup/Teardown Methods

        [SetUp]
        public void SetUp()
        {
            _beforeParsePath = "Assets/asdfasdlfja";
            _childPath = "asdfasdlfja";
            _whiteCatPath = "lksdfkj";
            _fileName = "235432asdfasdf";
            _pngExtension = "png";
            _sourcePath = @"C:\" + _childPath + @"\" + _fileName + "." + _pngExtension;
            _targetPath = UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_childPath, _fileName, _pngExtension);
            _sourcePathSecond = @"C:\" + _whiteCatPath + @"\" + _fileName + "." + _pngExtension;
        }

        #endregion

        #region Test Methods

        [Test]
        public void IsFolderExist()
        {
            CreateFolderUseChildPath();
            var isFolderExist = CSharpFileUtility.IsFolderExist(_beforeParsePath);
            Assert.IsTrue(isFolderExist);
        }

        [Test]
        public void ParseSlashToCsharpTest()
        {
            var afterParse = @"Assets\asdfasdlfja";
            var actual = CSharpFileUtility.ParseSlashToCsharp(_beforeParsePath);
            Assert.AreEqual(afterParse, actual);
        }

        [Test]
        public void IsFileInPath()
        {
            CreateFolderUseChildPath();
            UnityFileUtility.CreateAssetFile(UnityFileUtility.FileType.Png, _childPath, _fileName);
            var isFileInPath = CSharpFileUtility.IsFileInPath(_beforeParsePath, _fileName, _pngExtension);
            Assert.AreEqual(true, isFileInPath);
        }

        [Test]
        public void CopyFile_If_Folder_Exist_And_File_NotExist()
        {
            CreateFolderUseChildPath();
            CopyFile(_sourcePath, _targetPath);
            ShouldFileEqual(true);
        }

        [Test]
        public void Dont_CopyFile_If_FileNameAndExtension_Is_Same()
        {
            CreateFolderUseChildPath();
            CopyFile(_sourcePath, _targetPath);
            CopyFile(_sourcePathSecond, _targetPath);
            ShouldFileEqual(true);
        }

        [Test]
        public void CopyFile_If_FileExtension_Is_Not_Same()
        {
            CreateFolderUseChildPath();

            var sourcePath = @"C:\" + _childPath + @"\" + _fileName + "." + _pngExtension;
            var targetPath = UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_childPath, _fileName, _pngExtension);
            CSharpFileUtility.CopyFile(sourcePath, targetPath);

            var targetPath2 = UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_childPath, _fileName, "jpg");
            var sourcePathSecond = @"C:\" + _whiteCatPath + @"\" + _fileName + "." + "jpg";
            CSharpFileUtility.CopyFile(sourcePathSecond, targetPath2);
            Assert.AreEqual(true, CSharpFileUtility.IsFileAreEqual(sourcePath, targetPath));
        }

        #endregion

        #region Public Methods

        [TearDown]
        public void TearDown()
        {
            DeleteFolderUseChildPath();
        }

        #endregion

        #region Private Methods

        private void CopyFile(string sourcePath, string targetPath)
        {
            CSharpFileUtility.CopyFile(sourcePath, targetPath);
        }

        private void CreateFolderUseChildPath()
        {
            Directory.CreateDirectory(UnityPathUtility.GetCsharpUnityAbsoluteFolderPath(_childPath));
        }

        private void DeleteFolderUseChildPath()
        {
            UnityFileUtility.DeleteUnityFolder(_childPath);
        }

        private void ShouldFileEqual(bool expected)
        {
            Assert.AreEqual(expected, CSharpFileUtility.IsFileAreEqual(_sourcePath, _targetPath));
        }

        #endregion
    }
}