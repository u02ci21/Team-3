using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebugUICanvas : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            GraphicRaycaster raycaster = canvas?.GetComponent<GraphicRaycaster>();
            EventSystem es = FindFirstObjectByType<EventSystem>();

            Debug.Log($"Canvas exists: {canvas != null}");
            Debug.Log($"Canvas enabled: {(canvas != null ? canvas.enabled : false)}");
            Debug.Log($"Canvas render mode: {(canvas != null ? canvas.renderMode.ToString() : "N/A")}");
            Debug.Log($"GraphicRaycaster exists: {raycaster != null}");
            Debug.Log($"GraphicRaycaster enabled: {(raycaster != null ? raycaster.enabled : false)}");
            Debug.Log($"EventSystem exists: {es != null}");
            Debug.Log($"EventSystem enabled: {(es != null ? es.enabled : false)}");

            // Check if any UI element is blocking
            if (es != null && Input.GetMouseButtonDown(0))
            {
                var pointerData = new UnityEngine.EventSystems.PointerEventData(es);
                pointerData.position = Input.mousePosition;
                var results = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
                if (raycaster != null) raycaster.Raycast(pointerData, results);
                Debug.Log($"UI elements under mouse: {results.Count}");
                foreach (var result in results)
                {
                    Debug.Log($"  - {result.gameObject.name}");
                }
            }
        }
    }
}
