using UnityEngine;
using UnityEngine.InputSystem;

public class PickupManager : MonoBehaviour
{
    private GameObject heldItem;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(
                Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.GetComponent<ItemData>() != null)
            {
                heldItem = hit.collider.gameObject;
                heldItem.GetComponent<Rigidbody2D>().simulated = false;
            }
        }

        if (heldItem != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(
                Mouse.current.position.ReadValue());
            heldItem.transform.position = new Vector2(mousePos.x, mousePos.y);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && heldItem != null)
        {
            heldItem.GetComponent<Rigidbody2D>().simulated = true;
            heldItem = null;
        }
    }
}