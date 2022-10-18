using UnityEditor;

namespace CapybaraCrossing
{
    [CustomEditor(typeof(BuffPickup))]
    public class BuffPickupInspector : Editor
    {
        BuffPickup behaviourPickup;

        private void OnEnable()
        {
            behaviourPickup = (BuffPickup)target;
        }

        public override void OnInspectorGUI()
        {
            if(!behaviourPickup) return;

            EditorGUILayout.ObjectField(serializedObject.FindProperty("currentEffectBehaviorList"));
            EditorGUILayout.Space(5);
            EditorGUILayout.ObjectField(serializedObject.FindProperty("singleplayerEffectBehaviorList"));
            EditorGUILayout.Space(5);
            EditorGUILayout.ObjectField(serializedObject.FindProperty("multiplayerEffectBehaviorList"));

            EditorGUILayout.Space(20);

            if(behaviourPickup.EffectBehaviorList != null)
            {
                behaviourPickup.Index = EditorGUILayout.Popup(behaviourPickup.Index, behaviourPickup.EffectBehaviorList.GetBehaviorNames());
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}