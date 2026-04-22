using UnityEngine;
using TMPro;

public class FollowObject : MonoBehaviour
{
    public Transform target; // assign the house
    private Camera mainCamera;
    private RectTransform rectTransform;

    void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
        rectTransform.position = screenPos;
    }
}
