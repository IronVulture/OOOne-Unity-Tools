using NUnit.Framework;
<<<<<<< refs/remotes/origin/feature/在資料夾內找單個或多個指定類型的檔案
using OOOneUnityTools.Editor;
=======
using Plugins.OOOneUnityTools.Editor;
>>>>>>> feat : add FileUtility and Tests
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