using NUnit.Framework;
using OOOneUnityTools.Editor;
using UnityEngine;

namespace OOOneTools.Editor.Tests
{
    public class FileUtilityTests
    {
        [Test]
        public void Get_All_OverrideController_By_Folder()
        {
            var fileUtility = new FileUtility();
            var folderPath  = "Assets/XX";
            var hasFile = fileUtility.TryGetAllOverrideControllerByFolder(folderPath
                                                                          , out AnimatorOverrideController[] overrideControllers);
            Assert.AreEqual(true , hasFile);
            Assert.NotNull(overrideControllers);
        }
    }
}