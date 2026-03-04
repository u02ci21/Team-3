using UnityEngine;
using TMPro;

public enum ItemCategory { GeneralWaste, Recycling, DoNotPickUp, Special }

public class Bin : MonoBehaviour
{
    public ItemCategory acceptedCategory;
    public int penaltyPoints = -5;

    void OnTriggerEnter2D(Collider2D other)
    {
        ItemData item = other.GetComponent<ItemData>();
        if (item == null) return;

        if (item.category == acceptedCategory)
        {
            ScoreManager.AddScore(item.points);
            Debug.Log("Correct! +points");
        }
        else if (item.category == ItemCategory.DoNotPickUp)
        {
            Debug.Log("Thats a living creature! Put it back!");
        }
        else
        {
            ScoreManager.AddScore(penaltyPoints);
            Debug.Log("Wrong bin!");
        }

        Destroy(other.gameObject);
    }
}