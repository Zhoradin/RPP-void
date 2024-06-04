using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsInGameMenu : MonoBehaviour
{
    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    public GameMenuManager gameMenuManager;

    [SerializeField] private ArrangerSO arrangerSO;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);
        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            saveSlot.SetInteractable(!isLoadingGame || (profileData != null));
        }
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        if (gameMenuManager.fromLoadGame == true)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            if (!isLoadingGame)
            {
                // Yeni oyun baþlatma
                DataPersistenceManager.instance.NewGame();
                SceneManager.LoadSceneAsync("RobisRoom");
            }
            else if (isLoadingGame)
            {
                arrangerSO.fromLoadGame = true;
                // Oyun yükleme
                SceneManager.LoadSceneAsync(DataPersistenceManager.instance.GetSavedSceneName());
            }
            Debug.Log("Game Loaded");
            gameMenuManager.fromLoadGame = false;
        }
        
        DeactivateMenu();
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
