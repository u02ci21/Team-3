using UnityEngine;
using UnityEngine.EventSystems;

public class HoverHighlight2D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public Color highlightColor = Color.yellow;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        spriteRenderer.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        spriteRenderer.color = originalColor;
    }
}
