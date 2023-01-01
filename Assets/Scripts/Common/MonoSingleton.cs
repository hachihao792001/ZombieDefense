using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_singleton;
    public static T Instance
    {
        get
        {
            if (s_singleton == null)
            {
                AssignSingleton(FindObjectOfType<T>());
            }
            return s_singleton;
        }
    }

    private static void AssignSingleton(T instance)
    {
        s_singleton = instance;
        DontDestroyOnLoad(s_singleton);
    }

    protected void Awake()
    {
        if (s_singleton == null)
        {
            AssignSingleton((T)(MonoBehaviour)this);
            SingletonAwake();
        }
        else if (s_singleton != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void SingletonAwake() { }
}