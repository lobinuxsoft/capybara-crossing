using UnityEngine;

public class PowerUp : MonoBehaviour
{
    protected GameObject user;
    protected GameObject affected;
    protected float duration;

    public GameObject Affected
    {
        get
        {
            return affected;
        }
        set
        {
            affected = value;
        }
    }

    public GameObject User
    {
        get
        {
            return user;
        }
        set
        {
            user = value;
        }
    }
}
