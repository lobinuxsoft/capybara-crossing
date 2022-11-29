using UnityEditor;
using UnityEngine;

namespace TMPro.Examples
{
    [CustomEditor(typeof(WarpText))]
    public class WarpTextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            WarpText wt = (WarpText)target;

            EditorGUILayout.Space(20);

            if (GUILayout.Button("Update View"))
            {
                wt.Warp();
            }
        }
    }
}