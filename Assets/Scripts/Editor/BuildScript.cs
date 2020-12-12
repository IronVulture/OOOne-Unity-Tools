namespace OOOneBuildScript
{
    using UnityEditor;

    class BuildScript
    {
        [MenuItem("MyTools/Switch Build")]
        static void BuildGame()
        {
            // Get filename.
            string[] scenes = new string[] {"Assets/Scenes/SampleScence.unity"};

#if UNITY_EDITOR
            // Build player.
            BuildPipeline.BuildPlayer(scenes, "./builds/OOOneTools", BuildTarget.Switch, BuildOptions.Development);
#endif
        }
    }
}