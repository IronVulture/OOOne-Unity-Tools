using System.IO;
using UnityEditor;
using UnityEngine;

namespace OOOneTools.Editor
{
    public class FileHandler
    {
        public bool TryGetFile(string filePath)
        {
            //把filePath轉換為系統絕對路徑
            string systemFilePath = AssetPath2FilePath(filePath);

            return (File.Exists(systemFilePath));
        }

        public bool TryGetFile(string fileNameWithExtension, string folderPath)
        {
            //把檔案路徑組起來
            string filePath = $"{folderPath}/{fileNameWithExtension}";
            //把filePath轉換為系統絕對路徑
            string systemFilePath = AssetPath2FilePath(filePath);

            return (File.Exists(systemFilePath));
        }
        public bool TryGetFile(string fileName, string folderPath, string fileNameExtensionWithDot)
        {
            //把檔案路徑組起來
            string filePath = $"{folderPath}/{fileName}{fileNameExtensionWithDot}";
            //把filePath轉換為系統絕對路徑
            string systemFilePath = AssetPath2FilePath(filePath);

            return (File.Exists(systemFilePath));
        }

        public bool CreateAnimationClipFile(string fileName, string folderPath)
        {

            throw new System.NotImplementedException();
        }
        private string AssetPath2FilePath(string assetPath)
        {
            string assetsFolder = "Assets";
            assetPath = assetPath.Substring(assetsFolder.Length);
            string filePath = Application.dataPath + assetPath;
            return filePath;
        }

    }
}