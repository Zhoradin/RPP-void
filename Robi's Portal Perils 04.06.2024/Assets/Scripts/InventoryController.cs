using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Inventory.Model;
using Inventory.UI;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        public GameObject InventoryIcon;
        public GameObject InventoryIconSelected;
        public static InventoryController Instance { get; private set; }

        [SerializeField]
        private UIInventoryPage inventoryUI;

        [SerializeField]
        private InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            PrepareUI();
            PrepareInventoryData();
            InventoryIconSelected.SetActive(false);
            CheckSceneAndSetActive();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            if (inventoryUI == null)
            {
                Debug.LogError("UIInventoryPage reference is missing!");
                return;
            }

            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            // Buraya itemle ilgili bir eylem yapmak için gereken kodlarý ekleyebilirsiniz.
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.Name, item.Description);
        }

        private void CheckSceneAndSetActive()
        {
            // Eðer sahne adý MainMenu ise nesneyi deaktif et
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                gameObject.SetActive(false);
            }
            // Eðer sahne adý MainMenu deðilse nesneyi aktif et
            else
            {
                gameObject.SetActive(true);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Eklenen kýsým
            CheckSceneAndSetActive();

            GameObject uiInventoryPageObj = GameObject.Find("UIInventoryPage");
            if (uiInventoryPageObj != null)
            {
                inventoryUI = uiInventoryPageObj.GetComponent<UIInventoryPage>();
                PrepareUI();
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventoryUI();
            }

            if (InventoryIcon != null && RectTransformUtility.RectangleContainsScreenPoint(
                InventoryIcon.GetComponent<RectTransform>(), Input.mousePosition))
            {
                if (InventoryIconSelected != null)
                {
                    InventoryIconSelected.SetActive(true);
                }
            }
            else
            {
                if (InventoryIconSelected != null)
                {
                    InventoryIconSelected.SetActive(false);
                }
            }
        }

        public void ToggleInventoryUI()
        {
            if (inventoryUI.isActiveAndEnabled)
            {
                inventoryUI.Hide();
            }
            else
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                }
            }
        }
    }
}