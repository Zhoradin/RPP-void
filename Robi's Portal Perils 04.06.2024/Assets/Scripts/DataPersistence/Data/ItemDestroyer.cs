using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemDestroyer : MonoBehaviour, IDataPersistence
{
    [SerializeField] private DestroyerSO destroyerSO;
    [SerializeField] private ArrangerSO arrangerSO;

    private static ItemDestroyer instance;

    [SerializeField]
    private List<int> inventoryItemIds = new List<int>();

    private void Start()
    {

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadData(GameData data)
    {
        this.inventoryItemIds.Clear();
        this.inventoryItemIds.AddRange(data.inventoryItemIDs);
    }

    public void SaveData(GameData data)
    {
        data.inventoryItemIDs.Clear();
        data.inventoryItemIDs.AddRange(this.inventoryItemIds);
    }

    public static ItemDestroyer GetInstance()
    {
        return instance;
    }

    public void AddItemID(int itemID)
    {
        destroyerSO.AddItemId(itemID);
        if (!inventoryItemIds.Contains(itemID))
        {
            inventoryItemIds.Add(itemID);
        }
    }

    private void Update()
    {
        CheckInventoryItemsInScene();
        
    }

    private void CheckInventoryItemsInScene()
    {
        foreach (int itemID in inventoryItemIds)
        {
            DestroyItemInScene(itemID);
        }
    }

    private void DestroyItemInScene(int itemID)
    {
        GameObject[] itemsInInventory = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject itemObject in itemsInInventory)
        {
            Item item = itemObject.GetComponent<Item>();
            if (item != null && item.InventoryItem.ItemID == itemID)
            {
                Destroy(itemObject);
                break;
            }
        }
    }

     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {        
        if (arrangerSO.inGameNewGameDestroyer || arrangerSO.fromNewGameDestroyer)
        {
            destroyerSO.GetInventoryItemIds().Clear();
            inventoryItemIds.Clear();
            Debug.Log("IgNg döndü");
            arrangerSO.inGameNewGameDestroyer = false;
            arrangerSO.fromNewGameDestroyer = false;
        }
        TransferItemIdsFromDestroyerSO();
    }

    private void OnApplicationQuit()
    {
        // Oyun kapatýldýðýnda DestroyerSO içeriðini sýfýrla
        destroyerSO.GetInventoryItemIds().Clear();
    }

    private void TransferItemIdsFromDestroyerSO()
    {
        List<int> destroyerSOItemIds = destroyerSO.GetInventoryItemIds();
        inventoryItemIds.Clear(); // ItemDestroyer'ýn kendi listesini temizle

        // DestroyerSO içindeki itemId'leri ItemDestroyer'a aktar
        foreach (int itemId in destroyerSOItemIds)
        {
            inventoryItemIds.Add(itemId);
        }
    }
}