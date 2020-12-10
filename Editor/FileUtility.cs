using UnityEngine;

namespace OOOneUnityTools.Editor
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