using UnityEngine;
using UnityEngine.Tilemaps;

public class TileClickHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool loadStreamGame = true;

    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null && hit.gameObject == gameObject)
            {
                OnTilemapClicked();
            }
        }
    }

    void OnTilemapClicked()
    {
        // Use the static Instance directly - no reference needed!
        if (GameSceneManager.Instance == null)
        {
            Debug.LogError("GameSceneManager.Instance is NULL! Make sure GameSceneManager exists in the scene.");
            return;
        }

        Debug.Log("Loading " + (loadStreamGame ? "STREAM GAME" : "MAIN GAME"));

        if (loadStreamGame)
            GameSceneManager.Instance.LoadStreamGame();
        else
            GameSceneManager.Instance.LoadMainGame();
    }

    void OnMouseEnter()
    {
        if (tilemap != null)
            tilemap.color = Color.gray;
    }

    void OnMouseExit()
    {
        if (tilemap != null)
            tilemap.color = Color.white;
    }
}
