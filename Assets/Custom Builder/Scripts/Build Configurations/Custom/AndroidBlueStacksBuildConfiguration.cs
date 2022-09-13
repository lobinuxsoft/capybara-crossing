#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Utilities.Builder
{

    [CreateAssetMenu(fileName = "Android BlueStacks Build Configuration", menuName = "Utilities/Builder Configurations/Custom/Android BlueStacks Build Configuration", order = 1)]
    public class AndroidBlueStacksBuildConfiguration : BaseBuildConfiguration
    {

        public override void CustomBuildConfiguration()
        {
            PlayerSettings.Android.optimizedFramePacing = false;
        }

        public override void ResetBuildConfiguration()
        {
            PlayerSettings.Android.optimizedFramePacing = true;
        }

    }

}

#endif
