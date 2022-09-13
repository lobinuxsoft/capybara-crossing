using System.Linq;
using UnityEngine;

namespace CapybaraCrossing
{
    [CreateAssetMenu(menuName = "Effect Behavior/ Swap Positions Behavior")]
    public class SwapPositionsBehavior : EffectBehavior
    {
        [Tooltip("Tag of the other players with whom an exchange of positions could be made.")]
        [SerializeField] string otherPlayersTag = "Player";

        GameObject[] othersPlayers;

        public override void OnInit(EffectBehaviorComponent behaviorComponent)
        {
            othersPlayers = GameObject
                            .FindGameObjectsWithTag(otherPlayersTag)
                            .Where(x => x.GetHashCode() != behaviorComponent.gameObject.GetHashCode()).ToArray();

            Random.InitState(Mathf.RoundToInt(Time.time));
            int rnd = Random.Range(0, othersPlayers.Length);

            behaviorComponent.GetComponent<PlayerMovement>().StopAllCoroutines();
            othersPlayers[rnd].GetComponent<PlayerMovement>().StopAllCoroutines();

            Vector3 positionAux = behaviorComponent.transform.position;
            behaviorComponent.transform.position = othersPlayers[rnd].transform.position;
            othersPlayers[rnd].transform.position = positionAux;
            Destroy(behaviorComponent);
        }
    }
}