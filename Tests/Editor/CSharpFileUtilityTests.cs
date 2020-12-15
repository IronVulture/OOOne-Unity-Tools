using System.IO;
using NUnit.Framework;
using Plugins.OOOneUnityTools.Editor;

namespace OOOneTools.Editor.Tests
{
    public class CSharpFileUtilityTests
    {
        #region Private Variables

        private string _beforeParsePath;
        private string _sourceChildPath;
        private string _fileName;
        private string _pngExtension;
        private string _sourcePath;
        private string _sourcePathPng;
        private string _targetPath;
        private string _sourceChildPath2;
        private string _targetPathJpg;
        private string _sourcePathJpg;
        private string _jpgExtension;
        private string _targetChildPath;
        private string _targetChildPath2;

        #endregion

        #region Setup/Teardown Methods

        [SetUp]
        public void SetUp()
        {
            _beforeParsePath = "Assets/asdfasdlfja";
            _sourceChildPath = "asdfasdlfja";
            _targetChildPath = "eedkcvjiosder";
            _sourceChildPath2 = "lksdfkj";
            _fileName = "235432asdfasdf";
            _pngExtension = "png";
            _jpgExtension = "jpg";
            _sourcePath = $@"C:\{_sourceChildPath}\{_fileName}.{_pngExtension}";
            _sourcePathPng = $@"C:\{_sourceChildPath2}\{_fileName}.{_pngExtension}";
            _sourcePathJpg = $@"C:\{_sourceChildPath2}\{_fileName}.{_jpgExtension}";
            _targetPath = UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_targetChildPath, _fileName, _pngExtension);
            _targetPathJpg = UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_targetChildPath, _fileName, _jpgExtension);
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
            UnityFileUtility.CreateAssetFile(UnityFileUtility.FileType.Png, _sourceChildPath, _fileName);
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
            CopyFile(_sourcePathPng, _targetPath);
            ShouldFileEqual(true);
        }

        [Test]
        public void CopyFile_If_FileExtension_Is_Not_Same()
        {
            CreateFolderUseChildPath();
            CopyFile(_sourcePath, _targetPath);
            CopyFile(_sourcePathJpg, _targetPathJpg);
            ShouldFileEqual(true);
        }
        [Test]
        public void CopyFile_If_Folder_Is_NotExist()
        {
            CopyFile(_sourcePath, _targetPath);
            ShouldFileEqual(true);
        }

        [Test]
        public void Not_CopyFile_If_Source_Is_NotExist()
        {
            _sourcePath = "asdjfk";
            CopyFile(_sourcePath, _targetPath);
            Assert.AreEqual( false , File.Exists(_targetPath) );
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
            Directory.CreateDirectory(UnityPathUtility.GetCsharpUnityAbsoluteFolderPath(_sourceChildPath));
        }

        private void DeleteFolderUseChildPath()
        {
            UnityFileUtility.DeleteUnityFolder(_targetChildPath);
        }

        private void ShouldFileEqual(bool expected)
        {
            Assert.AreEqual(expected, CSharpFileUtility.IsFileAreEqual(_sourcePath, _targetPath));
        }

        #endregion
    }
}