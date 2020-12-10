using OOOneTools.Editor;
using UnityEngine;

namespace OOOneUnityTools.Editor
{
    public class FileUtility
    {
        public bool TryGetAllOverrideControllerByFolder(string                           folderPath ,
                                                        out AnimatorOverrideController[] animatorOverrideControllers)
        {
            var folderPathHandler = new FolderPathHandler();
            var isFolderExist     = folderPathHandler.IsFolderExist(folderPath);
            animatorOverrideControllers = new AnimatorOverrideController[3];
            return isFolderExist;
        }
    }
}