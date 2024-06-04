using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Animator anim;

    private Coroutine closeGameMenuCoroutine;

    void Start()
    {
        gameMenuPanel.SetActive(false);
    }

    void Update()
    {
        // Esc tu�una bas�ld���nda ve hem gameMenuPanel hem de SaveSlotsMenu kapal�ysa, gameMenuPanel'i a�
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameMenuOpen && !isSaveSlotsMenuOpen)
        {
            OpenGameMenu();
        }
        // Esc tu�una bas�ld���nda ve gameMenuPanel a��ksa, gameMenuPanel'i kapat
        else if (Input.GetKeyDown(KeyCode.Escape) && isGameMenuOpen)
        {
            CloseGameMenu();
        }
        // Esc tu�una bas�ld���nda ve SaveSlotsMenu a��ksa, SaveSlotsMenu'yu kapat, gameMenuPanel'i a�
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
        // SaveSlotsMenu a��k de�ilse, gameMenuPanel'i a�
        if (!isSaveSlotsMenuOpen)
        {
            // gameMenuPanel'i aktif hale getiriyoruz
            gameMenuPanel.SetActive(true);
            anim.SetTrigger("Open");
            StartCoroutine(OpenGameMenuCo());

            isGameMenuOpen = true;
        }
    }

    IEnumerator OpenGameMenuCo()
    {
        yield return new WaitForSeconds(.3f);
        anim.SetTrigger("OpenMenu");
    }

    public void CloseGameMenu()
    {
        // E�er coroutine �al���yorsa, durdur ve men�y� hemen kapat, ard�ndan yeniden a�
        if (closeGameMenuCoroutine != null)
        {
            StopCoroutine(closeGameMenuCoroutine);
            closeGameMenuCoroutine = null;
            ImmediateCloseGameMenu();
            OpenGameMenu();
        }
        else
        {
            // Animasyon tamamland���nda paneli devre d��� b�rak
            anim.SetTrigger("Close");
            closeGameMenuCoroutine = StartCoroutine(CloseGameMenuCo());
        }
    }

    IEnumerator CloseGameMenuCo()
    {
        yield return new WaitForSeconds(1f);

        ImmediateCloseGameMenu();
    }

    private void ImmediateCloseGameMenu()
    {
        anim.ResetTrigger("Close");
        gameMenuPanel.SetActive(false);
        isGameMenuOpen = false;
    }

    public void OnGameMenuIconClick()
    {
        ToggleGameMenu();
    }

    public void OnSaveGameClicked()
    {
        // SaveGame butonuna bas�l�nca SaveSlotsInGameMenu'yu a�;
        isSaveSlotsMenuOpen = true;
        // Oyunu kaydetme
        DataPersistenceManager.instance.SaveGame();
        Debug.Log("Game Saved");
        CloseGameMenu();
    }

    public void OnLoadGameClicked()
    {
        // LoadGame butonuna bas�l�nca SaveSlotsInGameMenu'yu a�
        saveSlotsMenu.SetActive(true);
        isSaveSlotsMenuOpen = true;
        CloseGameMenu();
        fromLoadGame = true;

        // SaveSlotsInGameMenu'yu etkinle�tir
        saveSlotsInGameMenu.ActivateMenu(true); // true: Oyun y�kl�yoruz
    }

    public void OnMainMenuClicked()
    {
        arrangerSO.menuClicked = true;
        // MainMenu sahnesini y�kle
        dataPersistenceManager.SaveGame();
        Debug.Log("Oyun Kaydedildi");
        SceneManager.LoadScene("MainMenu");

        // Oyun men�s�n� kapat
        CloseGameMenu();
    }

    public void CloseSaveSlotsMenu()
    {
        saveSlotsMenu.SetActive(false);
        isSaveSlotsMenuOpen = false;
        OpenGameMenu(); // SaveSlotsInGameMenu kapat�ld���nda gameMenuPanel'i a�
    }

    public void OnBackButtonClicked()
    {
        if (isSaveSlotsMenuOpen)
        {
            CloseSaveSlotsMenu();
        }
    }
}
