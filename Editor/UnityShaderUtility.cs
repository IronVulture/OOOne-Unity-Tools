using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace OOOneUnityTools.Editor
{
    public class UnityShaderUtility
    {
        public static List<string> GetTexturePropertyNames(string shaderFullPath)
        {
            var shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderFullPath);
            List<string> texturePropertyNames = new List<string>();
            int propertyCount = shader.GetPropertyCount();
            for (int i = 0; i < propertyCount; i++)
            {
                if (shader.GetPropertyType(i) == ShaderPropertyType.Texture)
                {
                    texturePropertyNames.Add(shader.GetPropertyName(i));
                }
            }
            return texturePropertyNames;
        }
    }
}