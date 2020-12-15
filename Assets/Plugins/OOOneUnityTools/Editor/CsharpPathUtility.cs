using OOOneTools.Editor;
using UnityEngine;

namespace Plugins.OOOneUnityTools.Editor
{
    public class CsharpPathUtility
    {
        public static string GetCsharpUnityAbsoluteFolderPath(string childPath)
        {
            return CSharpFileUtility.ParseSlashToCsharp(Application.dataPath + "/" + childPath);
        }
    }
}