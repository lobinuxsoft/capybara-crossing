#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Builder
{

    [CreateAssetMenu(fileName = "Builder", menuName = "Utilities/Builder", order = 1)]

    public class Builder : ScriptableObject
    {
        public string ProjectName = default;
        public BaseBuildConfiguration[] ReleaseBuilds = default;
        public BaseBuildConfiguration[] MajorBuilds = default;
        public BaseBuildConfiguration[] MinorBuilds = default;

        [HideInInspector] public bool ManualVersionOverride = false;
        [HideInInspector][SerializeField] private int releaseVersion = 0;
        [HideInInspector][SerializeField] private int majorPatchVersion = 1;
        [HideInInspector][SerializeField] private int minorPatchVersion = 0;
        [HideInInspector][SerializeField] private int bundleVersion = 1;

        [HideInInspector] public bool useGlobalDefineSymbols = default;
        [HideInInspector][SerializeField] private string[] globalDefineSymbols = default;


        [HideInInspector] public Color logColor = Color.white;
        [HideInInspector] public Color warnningColor = Color.yellow;
        [HideInInspector] public Color errorColor = Color.red;
        [HideInInspector] public Color assertColor = Color.red + Color.yellow;
        [HideInInspector] public Color exceptionColor = Color.magenta;
        [HideInInspector] public Color stackTraceColor = Color.cyan;

        List<LogStruct> buildLogs = new List<LogStruct>();

        public List<LogStruct> BuildLogs
        {
            get => buildLogs;
            set => buildLogs = value;
        }

        public int ReleaseVersion => releaseVersion;
        public int MajorPatchVersion => majorPatchVersion;
        public int MinorPatchVersion => minorPatchVersion;
        public int BundleVersion => bundleVersion;

        public bool Building { get; private set; }

        public int callbackOrder => throw new System.NotImplementedException();

        public string GetReleaseFolderName() => ProjectName + " " + (releaseVersion + 1).ToString() + ".0.0";
        public string GetMajorFolderName() => ProjectName + " " + releaseVersion + "." + (majorPatchVersion + 1).ToString() + ".0";
        public string GetMinorFolderName() => ProjectName + " " + releaseVersion + "." + majorPatchVersion + "." + (minorPatchVersion + 1).ToString();

        public void BuildAllReleaseBuilds(string[] levels, string path) 
        {
            Application.logMessageReceived += OnLogMessageReceived;

            BuildTarget currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;

            Building = true;

            releaseVersion++;
            minorPatchVersion = 0;
            majorPatchVersion = 0;
            PlayerSettings.bundleVersion = releaseVersion + "." + majorPatchVersion + "." + minorPatchVersion;

            foreach (var config in ReleaseBuilds)
            {
                if(config) BuildConfigurationTask(config, levels, path, ProjectName);
            }

            bundleVersion = PlayerSettings.Android.bundleVersionCode;
            Building = false;

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(currentBuildTarget), currentBuildTarget);

            Application.logMessageReceived -= OnLogMessageReceived;
        }

        public void BuildAllMajorBuilds(string[] levels, string path)
        {
            Application.logMessageReceived += OnLogMessageReceived;

            BuildTarget currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;

            Building = true;

            majorPatchVersion++;
            minorPatchVersion = 0;
            PlayerSettings.bundleVersion = releaseVersion + "." + majorPatchVersion + "." + minorPatchVersion;

            foreach (var config in MajorBuilds)
            {
                if (config) BuildConfigurationTask(config, levels, path, ProjectName);
            }

            bundleVersion = PlayerSettings.Android.bundleVersionCode;
            Building = false;

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(currentBuildTarget), currentBuildTarget);

            Application.logMessageReceived -= OnLogMessageReceived;
        }

        public void BuildAllMinorBuilds(string[] levels, string path)
        {
            Application.logMessageReceived += OnLogMessageReceived;

            BuildTarget currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;

            Building = true;

            minorPatchVersion++;
            PlayerSettings.bundleVersion = releaseVersion + "." + majorPatchVersion + "." + minorPatchVersion;

            foreach (var config in MinorBuilds)
            {
                if (config) BuildConfigurationTask(config, levels, path, ProjectName);
            }

            bundleVersion = PlayerSettings.Android.bundleVersionCode;
            Building = false;

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(currentBuildTarget), currentBuildTarget);

            Application.logMessageReceived -= OnLogMessageReceived;
        }

        private void BuildConfigurationTask(BaseBuildConfiguration config, string[] levels, string path, string gameName) 
        {
            if (useGlobalDefineSymbols)
                EnableSymbols();
            else
                DisableSymbols();

            bool preparedToBuild = EditorUserBuildSettings.activeBuildTarget == config.GetTarget();

            if (!preparedToBuild) 
            {
                preparedToBuild = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(config.GetTarget()), config.GetTarget());
            }

            if (preparedToBuild) 
            {
                //config.SetBuildConfigurations();
                config.CustomBuildConfiguration();

                string folderPath = path + "/" + config.GetFolderName();

                var result = BuildGame(levels, folderPath, gameName + config.GetExtension(), config.GetTarget(), config.buildOptions);

                switch (result)
                {
                    case UnityEditor.Build.Reporting.BuildResult.Unknown:
                        buildLogs.Add(new LogStruct { condition = $"Platform: {config.GetTarget()} Unknown Result", logType = LogType.Exception, stackTrace = "" });
                        break;
                    case UnityEditor.Build.Reporting.BuildResult.Succeeded:
                        buildLogs.Add(new LogStruct { condition = $"Platform: {config.GetTarget()} Succeeded!", logType = LogType.Log, stackTrace = "" });
                        break;
                    case UnityEditor.Build.Reporting.BuildResult.Failed:
                        buildLogs.Add(new LogStruct { condition = $"Platform: {config.GetTarget()} Failed", logType = LogType.Error, stackTrace = "" });
                        break;
                    case UnityEditor.Build.Reporting.BuildResult.Cancelled:
                        buildLogs.Add(new LogStruct { condition = $"Platform: {config.GetTarget()} Canceled", logType = LogType.Exception, stackTrace = "" });
                        break;
                }

                config.ResetBuildConfiguration();
            }
        }

        private UnityEditor.Build.Reporting.BuildResult BuildGame(string[] levels, string path, string gameName, BuildTarget target, BuildOptions options)
        {
            UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(levels, path + "/" + gameName, target, options);
            return report.summary.result;
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            buildLogs.Add(new LogStruct { logType = type, condition = condition, stackTrace = stackTrace });
        }

        private List<string> GetCurrentDefines()
        {
            List<string> currentDefines;

            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            currentDefines = defines.Split(';').ToList();

            return currentDefines;
        }

        private void SetCurrentDefines(List<string> currentDefines)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = string.Join(";", currentDefines);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }

        public void EnableSymbols()
        {
            List<string> currentDefines = GetCurrentDefines();

            foreach (string define in globalDefineSymbols)
            {
                if (!currentDefines.Contains(define))
                    currentDefines.Add(define);
            }

            SetCurrentDefines(currentDefines);
        }

        public void DisableSymbols()
        {
            List<string> currentDefines = GetCurrentDefines();

            foreach (string define in globalDefineSymbols)
            {
                if (currentDefines.Contains(define))
                    currentDefines.Remove(define);
            }

            SetCurrentDefines(currentDefines);
        }
    }
}

#endif