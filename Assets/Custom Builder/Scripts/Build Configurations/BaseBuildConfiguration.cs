#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Utilities.Builder
{

    public abstract class BaseBuildConfiguration : ScriptableObject
    {

        [SerializeField] private BuildTarget Target = BuildTarget.StandaloneWindows;
        [SerializeField] private string extension = ".exe";
        [SerializeField] private string folderName = "Windows";

        public BuildOptions buildOptions = default;

        public BuildTarget GetTarget() => Target;

        public string GetExtension() => extension;

        public string GetFolderName() => folderName;

        //public void SetBuildConfigurations() 
        //{
        //    CustomBuildConfiguration();
        //    ResetBuildConfiguration();
        //}

        abstract public void ResetBuildConfiguration();

        abstract public void CustomBuildConfiguration();

    }

}

#endif
