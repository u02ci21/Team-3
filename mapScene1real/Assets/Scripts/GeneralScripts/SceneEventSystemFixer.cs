using UnityEngine;
using UnityEngine.EventSystems;

public class SceneEventSystemFixer : MonoBehaviour
{
    void Start()
    {
        // Ensure EventSystem is enabled and working
        EventSystem es = FindFirstObjectByType<EventSystem>();
        if (es != null)
        {
            es.enabled = true;
            Debug.Log($"EventSystem '{es.name}' enabled in scene {gameObject.scene.name}");
        }

        // Also ensure time scale is normal
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            Debug.Log("Time.timeScale was 0, reset to 1");
        }
    }
}
