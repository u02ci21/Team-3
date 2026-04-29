using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour
{
    void Awake()
    {
        // Find all EventSystems in the scene
        EventSystem[] eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

        // If there's more than one, destroy the extras
        if (eventSystems.Length > 1)
        {
            Debug.Log("Found " + eventSystems.Length + " EventSystems. Cleaning up...");

            // Keep the first one, destroy the rest
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Debug.Log("Destroying extra EventSystem: " + eventSystems[i].gameObject.name);
                Destroy(eventSystems[i].gameObject);
            }
        }
    }

    void Update()
    {
        // Extra safety: check every few seconds for duplicate EventSystems
        if (Time.frameCount % 120 == 0) // Check every 120 frames (about 2 seconds at 60fps)
        {
            CheckForDuplicateEventSystems();
        }
    }

    void CheckForDuplicateEventSystems()
    {
        EventSystem[] eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

        if (eventSystems.Length > 1)
        {
            Debug.LogWarning("Duplicate EventSystems detected! Cleaning up...");

            // Find the one that's a child of GameSceneManager (our persistent one)
            EventSystem persistentEventSystem = null;

            foreach (EventSystem es in eventSystems)
            {
                if (es.transform.IsChildOf(transform))
                {
                    persistentEventSystem = es;
                    break;
                }
            }

            // Destroy all except the persistent one
            foreach (EventSystem es in eventSystems)
            {
                if (es != persistentEventSystem)
                {
                    Debug.Log("Destroying duplicate: " + es.gameObject.name);
                    Destroy(es.gameObject);
                }
            }
        }
    }

    void OnSceneLoaded()
    {
        // When a scene loads, check immediately for duplicates
        CheckForDuplicateEventSystems();
    }
}
