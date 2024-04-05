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

    // Di�er gerekti�inde kullan�lacak fonksiyonlar� ekleyebilirsiniz.

    // �rnek olarak, aktif g�revi TextMeshPro nesnesine yazan bir fonksiyon
    void UpdateQuestText()
    {
        if (quests.Count > 0 && activeQuestIndex < quests.Count)
        {
            questText.text = "Aktif G�rev: " + quests[activeQuestIndex].questName;
        }
        else
        {
            questText.text = "Aktif G�rev: Yok";
        }
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
