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
            List<string> texturePropertyNames = new List<string>();
            var shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderFullPath);
            if (shader == null)
            {
                return texturePropertyNames;
            }
            int propertyCount = shader.GetPropertyCount();
            for (int i = 0; i < propertyCount; i++)
            {
                //注意：Texture3D和CubeMap都算是Texture
                if (shader.GetPropertyType(i) == ShaderPropertyType.Texture)
                {
                    texturePropertyNames.Add(shader.GetPropertyName(i));
                }
            }
            return texturePropertyNames;
        }
    }
}