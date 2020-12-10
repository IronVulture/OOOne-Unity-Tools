using UnityEditor;

namespace OOOneTools.Editor
{
    public class FolderPathHandler
    {
        public bool IsFolderExist(string folderPath)
        {
            return AssetDatabase.IsValidFolder(folderPath);
        }
    }
}