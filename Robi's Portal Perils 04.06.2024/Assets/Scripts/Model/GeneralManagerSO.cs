using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralManagerSO", menuName = "ScriptableObjects/GeneralManagerSO", order = 1)]
public class GeneralManagerSO : ScriptableObject
{
    private Dictionary<string, bool> itemDictionary = new Dictionary<string, bool>();

    public bool questTrigger;
    public string itemKey;

    void Start()
    {

    }

    public void CheckItemName(string itemName)
    {
        itemKey = itemName;

        if (itemDictionary.ContainsKey(itemKey))
        {
            itemDictionary[itemKey] = true;
        }
        else
        {
            itemDictionary.Add(itemKey, true);
        }

        Debug.Log(itemKey + " is set to " + itemDictionary[itemKey]);

        questTrigger = itemDictionary[itemName];
    }

    public bool IsItemAdded(string itemName)
    {
        string itemKey = itemName;
        if (itemDictionary.ContainsKey(itemKey))
        {
            return itemDictionary[itemKey];
        }
        return false;
    }
}