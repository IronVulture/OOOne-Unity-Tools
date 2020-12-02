namespace OOOne.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// This class helped us quickly assign anim clips into Animator Override controllers based on filename
    /// </summary>
    public class AnimatorOverrideFillIn : MonoBehaviour
    {
        private const string FillClipsFromDirectoryMenuPath = OOOneToolsMenuPaths.AnimationUtilitiesBase +
                                                              OOOneToolsMenuPaths.AnimatorOverrideSubmenu +
                                                              "Fill Clips from Directory";

        /// <summary>
        /// Fill in clips on a selected Animator Controller based on file names
        /// </summary>
        [MenuItem(FillClipsFromDirectoryMenuPath)]
        public static void FillInClips()
        {
            
            string directory = SelectionUtility.GetDirectoryOfSelection();
            // Get all animation clips in the current directory
            List<AnimationClip> clipsForAnimator = new List<AnimationClip>();
            string[] clipGUIDs = AssetDatabase.FindAssets("t:animationClip");
            directory = directory.Replace(@"\",@"/");
            foreach (string clipGUID in clipGUIDs)
            {
                string guidPath = AssetDatabase.GUIDToAssetPath(clipGUID);
                Debug.Log($"guidPath: {guidPath}");
                Debug.Log($"directory: {directory}");
                if (guidPath.Contains(directory))
                 {
                     Debug.Log($"directory: {directory}");

                    clipsForAnimator.Add(AssetDatabase.LoadAssetAtPath<AnimationClip>(guidPath));
                 }

            }

            AnimatorOverrideController overrideController = (AnimatorOverrideController)Selection.activeObject;

            Debug.Log($"clip: {clipsForAnimator.Count}" );
            // Replace clips if they match
            foreach (AnimationClip clip in clipsForAnimator)
            {
                Debug.Log($"clip: {clip}" );
                Debug.Log("test" );
                // Get just the clip name
                string clipAnimName = ExtractAnimNameFromClipName(clip.name);
                Debug.Log("Searching for matches for clip: " + clipAnimName);

                // Find the corresponding clip to replace
                for (int i = 0; i < overrideController.clips.Length; i++)
                {
                    string originalClipName = overrideController.clips[i].originalClip.name;
                    string animName = ExtractAnimNameFromClipName(originalClipName);
                    if (clipAnimName == animName)
                    {
                        Debug.Log("Replacing clip: " + originalClipName + " With: " + clip.name);
                        overrideController[originalClipName] = clip;
                        break;
                    }
                }
            }
        }

        private static string ExtractAnimNameFromClipName(string clipName)
        {
            string[] possiblePrefixes = new string[]
            {
                "Actor_AutoFillReference",
                "Actor_Soldier_Berserker",
                "Actor_Soldier_ShieldBearer",
                "Actor_Soldier_Ranger",
                "Actor_Soldier_Medic",
                "Actor_Soldier_Grenadier",
                "Actor_Soldier_Sniper",
                "Actor_Monster_SmallBug",
                "Actor_Monster_MeatBug",
                "Actor_Monster_NailBug",
                "Actor_Monster_ShieldBug",
                "Actor_Monster_PunchBug",
                "Actor_Monster_QueenBug",
                "Actor_Monster_Squid",
                "Actor_Monster_ShieldClam",
                "Actor_Monster_FloatClam",
                "Actor_Monster_SquidKing",
                "Actor_Monster_CanonWorm",
                "Actor_Monster_WormLord",
                "Actor_Monster_FlyBug",
                "Actor_Monster_SpeedBug",
                "Actor_Monster_HeavyBug",
                "Actor_Monster_KingBug",
                "Actor_Monster_InkBomb",
                "Actor_Monster_SpikeWorm",
                "Actor_Monster_HeavyClam",
                "Actor_Monster_SmallLizard",
                "Actor_Monster_JumpLizard",
                "Actor_Monster_FlameLizard",
                "Actor_Monster_SlugBomb",
                "Actor_Monster_BurningMonkey",
                "Actor_Monster_ResonanceMonkey",
                "Actor_Monster_BigLizard",
                "Actor_Monster_TransformMonkey",
                "Actor_Monster_Mushroom",
                "Actor_Monster_MonkeyLord"
                
            };
            foreach (string prefix in possiblePrefixes)
            {
                if (clipName.Contains(prefix))
                {
                    return clipName.Remove(0, (prefix + "_").Length);
                }
            }

            return string.Empty;
        }

        [MenuItem(FillClipsFromDirectoryMenuPath, true)]
        private static bool IsFillInClipsValid()
        {
            return SelectionUtility.IsActiveObjectOfType<AnimatorOverrideController>();
        }
    }
}