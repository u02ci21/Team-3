using UnityEngine;

public class ItemData : MonoBehaviour
{
    public string itemName;
    public ItemCategory category;
    public int points = 10;
    public float moveSpeed = 1.5f;

    private float startY;
    private float randomOffset;

    void Start()
    {
        startY = transform.position.y;
        randomOffset = Random.Range(0f, 100f);
        moveSpeed = Random.Range(1f, 2.5f);
    }

    void Update()
    {
        // Move right
        float newX = transform.position.x + moveSpeed * Time.deltaTime;

        // Wave up and down but stay near starting Y position
        float newY = startY + Mathf.Sin((Time.time + randomOffset) * 2f) * 0.3f;

        transform.position = new Vector2(newX, newY);

        // Destroy item if it goes off the right side of the screen
        if (transform.position.x > 12f)
            Destroy(gameObject);
    }
}