#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Utilities.Builder
{
    [CreateAssetMenu(fileName = "App Bundle Build Configuration", menuName = "Utilities/Builder Configurations/Custom/App Bundle Build Configuration", order = 1)]
    public class AppBundleBuildConfiguration : BaseBuildConfiguration
    {
        public override void CustomBuildConfiguration()
        {
            PlayerSettings.Android.bundleVersionCode++;
            EditorUserBuildSettings.buildAppBundle = true;
        }

        public override void ResetBuildConfiguration()
        {
            EditorUserBuildSettings.buildAppBundle = false;
        }
    }
}

#endif