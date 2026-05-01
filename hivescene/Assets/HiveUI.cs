using UnityEngine;
using TMPro;

public class HiveUI : MonoBehaviour
{
    [Header("Pollen UI")]
    public TextMeshProUGUI pollenText;

    [Header("Honey UI")]
    public TextMeshProUGUI honeyText;

    void Start()
    {
        // Subscribe here instead of OnEnable — ResourceManager is ready by now
        ResourceManager.Instance.OnPollenChanged.AddListener(UpdatePollen);
        ResourceManager.Instance.OnHoneyChanged.AddListener(UpdateHoney);

        UpdatePollen(ResourceManager.Instance.Pollen);
        UpdateHoney(ResourceManager.Instance.Honey);
    }

    void OnDestroy()
    {
        if (ResourceManager.Instance == null) return;
        ResourceManager.Instance.OnPollenChanged.RemoveListener(UpdatePollen);
        ResourceManager.Instance.OnHoneyChanged.RemoveListener(UpdateHoney);
    }

    void UpdatePollen(float value) => pollenText.text = $"Pollen: {value:F0}";
    void UpdateHoney(float value)  => honeyText.text  = $"Honey: {value:F0}";
}