using UnityEngine;

public class BackgroundTiler : MonoBehaviour
{
    public Sprite grassSprite;
    public int columns = 20;
    public int rows = 15;
    public float tileWidth = 1.5f;
    public float tileHeight = 1.0f;

    void Start()
    {
        float totalWidth = columns * tileWidth;
        float totalHeight = rows * tileHeight;
        float startX = -totalWidth / 2f + tileWidth / 2f;
        float startY = -totalHeight / 2f + tileHeight / 2f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject tile = new GameObject("Grass_" + row + "_" + col);
                tile.transform.SetParent(transform);
                tile.transform.localPosition = new Vector3(
                    startX + col * tileWidth,
                    startY + row * tileHeight,
                    0f
                );
                tile.transform.localScale = Vector3.one;

                SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                sr.sprite = grassSprite;
                sr.sortingOrder = -1;
            }
        }
    }
}