using UnityEditor;

public static class BuildScript 
{
    [MenuItem("MyTools/Switch Build")]
    public static void BuildGame ()
    {
        // Get filename.
        string[] scenes = new string[] {"Assets/Scenes/SampleScence.unity"};

        // Build player.
        BuildPipeline.BuildPlayer(scenes, "./builds/OOOneTools", BuildTarget.Switch, BuildOptions.Development);
        
        
    }
}
