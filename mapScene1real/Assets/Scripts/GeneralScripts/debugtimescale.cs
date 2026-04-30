using UnityEngine;

public class DebugTimeScale : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log($"Current Time.timeScale: {Time.timeScale}");
            Time.timeScale = 1;
            Debug.Log("Reset timeScale to 1");
        }
    }
}