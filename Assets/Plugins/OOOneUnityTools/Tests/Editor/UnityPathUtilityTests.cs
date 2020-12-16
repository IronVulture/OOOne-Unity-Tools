using NUnit.Framework;
using UnityEngine;

namespace OOOneUnityTools.Editor.Tests
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
                UnityPathUtility.GetUnityAbsoluteFullPath(_childPath, _fileName, extension));
        }

        [Test]
        public static void GetUnityPath()
        {
            UnityFileUtilityTests.ShouldEqualResult("Assets", UnityPathUtility.GetUnityPath());
        }

        [Test]
        public static void GetUnityAbsolutePath()
        {
            UnityFileUtilityTests.ShouldEqualResult(Application.dataPath, UnityPathUtility.GetAbsolutePath());
        }

        [Test]
        public static void GetUnityFolderPath()
        {
            UnityFileUtilityTests.ShouldEqualResult("Assets/" + _childPath,
                UnityPathUtility.GetUnityFolderPath(_childPath));
        }

        [Test]
        public static void GetUnityAbsoluteFolderPath()
        {
            UnityFileUtilityTests.ShouldEqualResult($"{Application.dataPath}/{_childPath}",
                UnityPathUtility.GetUnityAbsoluteFolderPath(_childPath));
        }

        [Test]
        public static void GetUnityFullPath()
        {
            var extension = "overrideController";
            var expected = $"Assets/{_childPath}/{_fileName}.{extension}";
            UnityFileUtilityTests.ShouldEqualResult(expected,
                UnityPathUtility.GetUnityFullPath(_childPath, _fileName, extension));
        }
    }
}