using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] rubbishPrefabs;
    public float spawnInterval = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;
            int random = Random.Range(0, rubbishPrefabs.Length);
            Instantiate(rubbishPrefabs[random], transform.position, Quaternion.identity);
        }
    }
}