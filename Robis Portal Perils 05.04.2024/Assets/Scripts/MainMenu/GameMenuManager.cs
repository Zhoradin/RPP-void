using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameMenuManager : MonoBehaviour
{
    [Header("Game Menu UI")]
    public GameObject gameMenuPanel;
    public GameObject saveSlotsMenu;
    public Button gameMenuIcon;
    public Button saveGameButton;
    public Button loadGameButton;
    public Button backButton;

    [SerializeField] private SaveSlotsInGameMenu saveSlotsInGameMenu;
    [SerializeField] private ArrangerSO arrangerSO;
    [SerializeField] private DataPersistenceManager dataPersistenceManager;

    private bool isGameMenuOpen = false;
    private bool isSaveSlotsMenuOpen = false;

    public bool fromSaveGame = false;
    public bool fromLoadGame = false;
    private float animationDuration = 0.7f;

    void Start()
    {
        gameMenuPanel.SetActive(false);
    }

    void Update()
    {
        // Esc tuþuna basýldýðýnda ve hem gameMenuPanel hem de SaveSlotsMenu kapalýysa, gameMenuPanel'i aç
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameMenuOpen && !isSaveSlotsMenuOpen)
        {
            OpenGameMenu();
        }
        // Esc tuþuna basýldýðýnda ve gameMenuPanel açýksa, gameMenuPanel'i kapat
        else if (Input.GetKeyDown(KeyCode.Escape) && isGameMenuOpen)
        {
            CloseGameMenu();
        }
        // Esc tuþuna basýldýðýnda ve SaveSlotsMenu açýksa, SaveSlotsMenu'yu kapat, gameMenuPanel'i aç
        else if (Input.GetKeyDown(KeyCode.Escape) && isSaveSlotsMenuOpen)
        {
            CloseSaveSlotsMenu();
        }
    }

    public void ToggleGameMenu()
    {
        if (isGameMenuOpen)
        {
            CloseGameMenu();
        }
        else
        {
            OpenGameMenu();
        }
    }

    public void OpenGameMenu()
    {
        // SaveSlotsMenu açýk deðilse, gameMenuPanel'i aç
        if (!isSaveSlotsMenuOpen)
        {
            // Açýlýþ animasyonu
            gameMenuPanel.transform.DOMoveX(250, animationDuration).SetEase(Ease.OutExpo);

            // gameMenuPanel'i aktif hale getiriyoruz
            gameMenuPanel.SetActive(true);

            isGameMenuOpen = true;
        }
    }

    public void CloseGameMenu()
    {
        // Kapanýþ animasyonu
        gameMenuPanel.transform.DOMoveX(-100, animationDuration).SetEase(Ease.InExpo).OnComplete(() =>
        {
            // Animasyon tamamlandýðýnda paneli devre dýþý býrak
            gameMenuPanel.SetActive(false);
        });

        isGameMenuOpen = false;
    }


    public void OnGameMenuIconClick()
    {
        ToggleGameMenu();
    }

    public void OnSaveGameClicked()
    {
        // SaveGame butonuna basýlýnca SaveSlotsInGameMenu'yu aç;
        isSaveSlotsMenuOpen = true;
        // Oyunu kaydetme
        DataPersistenceManager.instance.SaveGame();
        Debug.Log("Game Saved");
        CloseGameMenu();
    }

    public void OnLoadGameClicked()
    {
        // LoadGame butonuna basýlýnca SaveSlotsInGameMenu'yu aç
        saveSlotsMenu.SetActive(true);
        isSaveSlotsMenuOpen = true;
        CloseGameMenu();
        fromLoadGame = true;

        // SaveSlotsInGameMenu'yu etkinleþtir
        saveSlotsInGameMenu.ActivateMenu(true); // true: Oyun yüklüyoruz
    }

    public void OnMainMenuClicked()
    {
        arrangerSO.menuClicked = true;
        // MainMenu sahnesini yükle
        dataPersistenceManager.SaveGame();
        Debug.Log("Oyun Kaydedildi");
        SceneManager.LoadScene("MainMenu");

        // Oyun menüsünü kapat
        CloseGameMenu();
        
    }

    public void CloseSaveSlotsMenu()
    {
        saveSlotsMenu.SetActive(false);
        isSaveSlotsMenuOpen = false;
        OpenGameMenu(); // SaveSlotsInGameMenu kapatýldýðýnda gameMenuPanel'i aç
    }

    public void OnBackButtonClicked()
    {
        if (isSaveSlotsMenuOpen)
        {
            CloseSaveSlotsMenu();
        }
    }
}
