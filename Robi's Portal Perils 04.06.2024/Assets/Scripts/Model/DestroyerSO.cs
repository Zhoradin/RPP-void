using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestroyerSO", menuName = "ScriptableObjects/DestroyerSO", order = 1)]
public class DestroyerSO : ScriptableObject
{
    [SerializeField]
    private List<int> inventoryItemIds = new List<int>();

    public void AddItemId(int itemId)
    {
        if (!inventoryItemIds.Contains(itemId))
        {
            inventoryItemIds.Add(itemId);
        }
    }

    public void RemoveItemId(int itemId)
    {
        inventoryItemIds.Remove(itemId);
    }

    public List<int> GetInventoryItemIds()
    {
        return inventoryItemIds;
    }
}
