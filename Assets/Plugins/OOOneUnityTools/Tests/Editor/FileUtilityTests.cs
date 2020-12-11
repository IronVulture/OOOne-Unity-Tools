#region

using NUnit.Framework;
using OOOne.Tools.Editor;

#endregion

namespace OOOneTools.Editor.Tests
{
    public class FileUtilityTests
    {
        private FileUtility _fileUtility;

        [SetUp]
        public void SetUp()
        {
            _fileUtility = new FileUtility();
        }

        [Test]
        public void Get_All_OverrideController_By_Folder()
        {
            var folderPath = "Assets/TTTT";
            var hasFile = _fileUtility.TryGetAllOverrideControllerByFolder(folderPath
                                                                           , out var overrideControllers);
            Assert.AreEqual(true , hasFile);
            Assert.NotNull(overrideControllers);
        }
    }
}