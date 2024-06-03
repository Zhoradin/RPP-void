using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    public InventorySO inventoryData;

    public GeneralManagerSO generalManagerSO;

    private Item itemToPickUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        itemToPickUp = collision.GetComponent<Item>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        itemToPickUp = null;
    }

    private void Update()
    {
        if (itemToPickUp != null && Input.GetKeyDown(KeyCode.E))
        {
            // Eþyanýn ItemSO'daki itemID'sini al
            int itemIDToAdd = itemToPickUp.InventoryItem.ItemID;

            string itemNameToAdd = itemToPickUp.InventoryItem.Name;

            generalManagerSO.CheckItemName(itemNameToAdd);

            // ItemDestroyer örneðine itemID'yi ekle
            ItemDestroyer itemDestroyer = ItemDestroyer.GetInstance();
            if (itemDestroyer != null)
            {
                itemDestroyer.AddItemID(itemIDToAdd);
            }

            int reminder = inventoryData.AddItem(itemToPickUp.InventoryItem, itemToPickUp.Quantity);
            if (reminder == 0)
                itemToPickUp.DestroyItem();
            else
                itemToPickUp.Quantity = reminder;
        }
    }
}