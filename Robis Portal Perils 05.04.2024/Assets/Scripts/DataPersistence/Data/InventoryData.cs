using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;
using UnityEngine.SceneManagement;
using System.Collections;

public class InventoryData : MonoBehaviour, IDataPersistence
{
    [SerializeField] private InventorySO inventorySO;

    [SerializeField] private ArrangerSO arrangerSO;

    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    private static InventoryData instance;

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

    private void Update()
    {
        // InventorySO'daki olaya abone ol
        inventorySO.OnInventoryUpdated += UpdateInventoryItems;

        StartCoroutine(WaitAndProcessFromNewGame());
        if (arrangerSO.fromContinueGame || arrangerSO.fromLoadGame)
        {
            // fromContinueGame veya fromLoadGame durumunu i�leyin ve ard�ndan s�f�rlay�n
            inventorySO.SetInventoryItems(inventoryItems);
            arrangerSO.fromContinueGame = false;
            arrangerSO.fromLoadGame = false;
        }
    }
    private IEnumerator WaitAndProcessFromNewGame()
    {
        yield return new WaitForSeconds(0.5f); // Bekleme s�resi (�rne�in 1 saniye)

        if (arrangerSO.fromNewGameInventory)
        {
            // fromNewGame durumunu i�leyin ve ard�ndan s�f�rlay�n
            inventoryItems = inventorySO.GetInventoryItems();
            arrangerSO.fromNewGameInventory = false;
        }
        else if (arrangerSO.inGameNewGameInventory)
        {
            // inGameNewGame durumunu i�leyin ve ard�ndan s�f�rlay�n
            inventoryItems = inventorySO.GetInventoryItems();
            arrangerSO.inGameNewGameInventory = false;
            arrangerSO.menuClicked = false;
        }
    }

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

    private void OnDestroy()
    {
        // Script yok edildi�inde olaya abonelikten ��k
        inventorySO.OnInventoryUpdated -= UpdateInventoryItems;
    }

    private void UpdateInventoryItems(Dictionary<int, InventoryItem> updatedInventory)
    {
        // InventorySO'dan gelen g�ncel verileri mevcut inventoryItems listesine kopyala
        foreach (var item in updatedInventory)
        {
            inventoryItems[item.Key] = item.Value;
        }

        // Bo� ��eleri korumak i�in listeyi s�f�rlamadan �nce kopyalay�n
        List<InventoryItem> currentItems = new List<InventoryItem>(inventoryItems);

        // Mevcut bo� ��eleri geri ekleme
        for (int i = 0; i < currentItems.Count; i++)
        {
            if (currentItems[i].IsEmpty)
            {
                inventoryItems[i] = currentItems[i];
            }
        }
    }

    public void LoadData(GameData data)
    {
        // `GameData` nesnesinden gelen verileri `inventoryItems` listesine kopyala
        foreach (var itemData in data.inventoryItems)
        {
            // Burada yeni bir InventoryItem olu�turup verileri ekleyebiliriz
            InventoryItem newItem = new InventoryItem
            {
                item = itemData.item,
                quantity = itemData.quantity
            };

            // Olu�turulan InventoryItem'i `inventoryItems` listesine ekle
            inventoryItems.Add(newItem);
        }
    }

    public void SaveData(GameData data)
    {
        data.inventoryItems.Clear();
        data.inventoryItems.AddRange(this.inventoryItems);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (arrangerSO.inGameNewGameInventory)
        {
            for (int i = 0; i < inventorySO.inventoryItems.Count; i++)
            {
                inventorySO.inventoryItems[i] = default; // Elemanlar� default de�erlerle doldurur (null veya 0)
            }
        }
        // Sahne de�i�ti�inde gerekli i�lemleri yap�n.
        ResizeInventory();
    }

    public void ResizeInventory()
    {
        Debug.Log("Sahne De�i�ti");
        if (inventoryItems.Count > 9)
        {
            inventoryItems.RemoveRange(9, inventoryItems.Count - 9);
        }
    }
}