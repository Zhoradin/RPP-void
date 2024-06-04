using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MapController : MonoBehaviour
{
    [System.Serializable]
    public class ObjectScenePair
    {
        public GameObject interactableObject;
        public string sceneName;
    }

    public List<ObjectScenePair> interactableObjectScenePairs; // Inspector �zerinden �iftleri s�r�kleyip b�rak�n
    public Canvas wantToGoCanvas;
    public TextMeshProUGUI clickedObjectNameText;
    public Button yesButton;
    public Button noButton;

    private bool canvasVisible = false;
    private string targetSceneName = "";

    private void Start()
    {
        wantToGoCanvas.gameObject.SetActive(false);

        // Buttonlara t�klama i�levleri atan�yor
        yesButton.onClick.AddListener(YesButtonClicked);
        noButton.onClick.AddListener(NoButtonClicked);
    }

    private void Update()
    {
        // Fare imleci ile objelerin �zerine gelip gelmedi�ini ve t�klan�p t�klanmad���n� kontrol et
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            // Listede olan objelerden birine fare imleci geldiyse alt objeleri etkinle�tir
            if (interactableObjectScenePairs.Exists(pair => pair.interactableObject == hitObject))
            {
                if (Input.GetMouseButtonDown(0)) // Sol t�klama kontrol�
                {
                    targetSceneName = interactableObjectScenePairs.Find(pair => pair.interactableObject == hitObject)?.sceneName;
                    ShowWantToGoCanvas(hitObject.name);
                }
                ActivateSubObjects(hitObject);
            }
            else
            {
                if (!canvasVisible)
                {
                    HideWantToGoCanvas();
                    DeactivateAllSubObjects();
                }
            }
        }
        else
        {
            if (!canvasVisible)
            {
                HideWantToGoCanvas();
                DeactivateAllSubObjects();
            }
        }
    }

    // T�m alt objeleri etkisizle�tir
    private void DeactivateAllSubObjects()
    {
        foreach (ObjectScenePair pair in interactableObjectScenePairs)
        {
            DeactivateSubObjects(pair.interactableObject);
        }
    }

    // Verilen ana objenin alt objelerini etkinle�tir
    private void ActivateSubObjects(GameObject mainObject)
    {
        DeactivateAllSubObjects();

        // Ana objenin alt objelerini etkinle�tir
        foreach (Transform child in mainObject.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    // Verilen ana objenin alt objelerini etkisizle�tir
    private void DeactivateSubObjects(GameObject mainObject)
    {
        // Ana objenin alt objelerini etkisizle�tir
        foreach (Transform child in mainObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    // WantToGo Canvas'�n� g�ster ve t�klanan objenin ad�n� yazd�r
    private void ShowWantToGoCanvas(string objectName)
    {
        wantToGoCanvas.gameObject.SetActive(true);
        clickedObjectNameText.text = "Do you want to go to " + objectName + "?";
        canvasVisible = true;
    }

    // WantToGo Canvas'�n� gizle
    private void HideWantToGoCanvas()
    {
        wantToGoCanvas.gameObject.SetActive(false);
        canvasVisible = false;
    }

    // YesButton t�kland���nda yap�lacak i�lev
    private void YesButtonClicked()
    {
        string clickedObjectName = clickedObjectNameText.text.Substring(18); // "Do you want to go to " k�sm�n� ��kart�yoruz

        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName); // Sahneyi y�kle
        }

        HideWantToGoCanvas(); // Canvas'� gizle
    }

    // NoButton t�kland���nda yap�lacak i�lev
    private void NoButtonClicked()
    {
        HideWantToGoCanvas(); // Canvas'� gizle
    }
}
