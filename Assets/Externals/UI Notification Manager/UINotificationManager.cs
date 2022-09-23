using UnityEngine;
using UnityEngine.Pool;

public class UINotificationManager : MonoBehaviour
{
    private static UINotificationManager instance;
   
    [SerializeField] UINotificationMessage messagePref;
    [SerializeField] Transform container;

    ObjectPool<UINotificationMessage> pool;

    public static UINotificationManager Instance
    {
        get
        {
            if (instance == null)
                instance = Instantiate(Resources.Load<UINotificationManager>(nameof(UINotificationManager)));

            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        pool = new ObjectPool<UINotificationMessage>(
            () => { return Instantiate(messagePref, container); },
            notification => { notification.gameObject.SetActive(true); },
            notification => { notification.gameObject.SetActive(false); },
            notification => { Destroy(notification.gameObject); }, false, 10, 20
            );
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        UINotificationMessage notification = pool.Get();

        StartCoroutine(notification.ShowNotification(message, () =>
        {
            pool.Release(notification);
            notification = null;
        }, duration));
    }
}