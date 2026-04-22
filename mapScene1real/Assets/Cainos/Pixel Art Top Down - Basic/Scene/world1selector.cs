using UnityEngine;
using UnityEngine.SceneManagement;

public class World1Selector : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "harmonygarden";

    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // Add collider if needed
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    void OnMouseEnter()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = Color.green;
    }

    void OnMouseExit()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }

    void OnMouseDown()
    {
        Debug.Log("Loading World 1...");
        SceneManager.LoadScene(sceneToLoad);
    }
}
