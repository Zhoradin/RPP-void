using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class GameData
{
    public long lastUpdated;
    public List<int> inventoryItemIDs;
    public Vector3 playerPosition;
    public string currentSceneName;
    public List<InventoryItem> inventoryItems;

    public GameData()
    {
        inventoryItemIDs = new List<int>();
        inventoryItems = new List<InventoryItem>();
        playerPosition = Vector3.zero;
        currentSceneName = "";
    }
    /*public void OnButtonClicked(GameObject clickedObject)
    {
        Image clickedImage = clickedObject.GetComponent<Image>();
        if (clickedImage != null)
        {
            GameObject backgroundObject = GameObject.FindGameObjectWithTag("Background");

            if (backgroundObject != null)
            {
                SpriteRenderer backgroundSpriteRenderer = backgroundObject.GetComponent<SpriteRenderer>();

                if (backgroundSpriteRenderer != null && backgroundSpriteRenderer.sprite != null)
                {
                    clickedImage.sprite = backgroundSpriteRenderer.sprite;
                }
            }
        }
    }*/
}
