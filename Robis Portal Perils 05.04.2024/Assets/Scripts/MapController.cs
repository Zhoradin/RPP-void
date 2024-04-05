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

    public List<ObjectScenePair> interactableObjectScenePairs; // Inspector üzerinden çiftleri sürükleyip býrakýn
    public Canvas wantToGoCanvas;
    public TextMeshProUGUI clickedObjectNameText;
    public Button yesButton;
    public Button noButton;

    private bool canvasVisible = false;
    private string targetSceneName = "";

    private void Start()
    {
        wantToGoCanvas.gameObject.SetActive(false);

        // Buttonlara týklama iþlevleri atanýyor
        yesButton.onClick.AddListener(YesButtonClicked);
        noButton.onClick.AddListener(NoButtonClicked);
    }

    private void Update()
    {
        // Fare imleci ile objelerin üzerine gelip gelmediðini ve týklanýp týklanmadýðýný kontrol et
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            // Listede olan objelerden birine fare imleci geldiyse alt objeleri etkinleþtir
            if (interactableObjectScenePairs.Exists(pair => pair.interactableObject == hitObject))
            {
                if (Input.GetMouseButtonDown(0)) // Sol týklama kontrolü
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

    // Tüm alt objeleri etkisizleþtir
    private void DeactivateAllSubObjects()
    {
        foreach (ObjectScenePair pair in interactableObjectScenePairs)
        {
            DeactivateSubObjects(pair.interactableObject);
        }
    }

    // Verilen ana objenin alt objelerini etkinleþtir
    private void ActivateSubObjects(GameObject mainObject)
    {
        DeactivateAllSubObjects();

        // Ana objenin alt objelerini etkinleþtir
        foreach (Transform child in mainObject.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    // Verilen ana objenin alt objelerini etkisizleþtir
    private void DeactivateSubObjects(GameObject mainObject)
    {
        // Ana objenin alt objelerini etkisizleþtir
        foreach (Transform child in mainObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    // WantToGo Canvas'ýný göster ve týklanan objenin adýný yazdýr
    private void ShowWantToGoCanvas(string objectName)
    {
        wantToGoCanvas.gameObject.SetActive(true);
        clickedObjectNameText.text = "Do you want to go to " + objectName + "?";
        canvasVisible = true;
    }

    // WantToGo Canvas'ýný gizle
    private void HideWantToGoCanvas()
    {
        wantToGoCanvas.gameObject.SetActive(false);
        canvasVisible = false;
    }

    // YesButton týklandýðýnda yapýlacak iþlev
    private void YesButtonClicked()
    {
        string clickedObjectName = clickedObjectNameText.text.Substring(18); // "Do you want to go to " kýsmýný çýkartýyoruz

        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName); // Sahneyi yükle
        }

        HideWantToGoCanvas(); // Canvas'ý gizle
    }

    // NoButton týklandýðýnda yapýlacak iþlev
    private void NoButtonClicked()
    {
        HideWantToGoCanvas(); // Canvas'ý gizle
    }
}
