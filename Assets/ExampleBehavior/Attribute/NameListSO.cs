using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NameListSO" , menuName = "NameListSO" , order = 0)]
public class NameListSO : ScriptableObject
{
    public List<IDName> IDNames = new List<IDName>();
}

[System.Serializable]
public class IDName
{
    public int    ID;
    public string Name;
}

