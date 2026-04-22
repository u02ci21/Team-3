using UnityEngine;

public class SeedDraggable : MonoBehaviour
{
    public SeedType seedType;

    private Vector3 startPosition;
    private bool isDragging = false;
    private Vector3 offset;
    private CircleCollider2D col;

    void Start()
    {
        startPosition = transform.position;
        col = GetComponent<CircleCollider2D>();
    }

    void OnMouseDown()
    {
        // Detach shadow immediately so it stays in place
        Transform shadow = transform.Find("Shadow");
        if (shadow != null)
            shadow.SetParent(transform.parent);

        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
        col.enabled = false;
        HighlightCorrectPlot();
    }
    void OnMouseDrag()
    {
        if (isDragging)
            transform.position = GetMouseWorldPosition() + offset;
    }

    void OnMouseUp()
    {
        isDragging = false;
        ClearAllHighlights();
        TryPlant();
    }

    void HighlightCorrectPlot()
    {
        // Find all plots and highlight only the matching one
        SoilPlot[] allPlots = FindObjectsByType<SoilPlot>(FindObjectsSortMode.None);
        foreach (SoilPlot plot in allPlots)
        {
            if (plot.requiredSeedType == seedType)
                plot.Highlight(true);
        }
    }

    void ClearAllHighlights()
    {
        SoilPlot[] allPlots = FindObjectsByType<SoilPlot>(FindObjectsSortMode.None);
        foreach (SoilPlot plot in allPlots)
            plot.ClearHighlight();
    }

        public void ResetToStart()
    {
        transform.position = startPosition;
        col.enabled = true;
    }

    void TryPlant()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);

        if (hit.collider != null)
        {
            SoilPlot plot = hit.collider.GetComponent<SoilPlot>();

            if (plot != null)
            {
                if (plot.IsLocked())
                {
                    Debug.Log("Plot is locked!");
                    plot.FlashWrong();
                    FindObjectOfType<Timer>().ApplyPenalty(5f);
                    transform.position = startPosition;
                    col.enabled = true;
                    return;
                }

                if (plot.requiredSeedType != seedType)
                {
                    Debug.Log("Wrong seed!");
                    plot.FlashWrong();
                    FindObjectOfType<Timer>().ApplyPenalty(5f);
                    transform.position = startPosition;
                    col.enabled = true;
                    return;
                }

                if (!plot.IsTilled())
                {
                    Debug.Log("Soil isn't tilled yet!");
                    plot.FlashWrong();
                    FindObjectOfType<Timer>().ApplyPenalty(5f);
                    transform.position = startPosition;
                    col.enabled = true;
                    return;
                }

            bool success = plot.Plant(seedType, gameObject);
            if (success)
            {
                Transform shadow = transform.Find("Shadow");
                if (shadow != null)
                    shadow.SetParent(transform.parent);
                gameObject.SetActive(false);
                return;
            }
            }
        }

        transform.position = startPosition;
        col.enabled = true;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}