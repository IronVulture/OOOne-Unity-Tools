using System.Collections.Generic;
using NUnit.Framework;
using OOOneUnityTools.Editor;

namespace OOOneUnityTools.Editor.Tests
{
    public class UnityShaderUtilityTests
    {

        [Test]
        [TestCase("Assets/testShaderFolder/myShader.shader")]
        [TestCase("Assets/testShaderFolder/testPBRGraph.shadergraph")]
        [TestCase("Assets/testShaderFolder/testSpriteUnlitGraph.shadergraph")]
        public void GetTexturePropertyNames(string shaderPath)
        {
            List<string> expectedResult = new List<string>();
            expectedResult.Add("_MainTex");
            expectedResult.Add("_SubTex1");
            expectedResult.Add("_SubTex2");
            expectedResult.Add("_Fresnel");
            expectedResult.Add("_BumpMap");

            List<string> texturePropNames = UnityShaderUtility.GetTexturePropertyNames(shaderPath);

            Assert.AreEqual(expectedResult, texturePropNames);
        }

        [Test]
        [TestCase("Assets/testShaderFolder/testNoTexGraph.shadergraph")]
        public void Return_EmptyList_If_No_TextureProperty(string shaderPath)
        {
            List<string> expectedResult = new List<string>();

            List<string> texturePropNames = UnityShaderUtility.GetTexturePropertyNames(shaderPath);

            Assert.AreEqual(expectedResult, texturePropNames);
        }
    }
}