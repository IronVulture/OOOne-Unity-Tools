using UnityEngine;

<<<<<<< refs/remotes/origin/feature/在資料夾內找單個或多個指定類型的檔案
namespace OOOneUnityTools.Editor
=======
namespace Plugins.OOOneUnityTools.Editor
>>>>>>> feat : add FileUtility and Tests
{
    public class FileUtility
    {
        public bool TryGetAllOverrideControllerByFolder(string folderPath , out AnimatorOverrideController[] animatorOverrideControllers)
        {
            animatorOverrideControllers = new AnimatorOverrideController[3];
            return true;
        }
    }
}