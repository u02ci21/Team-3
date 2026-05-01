using UnityEngine;
using UnityEngine.Events;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [Header("Starting Resources")]
    public float startingPollen = 50f;
    public float startingHoney  = 20f;

    [Header("Caps")]
    public float maxPollen = 500f;
    public float maxHoney  = 500f;

    public float Pollen { get; private set; }
    public float Honey  { get; private set; }

    // UI can subscribe to these
    public UnityEvent<float> OnPollenChanged = new();
    public UnityEvent<float> OnHoneyChanged  = new();

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        Pollen = startingPollen;
        Honey  = startingHoney;
    }

    public void AddPollen(float amount)
    {
        Pollen = Mathf.Clamp(Pollen + amount, 0f, maxPollen);
        OnPollenChanged.Invoke(Pollen);
    }

    public bool SpendPollen(float amount)
    {
        if (Pollen < amount) return false;   // not enough
        Pollen -= amount;
        OnPollenChanged.Invoke(Pollen);
        return true;
    }

    public void AddHoney(float amount)
    {
        Honey = Mathf.Clamp(Honey + amount, 0f, maxHoney);
        OnHoneyChanged.Invoke(Honey);
    }

    public bool SpendHoney(float amount)
    {
        if (Honey < amount) return false;
        Honey -= amount;
        OnHoneyChanged.Invoke(Honey);
        return true;
    }
}