using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    protected static T instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this as T;
    }
}
