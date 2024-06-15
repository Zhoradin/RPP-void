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

    public float textSpeed = 0.1f; // Metnin ortaya ��kma h�z�n� ayarlamak i�in de�i�ken

    private Quest activeQuest; // Aktif g�revi tutmak i�in bir de�i�ken
    private TextMeshProUGUI activeQuestTextMeshPro;

    public GameObject questPanel;
    public Button questIcon;
    public bool questPanelOpen = false;

    void Start()
    {
        questPanel.SetActive(false);
        SetActiveQuest(); // Ba�lang��ta aktif g�revi belirle
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
        // Quest listesini dola�arak ilk tamamlanmam�� g�revi aktif g�rev olarak ayarla
        foreach (QuestList questList in questLists)
        {
            foreach (Quest quest in questList.quests)
            {
                if (!quest.isCompleted)
                {
                    activeQuest = quest;
                    // Aktif g�revin textMeshPro's� varsa, g�r�n�rl���n� ayarla ve yaz�y� yava��a yazd�r
                    if (activeQuest.textMeshPro != null)
                    {
                        activeQuestTextMeshPro = activeQuest.textMeshPro;
                        activeQuestTextMeshPro.gameObject.SetActive(true);
                        StartCoroutine(DisplayActiveQuest(activeQuest.questName));
                        // Kal�n yaz� tipi
                        activeQuestTextMeshPro.fontStyle = FontStyles.Bold;
                    }
                    return; // Aktif g�revi bulduktan sonra d�ng�den ��k
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
            yield return new WaitForSeconds(textSpeed); // Her karakter aras�nda belirtilen s�re kadar bekleyin
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
                // E�er activeQuest'in textMeshPro's� varsa, g�r�n�rl���n� ayarla ve textini de�i�tir
                if (activeQuest.textMeshPro != null)
                {
                    StartCoroutine(FadeText(activeQuest.textMeshPro));
                }
                SetActiveQuest(); // Aktif g�revi tamamlad�ktan sonra bir sonraki aktif g�revi belirle
            }
            
        }
    }

    IEnumerator FadeText(TextMeshProUGUI textMeshPro)
    {
        Color originalColor = textMeshPro.color;
        Color fadedColor = originalColor;
        fadedColor.a = 0.5f; // Griye d�n��ecek alfa de�eri
        for (float t = 0; t < 1; t += Time.deltaTime / 1) // 1 saniyede ge�i�
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
