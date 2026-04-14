using UnityEngine;

public enum ItemCategory { GeneralWaste, Recycling, DoNotPickUp, Special }

public class Bin : MonoBehaviour
{
    public ItemCategory acceptedCategory;
    public int penaltyPoints = -5;

    public void SortItem(ItemData item)
    {
        if (item.category == acceptedCategory)
        {
            ScoreManager.AddScore(item.points);
            Debug.Log("Correct! +" + item.points);
        }
        else if (item.category == ItemCategory.DoNotPickUp)
        {
            Debug.Log("Calling UI feedback nwo");
            Debug.Log("Thats a living creature, dont put wildlife in the bin!");
        }
        else
        {
            ScoreManager.AddScore(penaltyPoints);
            HeartSystem.LoseLife();
            Debug.Log("Wrong bin!");
        }
    }
}