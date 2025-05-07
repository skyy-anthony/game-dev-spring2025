using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    public string itemName;  // The name of the item
    public Sprite itemIcon;  // The icon representing the item (for UI)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add the item to the inventory
            InventoryManager.Instance.AddToInventory(this);

            // Destroy the object after picking it up
            Destroy(gameObject);
        }
    }
}
