using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class PropertyDrawersHelper
{
#if UNITY_EDITOR

    public static string[] AllSceneNames()
    {
        var temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0 , name.Length - 6);
                temp.Add(name);
            }
        }

        return temp.ToArray();
    }

    public static string[] AllNames()
    {
        List<NameListSO> nameListSos = new List<NameListSO>();
        var              type        = typeof(NameListSO);
        string[]         guids2      = AssetDatabase.FindAssets($"t:{type}");
        foreach (string guid2 in guids2)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid2);
            nameListSos.Add((NameListSO)AssetDatabase.LoadAssetAtPath(assetPath , type));
        }

        var nameListSo = nameListSos.First();
        return nameListSo.IDNames.Select(_ => _.Name).ToArray();
    }

#endif
}