using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue playerStorage;

    public Animator transition;
    public float transitionTime = 1f;
    public float characterWaitTime = 1.5f;

    private bool canChangeScene = false;

    private void Update()
    {
        if (canChangeScene && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(LoadLevelWithTransition(sceneToLoad));
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            canChangeScene = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            canChangeScene = false;
        }
    }

    IEnumerator LoadLevelWithTransition(string sceneName)
    {
        // Sahne deðiþtirme animasyonunu baþlat
        transition.SetTrigger("Start");

        // Karakterin belirli bir süre beklemesini saðla
        yield return new WaitForSeconds(characterWaitTime);

        // Oyuncu konumunu sakla
        playerStorage.initialValue = playerPosition;

        // Yeni sahneye geçiþ yap
        SceneManager.LoadScene(sceneName);
    }
}
