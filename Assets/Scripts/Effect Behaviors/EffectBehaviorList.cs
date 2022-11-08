using System.Collections.Generic;
using UnityEngine;

namespace CapybaraCrossing
{
    [CreateAssetMenu(menuName = "Effect Behavior/ Effect Behavior List", order = 0)]
    public class EffectBehaviorList : ScriptableObject
    {
        [SerializeField] List<EffectBehavior> effectBehaviors; 

        public EffectBehavior GetEffectBehaviorInstance(int index) => ScriptableObject.Instantiate(effectBehaviors[index]);

        public Sprite GetEffectSprite(int index) => effectBehaviors[index].EffectIcon;

        public string[] GetBehaviorNames()
        {
            List<string> result = new List<string>();

            foreach (var item in effectBehaviors)
            {
                result.Add(item.name);
            }

            return result.ToArray();
        }

        public List<EffectBehavior> GetEffectList()
        {
            return effectBehaviors;
        }
    }
}