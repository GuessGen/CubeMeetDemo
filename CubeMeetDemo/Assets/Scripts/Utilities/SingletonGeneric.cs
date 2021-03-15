using UnityEngine;

public class SingletonGeneric<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject emptyObject = new GameObject();
                    emptyObject.name = typeof(T).Name;
                    _instance = emptyObject.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        } else {
            Destroy(gameObject);
        }
    }
}
