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

    // Diðer gerektiðinde kullanýlacak fonksiyonlarý ekleyebilirsiniz.

    // Örnek olarak, aktif görevi TextMeshPro nesnesine yazan bir fonksiyon
    void UpdateQuestText()
    {
        if (quests.Count > 0 && activeQuestIndex < quests.Count)
        {
            questText.text = "Aktif Görev: " + quests[activeQuestIndex].questName;
        }
        else
        {
            questText.text = "Aktif Görev: Yok";
        }
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
