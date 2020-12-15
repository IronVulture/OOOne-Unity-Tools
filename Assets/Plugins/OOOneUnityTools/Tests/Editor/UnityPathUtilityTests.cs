using NUnit.Framework;
using Plugins.OOOneUnityTools.Editor;
using UnityEngine;

namespace OOOneTools.Editor.Tests
{
    public class UnityPathUtilityTests
    {
        private static string _childPath;
        private static string _fileName;

        [SetUp]
        public void SetUp()
        {
            _childPath = "SDFJHKL/SDFjkl";
            _fileName = "asdfeervsdklfjol";
        }

        [Test]
        public static void GetUnityAbsoluteFullPath()
        {
            var extension = "overrideController";
            var expected = $"{Application.dataPath}/{_childPath}/{_fileName}.{extension}";
            UnityFileUtilityTests.ShouldEqualResult(expected,
                UnityFileUtility.GetUnityAbsoluteFullPath(_childPath, _fileName, extension));
        }

        [Test]
        public static void GetUnityPath()
        {
            UnityFileUtilityTests.ShouldEqualResult("Assets", UnityFileUtility.GetUnityPath());
        }

        [Test]
        public static void GetUnityAbsolutePath()
        {
            UnityFileUtilityTests.ShouldEqualResult(Application.dataPath, UnityFileUtility.GetAbsolutePath());
        }

        [Test]
        public static void GetUnityFolderPath()
        {
            UnityFileUtilityTests.ShouldEqualResult("Assets/" + _childPath,
                UnityFileUtility.GetUnityFolderPath(_childPath));
        }

        [Test]
        public static void GetUnityAbsoluteFolderPath()
        {
            UnityFileUtilityTests.ShouldEqualResult($"{Application.dataPath}/{_childPath}",
                UnityFileUtility.GetUnityAbsoluteFolderPath(_childPath));
        }

        [Test]
        public static void GetUnityFullPath()
        {
            var extension = "overrideController";
            var expected = $"Assets/{_childPath}/{_fileName}.{extension}";
            UnityFileUtilityTests.ShouldEqualResult(expected,
                UnityFileUtility.GetUnityFullPath(_childPath, _fileName, extension));
        }
    }
}