using UnityEditor;

namespace OOOneTools.Editor
{
    public class FolderPathHandler
    {
        public bool IsFolderExist(string folderPath)
        {
            return AssetDatabase.IsValidFolder(folderPath);
        }

        public void CreateFolderIfNotExist(string targetFolderPath)
        {
            string[] folderNames = targetFolderPath.Split('/');
            string rebuiltFolderPath = "";
            for (int i = 0; i < folderNames.Length; i++)
            {
                if (i != 0)
                    rebuiltFolderPath += "/";
                if(!IsFolderExist(rebuiltFolderPath + folderNames[i]))
                AssetDatabase.CreateFolder(rebuiltFolderPath, folderNames[i]);


            }

        }

    }
}