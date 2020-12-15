using System.IO;
using NUnit.Framework;
using Plugins.OOOneUnityTools.Editor;

namespace OOOneTools.Editor.Tests
{
    public class CSharpFileUtilityTests
    {
        private string _beforeParsePath;
        private string _childPath;
        private string _fileName;


        [SetUp]
        public void SetUp()
        {
            _beforeParsePath = "Assets/asdfasdlfja";
            _childPath = "asdfasdlfja";
            _fileName = "235432asdfasdf";
        }

        [Test]
        public void IsFolderExist()
        {

            Directory.CreateDirectory(UnityPathUtility.GetCsharpUnityAbsoluteFolderPath(_childPath));
            var isFolderExist = CSharpFileUtility.IsFolderExist(_beforeParsePath);
            Assert.IsTrue(isFolderExist);
            UnityFileUtility.DeleteUnityFolder(_childPath);
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
            Directory.CreateDirectory(UnityPathUtility.GetCsharpUnityAbsoluteFolderPath(_childPath));
            UnityFileUtility.CreateAssetFile(UnityFileUtility.FileType.Png, _childPath, _fileName);
            const string extension = "png";
            var isFileInPath = CSharpFileUtility.IsFileInPath(_beforeParsePath, _fileName, extension);
            Assert.AreEqual(true, isFileInPath);
        }

        // [Test]
        // public void CopyFile_If_Folder_Exist_And_File_NotExist()
        // {
        //     string pathA = "";
        //     string pathB = "";
        //     Assert.AreEqual( true , CSharpFileUtility.IsFileAreEqual(pathA, pathB) );
        // }

    }
}