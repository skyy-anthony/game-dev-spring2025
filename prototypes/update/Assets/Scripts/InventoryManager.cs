using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public GameObject inventorySlotPrefab;  // The InventorySlot prefab to instantiate
    public Transform inventoryUI;           // The panel/grid where the slots will go

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Adds item to the inventory and updates the UI 
    public void AddToInventory(ObjectPickup item)
{
    // Instantiate a copy of the prefab (this creates an in-scene GameObject)
    GameObject newSlot = Instantiate(inventorySlotPrefab);

    // Set the UI panel as the parent of the instantiated slot (not the prefab itself)
    newSlot.transform.SetParent(inventoryUI, false);

    // Set the item icon in the UI
    Image slotImage = newSlot.GetComponent<Image>();
    slotImage.sprite = item.itemIcon;
}

}
