using UnityEngine;
using UnityEngine.Tilemaps;

public class soilTileClickHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool loadSoilGame = true;

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
        // Use the static Instance directly 
        if (GameSceneManager.Instance == null)
        {
            Debug.LogError("GameSceneManager.Instance is NULL! Make sure GameSceneManager exists in the scene.");
            return;
        }

        Debug.Log("Loading " + (loadSoilGame ? "SOIL GAME" : "MAIN GAME"));

        if (loadSoilGame)
            GameSceneManager.Instance.LoadSoilGame();
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
