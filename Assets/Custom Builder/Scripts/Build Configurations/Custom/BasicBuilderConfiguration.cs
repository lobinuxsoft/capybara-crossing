#if UNITY_EDITOR

using UnityEngine;

namespace Utilities.Builder
{

    [CreateAssetMenu(fileName = "Basic Build Configuration", menuName = "Utilities/Builder Configurations/Basic Build Configuration", order = 1)]
    public class BasicBuilderConfiguration : BaseBuildConfiguration
    {
        public override void CustomBuildConfiguration()
        {

        }

        public override void ResetBuildConfiguration()
        {

        }
    }

}

#endif
