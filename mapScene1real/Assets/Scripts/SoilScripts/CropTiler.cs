using UnityEngine;

public class CropTiler : MonoBehaviour
{
    [Header("Grid Settings")]
    public int columns = 2;
    public int rows = 2;
    public float tileSize = 1.0f;

    private SpriteRenderer[] tiles;

    public void ShowCrop(Sprite cropSprite)
    {
        ClearTiles();

        tiles = new SpriteRenderer[columns * rows];

        float totalWidth = columns * tileSize;
        float totalHeight = rows * tileSize;
        float startX = -totalWidth / 2f + tileSize / 2f;
        float startY = -totalHeight / 2f + tileSize / 2f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject tile = new GameObject("Tile_" + row + "_" + col);
                tile.transform.SetParent(transform);
                tile.transform.localPosition = new Vector3(
                    startX + col * tileSize,
                    startY + row * tileSize,
                    0f
                );
                tile.transform.localScale = Vector3.one;

                SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                sr.sprite = cropSprite;
                sr.sortingOrder = 1;

                tiles[row * columns + col] = sr;
            }
        }
    }

    public void SetColor(Color color)
    {
        if (tiles == null) return;
        foreach (SpriteRenderer sr in tiles)
            if (sr != null) sr.color = color;
    }

    public void HideCrop()
    {
        ClearTiles();
    }

    void ClearTiles()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        tiles = null;
    }
}