using UnityEngine;

public class ItemData : MonoBehaviour
{
    public string itemName;
    public ItemCategory category;
    public int points = 10;
    public float moveSpeed = 1.5f;

    void Update()
    {
        // Moves item across the screen from left to right
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        // Destroy item if it goes off the right side of the screen
        if (transform.position.x > 12f)
        {
            Destroy(gameObject);
        }
    }
}