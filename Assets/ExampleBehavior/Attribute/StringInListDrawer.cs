using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

public class StringInList : PropertyAttribute
{
    public delegate string[] GetStringList();

    public StringInList(params string[] list) => List = list;

    public StringInList(Type type , string methodName)
    {
        _type       = type;
        _methodName = methodName;
    }

    private Type     _type;
    private string   _methodName;
    private string[] _list;

    public string[] List
    {
        get
        {
            if (_list != null) return _list;
            var method = _type.GetMethod(_methodName);
            if (method != null) return method.Invoke(null , null) as string[];
            Debug.LogError("NO SUCH METHOD " + _methodName + " FOR " + _type);
            return null;
        }
        set => _list = value;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(StringInList))]
public class StringInListDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position , SerializedProperty property , GUIContent label)
    {
        var stringInList = attribute as StringInList;
        var list         = stringInList.List;
        if (property.propertyType == SerializedPropertyType.String)
        {
            int index = Mathf.Max(0 , Array.IndexOf(list , property.stringValue));
            index                = EditorGUI.Popup(position , property.displayName , index , list);
            property.stringValue = list[index];
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
            property.intValue = EditorGUI.Popup(position , property.displayName , property.intValue , list);
        else base.OnGUI(position , property , label);
    }
}
#endif