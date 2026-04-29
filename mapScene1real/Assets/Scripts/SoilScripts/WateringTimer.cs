using UnityEngine;

public class WaterTimerBar : MonoBehaviour
{
    public SpriteRenderer fillRenderer;
    public SpriteRenderer backgroundRenderer;

    private float fillFullScaleX;

    void Awake()
    {
        if (fillRenderer != null)
        {
            fillFullScaleX = fillRenderer.transform.localScale.x;
            fillRenderer.color = new Color(0.2f, 0.6f, 1f);
        }
        if (backgroundRenderer != null)
            backgroundRenderer.color = new Color(0.2f, 0.2f, 0.2f);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        SetFill(1f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetFill(float fraction)
    {
        fraction = Mathf.Clamp01(fraction);
        Vector3 scale = fillRenderer.transform.localScale;
        scale.x = fraction * fillFullScaleX;
        fillRenderer.transform.localScale = scale;
    }
}