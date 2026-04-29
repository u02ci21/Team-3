using UnityEngine;

public class WateringCan : MonoBehaviour
{
    private bool isSelected = false;
    private SpriteRenderer spriteRenderer;

    [Header("Rotation")]
    public float normalRotation = 0f;
    public float selectedRotation = -45f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

        if (hit.collider == null || hit.collider.gameObject != gameObject)
            return;

        isSelected = !isSelected;
        transform.rotation = Quaternion.Euler(0, 0, isSelected ? selectedRotation : normalRotation);

        if (isSelected)
        {
            TillTool till = FindObjectOfType<TillTool>();
            if (till != null) till.Deselect();
        }

        Debug.Log("Watering can selected: " + isSelected);
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void Deselect()
    {
        isSelected = false;
        transform.rotation = Quaternion.Euler(0, 0, normalRotation);
    }
}