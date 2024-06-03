using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralManagerSO", menuName = "ScriptableObjects/GeneralManagerSO", order = 1)]
public class GeneralManagerSO : ScriptableObject
{
    private Dictionary<string, bool> itemDictionary = new Dictionary<string, bool>();

    private void Start()
    {
        // Baþlangýçta bazý örnek itemler ekleyebiliriz
        itemDictionary["computerAdded"] = false;
        itemDictionary["phoneAdded"] = false;
    }

    public void CheckItemName(string itemName)
    {
        string itemKey = itemName + "Added";

        if (itemDictionary.ContainsKey(itemKey))
        {
            itemDictionary[itemKey] = true;
        }
        else
        {
            itemDictionary.Add(itemKey, true);
        }

        Debug.Log(itemKey + " is set to " + itemDictionary[itemKey]);
    }

    public bool IsItemAdded(string itemName)
    {
        string itemKey = itemName + "Added";
        if (itemDictionary.ContainsKey(itemKey))
        {
            return itemDictionary[itemKey];
        }
        return false;
    }
}
