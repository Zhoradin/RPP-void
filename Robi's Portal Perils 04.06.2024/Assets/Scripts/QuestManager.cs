using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    private void Awake()
    {
        instance = this;
    }

    public GeneralManagerSO generalManagerSO;

    public List<QuestList> questLists = new List<QuestList>();

    // TemporaryButton isimli buton buraya atanacak
    public Button temporaryButton;

    public float textSpeed = 0.1f; // Metnin ortaya çýkma hýzýný ayarlamak için deðiþken

    private Quest activeQuest; // Aktif görevi tutmak için bir deðiþken
    private TextMeshProUGUI activeQuestTextMeshPro;

    public GameObject questPanel;
    public Button questIcon;
    public bool questPanelOpen = false;

    void Start()
    {
        questPanel.SetActive(false);
        SetActiveQuest(); // Baþlangýçta aktif görevi belirle
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            OpenQuestMenu();
        }
    }

    void SetActiveQuest()
    {
        // Quest listesini dolaþarak ilk tamamlanmamýþ görevi aktif görev olarak ayarla
        foreach (QuestList questList in questLists)
        {
            foreach (Quest quest in questList.quests)
            {
                if (!quest.isCompleted)
                {
                    activeQuest = quest;
                    // Aktif görevin textMeshPro'sý varsa, görünürlüðünü ayarla ve yazýyý yavaþça yazdýr
                    if (activeQuest.textMeshPro != null)
                    {
                        activeQuestTextMeshPro = activeQuest.textMeshPro;
                        activeQuestTextMeshPro.gameObject.SetActive(true);
                        StartCoroutine(DisplayActiveQuest(activeQuest.questName));
                        // Kalýn yazý tipi
                        activeQuestTextMeshPro.fontStyle = FontStyles.Bold;
                    }
                    return; // Aktif görevi bulduktan sonra döngüden çýk
                }
            }
        }
    }

    IEnumerator DisplayActiveQuest(string questName)
    {
        string displayedText = "";
        for (int i = 0; i < questName.Length; i++)
        {
            displayedText += questName[i];
            activeQuestTextMeshPro.text = displayedText;
            yield return new WaitForSeconds(textSpeed); // Her karakter arasýnda belirtilen süre kadar bekleyin
        }
    }

    public void CompleteActiveQuest()
    {
        if (activeQuest != null && activeQuest.questName == generalManagerSO.itemKey && activeQuest.triggerType == Quest.QuestType.item)
        {
            if (generalManagerSO.questTrigger == true)
            {
                Debug.Log(generalManagerSO.itemKey);
                activeQuest.isCompleted = true;
                // Eðer activeQuest'in textMeshPro'sý varsa, görünürlüðünü ayarla ve textini deðiþtir
                if (activeQuest.textMeshPro != null)
                {
                    StartCoroutine(FadeText(activeQuest.textMeshPro));
                }
                SetActiveQuest(); // Aktif görevi tamamladýktan sonra bir sonraki aktif görevi belirle
            }
            
        }
    }

    IEnumerator FadeText(TextMeshProUGUI textMeshPro)
    {
        Color originalColor = textMeshPro.color;
        Color fadedColor = originalColor;
        fadedColor.a = 0.5f; // Griye dönüþecek alfa deðeri
        for (float t = 0; t < 1; t += Time.deltaTime / 1) // 1 saniyede geçiþ
        {
            textMeshPro.color = Color.Lerp(originalColor, fadedColor, t);
            yield return null;
        }
        textMeshPro.color = fadedColor;
    }

    public void OpenQuestMenu()
    {
        if (questPanel.activeInHierarchy)
        {
            questPanel.SetActive(false);
        }
        else
        {
            questPanel.SetActive(true);
        }

    }
}

[System.Serializable]
public class QuestList
{
    public string listName;
    public List<Quest> quests = new List<Quest>();
}

[System.Serializable]
public class Quest
{
    public string questName;
    public TextMeshProUGUI textMeshPro;
    public bool isCompleted;
    public enum QuestType { item, dialogue}
    public QuestType triggerType;
}
