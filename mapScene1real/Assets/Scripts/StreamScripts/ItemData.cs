using UnityEngine;

public class ItemData : MonoBehaviour
{
    public string itemName;
    public ItemCategory category;
    public int points = 10;
    public float moveSpeed = 1.5f;
    public Sprite[] possibleSprites;
    private float startY;
    private float randomOffset;
    public bool isHeld = false;
    private float timeAlive = 0f;

    void Start()
    {
        if (possibleSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, possibleSprites.Length);
            GetComponent<SpriteRenderer>().sprite = possibleSprites[randomIndex];
        }

        startY = transform.position.y;
        randomOffset = Random.Range(0f, 100f);
        moveSpeed = Random.Range(1f, 2.5f);
    }

    void Update()
    {
        if (isHeld) return;

        timeAlive += Time.deltaTime;

        float newX = transform.position.x + moveSpeed * Time.deltaTime;
        float newY = startY + Mathf.Sin((Time.time + randomOffset) * 2f) * 0.3f;
        transform.position = new Vector2(newX, newY);

        if (transform.position.x > 12f)
        {
            if (category != ItemCategory.DoNotPickUp && timeAlive > 2f)
                HeartSystem.LoseLife();
            Destroy(gameObject);
        }
    }
}