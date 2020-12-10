using System.IO;
using UnityEditor;
using UnityEngine;

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
            /*因為CreateFolder的parentFolder參數
            必須為"Assets/"開頭，故直接跳過i=0的情況*/
            string rebuiltFolderPath = "Assets";
            for (int i = 1; i < folderNames.Length; i++)
            {
                if(!IsFolderExist(Path.Combine(rebuiltFolderPath, folderNames[i]) ) )
                    AssetDatabase.CreateFolder(rebuiltFolderPath, folderNames[i]);

                rebuiltFolderPath = Path.Combine(rebuiltFolderPath, folderNames[i]);
            }
        }

        public bool DeleteTargetFolder(string targetFolder2Delete)
        {
            return AssetDatabase.DeleteAsset(targetFolder2Delete);
        }
    }
}