#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Utilities.Builder
{
    [CustomEditor(typeof(Builder))]
    public class BuilderEditor : Editor
    {
        bool editLogColors;
        GUIStyle labelStyle;
        GUIStyle textStyle;
        GUIStyle boxStyle;

        Vector2 scrollPos;

        private Builder builder = default;

        private void OnEnable()
        {
            builder = (Builder)target;

            labelStyle = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).label);
            labelStyle.richText = true;
            labelStyle.wordWrap = true;

            textStyle = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).textArea);
            textStyle.richText = true;

            boxStyle = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).box);
            boxStyle.richText = true;
        }
        

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (builder.Building) return;

            GUILayout.Space(20);

            builder.useGlobalDefineSymbols = EditorGUILayout.ToggleLeft("Use global define symbols?", builder.useGlobalDefineSymbols);

            if (builder.useGlobalDefineSymbols)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("globalDefineSymbols"));
                serializedObject.ApplyModifiedProperties();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Enable Define Symbols"))
                {
                    builder.EnableSymbols();
                }

                if (GUILayout.Button("Disable Define Symbols"))
                {
                    builder.DisableSymbols();
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(20);
            EditorGUILayout.HelpBox($"Current Build Version: {PlayerSettings.bundleVersion}", MessageType.Info);
            builder.ManualVersionOverride = EditorGUILayout.ToggleLeft("Manual version override?", builder.ManualVersionOverride);

            if (builder.ManualVersionOverride) 
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("releaseVersion"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("majorPatchVersion"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("minorPatchVersion"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bundleVersion"));
                serializedObject.ApplyModifiedProperties();
            }
            else 
            {
                GUILayout.Label("Current Version: " + builder.ReleaseVersion + "." + builder.MajorPatchVersion + "." + builder.MinorPatchVersion);
                GUILayout.Label("Current Bundle Version: " + builder.BundleVersion);
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Build Release Version"))
            {

                builder.BuildLogs.Clear();

                var path = EditorUtility.SaveFilePanel("Builds Locations", "", builder.GetReleaseFolderName(), "");

                if (path.Length == 0) return;

                builder.BuildAllReleaseBuilds(GetAllCurrentScenes(), path);

            }

            GUILayout.Space(10);

            if (GUILayout.Button("Build Major Version"))
            {
                builder.BuildLogs.Clear();

                var path = EditorUtility.SaveFilePanel("Builds Locations", "", builder.GetMajorFolderName(), "");

                if (path.Length == 0) return;

                builder.BuildAllMajorBuilds(GetAllCurrentScenes(), path);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Build Minor Version"))
            {
                builder.BuildLogs.Clear();

                var path = EditorUtility.SaveFilePanel("Builds Locations", "", builder.GetMinorFolderName(), "");

                if (path.Length == 0) return;

                builder.BuildAllMinorBuilds(GetAllCurrentScenes(), path);
            }

            if(builder.BuildLogs.Count > 0)
            {
                GUILayout.Space(20);

                GUILayout.Label("<b>Build Logs</b>", boxStyle);
                EditorGUILayout.Separator();
                editLogColors = EditorGUILayout.ToggleLeft("Edit Log Colors?", editLogColors);
                

                if (editLogColors)
                {
                    EditorGUILayout.Separator();
                    builder.logColor = EditorGUILayout.ColorField("Log Color", builder.logColor, GUILayout.ExpandWidth(false));
                    builder.warnningColor = EditorGUILayout.ColorField("Warnning Color", builder.warnningColor, GUILayout.ExpandWidth(false));
                    builder.errorColor = EditorGUILayout.ColorField("Error Color", builder.errorColor, GUILayout.ExpandWidth(false));
                    builder.assertColor = EditorGUILayout.ColorField("Assert Color", builder.assertColor, GUILayout.ExpandWidth(false));
                    builder.exceptionColor = EditorGUILayout.ColorField("Exception Color", builder.exceptionColor, GUILayout.ExpandWidth(false));
                    builder.stackTraceColor = EditorGUILayout.ColorField("Stack Trace Color", builder.stackTraceColor, GUILayout.ExpandWidth(false));
                }

                EditorGUILayout.Separator();

                scrollPos = GUILayout.BeginScrollView(scrollPos, textStyle, GUILayout.ExpandWidth(true), GUILayout.Height(300));

                foreach (var log in builder.BuildLogs)
                {
                    string message = log.FormatLog(
                                            builder.logColor,
                                            builder.warnningColor,
                                            builder.errorColor,
                                            builder.assertColor,
                                            builder.exceptionColor,
                                            builder.stackTraceColor
                                        );

                    GUILayout.Label(message, labelStyle);
                    GUILayout.Space(20);
                }

                GUILayout.EndScrollView();

                GUILayout.Space(5);

                if (GUILayout.Button("Clear Logs"))
                    builder.BuildLogs.Clear();
            }
        }

        
        private string[] GetAllCurrentScenes() 
        {
            List<string> scenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                    scenes.Add(scene.path);
            }
            return scenes.ToArray();
        }
    }
}
#endif