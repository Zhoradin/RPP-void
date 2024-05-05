using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Quest
{
    public string questName;
    public bool isCompleted;
    // Di�er gerekli alanlar� da ekleyebilirsiniz.
}

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();
    public TextMeshProUGUI questText; // TextMeshPro nesnesi

    private int activeQuestIndex = 0; // Aktif olan g�revin index'i

    void Start()
    {
        // �rnek olarak ba�lang��ta baz� g�revleri ekleyebilirsiniz.
        AddQuest("G�rev 1");
        AddQuest("G�rev 2");
        // ...

        // Ba�lang��ta aktif olan g�revin ad�n� yazd�r
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
                newText += "<b>" + "Aktif G�rev: " + quests[i].questName + "</b>" + "\n";
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


    // G�rev tamamland���nda �a�r�lacak fonksiyon
    public void CompleteQuest(string questName)
    {
        Quest completedQuest = quests.Find(q => q.questName == questName);

        if (completedQuest != null && !completedQuest.isCompleted)
        {
            completedQuest.isCompleted = true;

            // Bir sonraki g�revi aktif hale getir (e�er varsa)
            activeQuestIndex++;

            // Text'i g�ncelle
            UpdateQuestText();
        }
    }

    // TemporaryButton taraf�ndan �a�r�lacak fonksiyon
    public void OnTemporaryButtonClick()
    {
        if (activeQuestIndex < quests.Count)
        {
            CompleteQuest(quests[activeQuestIndex].questName);
        }
    }
}
