using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    //SOLO DEBUG
    [SerializeField] GameObject p1;
    [SerializeField] GameObject p2;
    //SOLO DEBUG

    public enum PowerUpType
    {
        SLOW_MOTION,
        TELEPORT,
        SWAP,
        FREEZING,
        INK
    }

    public static void CreateAndAddPowerUp(GameObject user, GameObject affected, PowerUpType powerUpType)
    {
        PowerUp power;
        switch (powerUpType)
        {
            case PowerUpType.SLOW_MOTION:
                power = user.AddComponent<SlowMotionBehaviour>();
                break;
            case PowerUpType.SWAP:
                power = user.AddComponent<SwapPlayersBehaviour>();
                break;
            case PowerUpType.TELEPORT:
                power = user.AddComponent<TeleportBehaviour>();
                break;
            case PowerUpType.FREEZING:
                power = user.AddComponent<FreezingBehaviour>();
                break;
            case PowerUpType.INK:
                power = user.AddComponent<InkBehaviour>();
                break;
            default:
                power = user.AddComponent<PowerUp>();
                break;
        }
        power.Affected = affected;
        power.User = user;
    }

    //SOLO DEBUG
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateAndAddPowerUp(p1, p2, PowerUpType.SLOW_MOTION);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateAndAddPowerUp(p1, p2, PowerUpType.SWAP);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CreateAndAddPowerUp(p1, p2, PowerUpType.TELEPORT);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CreateAndAddPowerUp(p1, p2, PowerUpType.INK);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CreateAndAddPowerUp(p1, p2, PowerUpType.FREEZING);
        }
    }
    //SOLO DEBUG
}
