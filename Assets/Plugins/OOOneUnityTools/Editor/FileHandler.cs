using UnityEditor;
using UnityEngine;

namespace OOOneTools.Editor
{
    public class FileHandler
    {
        public bool TryGetFile(string filePath)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
            return (obj != null);
        }
    }
}