using UnityEngine;
using UnityEngine.EventSystems;

public class PersistentEventSystem : MonoBehaviour
{
    private static PersistentEventSystem instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Ensure required components
            if (GetComponent<EventSystem>() == null)
                gameObject.AddComponent<EventSystem>();
            if (GetComponent<StandaloneInputModule>() == null)
                gameObject.AddComponent<StandaloneInputModule>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Always ensure EventSystem is enabled when this object becomes active
        EventSystem es = GetComponent<EventSystem>();
        if (es != null) es.enabled = true;
    }
}