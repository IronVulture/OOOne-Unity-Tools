using System.IO;
using NUnit.Framework;
using Plugins.OOOneUnityTools.Editor;

namespace OOOneTools.Editor.Tests
{
    public class CSharpFileUtilityTests
    {
        #region Private Variables

        private string _beforeParsePath;
        private string _fileName;
        private string _jpgExtension;
        private string _pngExtension;
        private string _source1_ChildPath;
        private string _source1_PngFullPath;
        private string _source2_ChildPath;
        private string _source2_PngFullPath;
        private string _sourcePathJpg;
        private string _target_JpgFullPath;
        private string _target_PngFullPath;
        private string _targetChildPath;
        private string _targetChildPath2;

        #endregion

        #region Setup/Teardown Methods

        [SetUp]
        public void SetUp()
        {
            _beforeParsePath = "Assets/asdfasdlfja";
            _source1_ChildPath = "asdfasdlfja";
            _targetChildPath = "eedkcvjiosder";
            _source2_ChildPath = "lksdfkj";
            _fileName = "235432asdfasdf";
            _pngExtension = "png";
            _jpgExtension = "jpg";
            _source1_PngFullPath =
                UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_source1_ChildPath, _fileName, _pngExtension);
            _source2_PngFullPath =
                UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_source2_ChildPath, _fileName, _pngExtension);
            _sourcePathJpg = $@"C:\{_source2_ChildPath}\{_fileName}.{_jpgExtension}";
            _target_PngFullPath =
                UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_targetChildPath, _fileName, _pngExtension);
            _target_JpgFullPath =
                UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_targetChildPath, _fileName, _jpgExtension);
        }

        #endregion

        #region Test Methods

        [Test]
        public void IsFolderExist()
        {
            CreateFolderUseTargetPath();
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
            CreateFolderUseTargetPath();
            UnityFileUtility.CreateAssetFile(UnityFileUtility.FileType.Png, _source1_ChildPath, _fileName);
            var isFileInPath = CSharpFileUtility.IsFileInPath(_beforeParsePath, _fileName, _pngExtension);
            Assert.AreEqual(true, isFileInPath);
        }

        [Test]
        public void CopyFile_If_Folder_Exist_And_File_NotExist()
        {
            CreateFolerUsePathSource1();
            CreatePngInFolderSource1();
            CreateFolderUseTargetPath();
            CopyFile(_source1_PngFullPath, _target_PngFullPath);
            ShouldFileEqual(true);
        }

        [Test]
        public void Dont_CopyFile_If_FileNameAndExtension_Is_Same()
        {
            CreateFolerUsePathSource1();
            CreatePngInFolderSource1();
            CreateFolderUsePathSource2();
            CreatePngInFolderSource2();
            CreateFolderUseTargetPath();
            CopyFile(_source1_PngFullPath, _target_PngFullPath);
            CopyFile(_source2_PngFullPath, _target_PngFullPath);
            ShouldFileEqual(true);
        }

        [Test]
        public void CopyFile_If_FileExtension_Is_Not_Same()
        {
            CreateFolderUseTargetPath();
            CopyFile(_source1_PngFullPath, _target_PngFullPath);
            CopyFile(_sourcePathJpg, _target_JpgFullPath);
            ShouldFileEqual(true);
        }

        [Test]
        public void CopyFile_If_Folder_Is_NotExist()
        {
            CopyFile(_source1_PngFullPath, _target_PngFullPath);
            ShouldFileEqual(true);
        }

        [Test]
        public void Not_CopyFile_If_Source_Is_NotExist()
        {
            _source1_PngFullPath = "asdjfk";
            CopyFile(_source1_PngFullPath, _target_PngFullPath);
            Assert.AreEqual(false, File.Exists(_target_PngFullPath));
        }

        #endregion

        #region Public Methods

        [TearDown]
        public void TearDown()
        {
            DeleteFolderSourceAndTarget();
        }

        #endregion

        #region Private Methods

        private void CopyFile(string sourcePath, string targetPath)
        {
            CSharpFileUtility.CopyFile(sourcePath, targetPath);
        }

        private void CreateFolderUsePathSource2()
        {
            UnityFileUtility.CreateUnityFolder(_source2_ChildPath);
        }

        private void CreateFolderUseTargetPath()
        {
            UnityFileUtility.CreateUnityFolder(_targetChildPath);
        }

        private void CreateFolerUsePathSource1()
        {
            UnityFileUtility.CreateUnityFolder(_source1_ChildPath);
        }

        private void CreatePngInFolderSource1()
        {
            UnityFileUtility.CreateAssetFile(UnityFileUtility.FileType.Png, _source1_ChildPath, _fileName);
        }

        private void CreatePngInFolderSource2()
        {
            UnityFileUtility.CreateAssetFile(UnityFileUtility.FileType.Png, _source2_ChildPath, _fileName);
        }

        private void DeleteFolderSourceAndTarget()
        {
            UnityFileUtility.DeleteUnityFolder(_source1_ChildPath);
            UnityFileUtility.DeleteUnityFolder(_source2_ChildPath);
            UnityFileUtility.DeleteUnityFolder(_targetChildPath);
        }

        private void ShouldFileEqual(bool expected)
        {
            Assert.AreEqual(expected, CSharpFileUtility.IsFileAreEqual(_source1_PngFullPath, _target_PngFullPath));
        }

        #endregion
    }
}