using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Quest
{
    public string questName;
    public bool isCompleted;
    // Diðer gerekli alanlarý da ekleyebilirsiniz.
}

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();
    public TextMeshProUGUI questText; // TextMeshPro nesnesi

    private int activeQuestIndex = 0; // Aktif olan görevin index'i

    void Start()
    {
        // Örnek olarak baþlangýçta bazý görevleri ekleyebilirsiniz.
        AddQuest("Görev 1");
        AddQuest("Görev 2");
        // ...

        // Baþlangýçta aktif olan görevin adýný yazdýr
        UpdateQuestText();
    }

    void AddQuest(string questName)
    {
        Quest newQuest = new Quest();
        newQuest.questName = questName;
        newQuest.isCompleted = false;
        quests.Add(newQuest);
    }

    void UpdateQuestText()
    {
        string newText = "";
        for (int i = 0; i < quests.Count; i++)
        {
            if (i == activeQuestIndex)
            {
                newText += "<b>" + "Aktif Görev: " + quests[i].questName + "</b>" + "\n";
            }
            else
            {
                if (quests[i].isCompleted)
                {
                    newText += "<s><alpha=#80>" + quests[i].questName + "</alpha></s>\n";
                }
                else
                {
                    newText += quests[i].questName + "\n";
                }
            }
        }
        questText.text = newText;
    }


    // Görev tamamlandýðýnda çaðrýlacak fonksiyon
    public void CompleteQuest(string questName)
    {
        Quest completedQuest = quests.Find(q => q.questName == questName);

        if (completedQuest != null && !completedQuest.isCompleted)
        {
            completedQuest.isCompleted = true;

            // Bir sonraki görevi aktif hale getir (eðer varsa)
            activeQuestIndex++;

            // Text'i güncelle
            UpdateQuestText();
        }
    }

    // TemporaryButton tarafýndan çaðrýlacak fonksiyon
    public void OnTemporaryButtonClick()
    {
        if (activeQuestIndex < quests.Count)
        {
            CompleteQuest(quests[activeQuestIndex].questName);
        }
    }
}
