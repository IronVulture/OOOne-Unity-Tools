using System;
using UnityEditor;
using UnityEngine;

namespace Tests
{
    public class FileManager
    {
        public bool IsFileExist() => true;

        public bool IsFileExist(string fullPath)
        {
            var pathManager   = new PathManager();
            var fileManager   = new FileManager();
            var isFolderExist = pathManager.IsFolderExist();
            var isFileExist   = fileManager.IsFileExist();
            if (isFolderExist)
            {
                if (isFileExist)
                {
                    if (AssetDatabase.LoadAssetAtPath<AnimationClip>(fullPath) != null)
                    {
                        return true;
                    }
                }
            }


            return false;
        }

        public void CreateFile(string fullPath)
        {
            var instance = Activator.CreateInstance<AnimationClip>();
            var obj      = instance as UnityEngine.Object;
            AssetDatabase.CreateAsset(obj , fullPath);
        }

        public void CreateAsset(string path)
        {

        }
    }
}