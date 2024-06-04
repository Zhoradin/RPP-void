using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ComicPanelController : MonoBehaviour
{
    public List<ComicList> comicLists = new List<ComicList>();
    private int currentListIndex = 0;
    private int currentPanelIndex = 0;
    public float fadeInSpeed = 2.0f;
    public Canvas comicPanelCanvas;
    [SerializeField] private ArrangerSO arrangerSO;

    public string sceneName;

    private Coroutine fadeInCoroutine;

    void Start()
    {
        DeactivateAllPanels();

        // Baþlangýçta duruma göre canvas'ý ayarla
        UpdateCanvasState();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (fadeInCoroutine != null)
            {
                StopCoroutine(fadeInCoroutine);
            }

            ActivateNextPanel();
        }

        // Her güncelleme adýmýnda duruma göre canvas'ý kontrol et
        UpdateCanvasState();
    }

    void DeactivateAllPanels()
    {
        foreach (var comicList in comicLists)
        {
            foreach (var panel in comicList.comicPanels)
            {
                SetPanelAlpha(panel, 0f);
            }
        }
    }

    void ActivateNextPanel()
    {
        if (currentListIndex >= comicLists.Count)
        {
            Debug.Log("Tüm paneller ve listeler görüntülendi!");
            SceneManager.LoadScene(sceneName);
            return;
        }

        if (currentListIndex < comicLists.Count && currentPanelIndex >= comicLists[currentListIndex].comicPanels.Count)
        {
            currentListIndex++;
            currentPanelIndex = 0;
            DeactivateAllPanels();
        }

        if (currentListIndex < comicLists.Count && currentPanelIndex < comicLists[currentListIndex].comicPanels.Count)
        {
            Image currentPanel = comicLists[currentListIndex].comicPanels[currentPanelIndex];

            if (fadeInCoroutine != null)
            {
                StopCoroutine(fadeInCoroutine);
            }

            fadeInCoroutine = StartCoroutine(FadeInPanel(currentPanel, fadeInSpeed));

            currentPanelIndex++;
        }
    }

    void SetPanelAlpha(Image panel, float alpha)
    {
        Color panelColor = panel.color;
        panelColor.a = alpha;
        panel.color = panelColor;
    }

    IEnumerator FadeInPanel(Image panel, float speed)
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += speed * Time.deltaTime;
            SetPanelAlpha(panel, alpha);
            yield return null;
        }
    }

    // Canvas'ý duruma göre güncelleyen fonksiyon
    void UpdateCanvasState()
    {
        if (arrangerSO.fromNewGameInventory)
        {
            comicPanelCanvas.gameObject.SetActive(true);
        }
        else
        {
            comicPanelCanvas.gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class ComicList
{
    public List<Image> comicPanels = new List<Image>();
}
