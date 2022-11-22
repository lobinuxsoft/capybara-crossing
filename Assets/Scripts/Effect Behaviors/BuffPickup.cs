using UnityEngine;
using UnityEngine.SceneManagement;

namespace CapybaraCrossing
{
    [RequireComponent(typeof(SphereCollider))]
    public class BuffPickup : MonoBehaviour
    {
        [SerializeField] protected EffectBehaviorList currentEffectBehaviorList;
        [SerializeField] protected EffectBehaviorList singleplayerEffectBehaviorList;
        [SerializeField] protected EffectBehaviorList multiplayerEffectBehaviorList;
        [SerializeField] private SpriteRenderer[] spriteRenderers;
        [SerializeField] GameObject explodeFvx;
        private string name;

        protected int index;

        public event System.Action<GameObject> onPickup;

        private void Awake()
        {
            if(SceneManager.GetActiveScene().name == "Gameplay-Single")
            {
                currentEffectBehaviorList = singleplayerEffectBehaviorList;
            }
            else
            {
                currentEffectBehaviorList = multiplayerEffectBehaviorList;
            }

            SphereCollider sc = GetComponent<SphereCollider>();
            sc.isTrigger = true;
            PlayerMovement.OnDeath += RemoveMultiplayerEffects;
        }

        private void OnDestroy()
        {
            PlayerMovement.OnDeath -= RemoveMultiplayerEffects;
        }

        private void OnEnable()
        {
            SetRandomEffect();
            name = currentEffectBehaviorList.GetNameByIndex(index);
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].sprite = currentEffectBehaviorList.GetEffectSprite(index);
            }
        }

        public int Index
        {
            get => index;
            set => index = value;
        }

        public EffectBehaviorList EffectBehaviorList => currentEffectBehaviorList;

        public void SetRandomEffect() => index = Random.Range(0, currentEffectBehaviorList.GetBehaviorNames().Length);

        private void OnTriggerEnter(Collider other)
        {
            Color color = other.name.Contains("1") ? Color.blue : Color.red;

            EffectBehaviorComponent effectBehaviorComponent;
            //SetRandomEffect();
            EffectBehavior effectBehavior = currentEffectBehaviorList.GetEffectBehaviorInstance(index);
            effectBehavior.name = effectBehavior.name.Replace("(Clone)", "").Trim();

            // Ahora lo que hace es primero ver si ya hay un EffectBehaviorComponent
            // si existe se fija que tipo de effect behavior tiene, si es el mismo el cual iba a ser activado,
            // directamente cancela todo, si no lo es le agrega uno nuevo
            if (other.gameObject.TryGetComponent<EffectBehaviorComponent>(out effectBehaviorComponent))
            {
                if(effectBehavior.GetType() == effectBehaviorComponent.Behavior.GetType())
                {
                    UINotificationManager.Instance.ShowMessage(
                            $"<b><color=#{ColorUtility.ToHtmlStringRGB(color)}>{other.gameObject.name}</color></b> destroy <b><color=green>{effectBehavior.name}</color></b>"
                        );

                    Destroy(effectBehavior);
                    gameObject.SetActive(false);
                    return;
                }
            }

            effectBehaviorComponent = other.gameObject.AddComponent<EffectBehaviorComponent>();
            effectBehaviorComponent.Behavior = effectBehavior;

            UINotificationManager.Instance.ShowMessage(
                    $"<b><color=#{ColorUtility.ToHtmlStringRGB(color)}>{other.gameObject.name}</color></b> activate <b><color=green>{effectBehavior.name}</color></b>"
                );

            Instantiate(explodeFvx, transform.position, transform.rotation);
            //gameObject.SetActive(false);
            onPickup?.Invoke(this.gameObject);
        }

        private void RemoveMultiplayerEffects()
        {
            currentEffectBehaviorList = singleplayerEffectBehaviorList;
            if (index >= currentEffectBehaviorList.GetBehaviorNames().Length || currentEffectBehaviorList.GetBehaviorNames()[index] != name)
            {
                SetRandomEffect();
                name = currentEffectBehaviorList.GetNameByIndex(index);
                for (int i = 0; i < spriteRenderers.Length; i++)
                {
                    spriteRenderers[i].sprite = currentEffectBehaviorList.GetEffectSprite(index);
                }
            }
        }
    }
}