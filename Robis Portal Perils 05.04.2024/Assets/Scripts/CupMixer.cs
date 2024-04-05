using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CupMixer : MonoBehaviour
{
    public List<Transform> cups;
    public float switchInterval = 1.0f;
    public float moveSpeed = 2.0f;
    public float parabolaHeightUp = 2.0f;
    public float parabolaHeightDown = -2.0f;
    public float clickHeightIncrease = 1.0f;
    public Button startButton;
    public float upDownSpeed = 1.0f;
    public Transform coin;
    public GameObject minigameToggleTrigger;
    public Canvas cupMixerCanvas;

    private bool isMixerActive = false;
    private Vector3[] currentPositions;
    private bool canClick = true;
    private Transform currentCupWithCoin;
    private bool isCupMoving = false;
    private bool shouldWaitBeforeMoving = false;
    private float waitTime = 2.0f;
    private Transform elevatedCup;
    private Vector3 elevatedCupOriginalPosition;
    public TextMeshProUGUI counterText;
    private int coinCounter = 0;
    private int currentMixingTurns = 0;
    private const int totalMixingTurns = 5;
    //private bool isFirstClick = true;

    private void Start()
    {
        startButton.interactable = false;

        startButton.onClick.AddListener(ToggleMixer);

        foreach (Transform cup in cups)
        {
            cup.GetComponent<Button>().onClick.AddListener(() => OnCupClicked(cup));
        }

        currentPositions = new Vector3[cups.Count];
        for (int i = 0; i < cups.Count; i++)
        {
            currentPositions[i] = cups[i].position;
        }

        currentCupWithCoin = cups[0];
        coin.SetParent(currentCupWithCoin);
        coin.localPosition = Vector3.zero;

        cupMixerCanvas.gameObject.SetActive(false);

        StartCoroutine(MixCups());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer trigger alanýna giren nesne "Player" tag'ine sahip ise
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // cupMixerCanvas'ý aktif hale getir
                cupMixerCanvas.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Eðer trigger alanýndan çýkan nesne "Player" tag'ine sahip ise
        if (other.CompareTag("Player"))
        {
            // cupMixerCanvas'ý deaktif hale getir
            cupMixerCanvas.gameObject.SetActive(false);
        }
    }


    IEnumerator MixCups()
    {
        while (true)
        {
            if (isMixerActive)
            {
                if (currentMixingTurns >= totalMixingTurns)
                {
                    isMixerActive = false;
                }

                if (shouldWaitBeforeMoving)
                {
                    yield return new WaitForSeconds(waitTime);
                    shouldWaitBeforeMoving = false;
                }

                isCupMoving = true;

                int cupIndex1, cupIndex2, cupIndex3;
                do
                {
                    cupIndex1 = Random.Range(0, cups.Count);
                    cupIndex2 = Random.Range(0, cups.Count);
                    cupIndex3 = Random.Range(0, cups.Count);
                } while (cupIndex1 == cupIndex2 || cupIndex1 == cupIndex3 || cupIndex2 == cupIndex3);

                Vector3 startPos1 = currentPositions[cupIndex1];
                Vector3 startPos2 = currentPositions[cupIndex2];
                Vector3 startPos3 = currentPositions[cupIndex3];
                Vector3 targetPos1 = new Vector3(startPos2.x, startPos2.y, startPos2.z);
                Vector3 targetPos2 = new Vector3(startPos3.x, startPos3.y, startPos3.z);
                Vector3 targetPos3 = new Vector3(startPos1.x, startPos1.y, startPos1.z);

                float journeyLength1 = Vector3.Distance(startPos1, targetPos1);
                float journeyLength2 = Vector3.Distance(startPos2, targetPos2);
                float journeyLength3 = Vector3.Distance(startPos3, targetPos3);
                float startTime = Time.time;

                while (Time.time - startTime < switchInterval)
                {
                    float distanceCovered1 = (Time.time - startTime) * (journeyLength1 / switchInterval) * moveSpeed;
                    float fractionOfJourney1 = distanceCovered1 / journeyLength1;
                    float distanceCovered2 = (Time.time - startTime) * (journeyLength2 / switchInterval) * moveSpeed;
                    float fractionOfJourney2 = distanceCovered2 / journeyLength2;
                    float distanceCovered3 = (Time.time - startTime) * (journeyLength3 / switchInterval) * moveSpeed;
                    float fractionOfJourney3 = distanceCovered3 / journeyLength3;

                    Vector3 newPos1 = Vector3.Lerp(startPos1, targetPos1, fractionOfJourney1);
                    Vector3 newPos2 = Vector3.Lerp(startPos2, targetPos2, fractionOfJourney2);
                    Vector3 newPos3 = Vector3.Lerp(startPos3, targetPos3, fractionOfJourney3);

                    float parabolaProgress = Mathf.Clamp01(distanceCovered1 / journeyLength1);
                    float parabolaHeightOffset1 = Mathf.Sin(parabolaProgress * Mathf.PI) * parabolaHeightUp;
                    float parabolaHeightOffset2 = Mathf.Sin(parabolaProgress * Mathf.PI) * 0;
                    float parabolaHeightOffset3 = Mathf.Sin(parabolaProgress * Mathf.PI) * parabolaHeightDown;

                    cups[cupIndex1].position = new Vector3(newPos1.x, newPos1.y + parabolaHeightOffset1, newPos1.z);
                    cups[cupIndex2].position = new Vector3(newPos2.x, newPos2.y + parabolaHeightOffset2, newPos2.z);
                    cups[cupIndex3].position = new Vector3(newPos3.x, newPos3.y + parabolaHeightOffset3, newPos3.z);

                    yield return null;
                }

                currentPositions[cupIndex1] = cups[cupIndex1].position;
                currentPositions[cupIndex2] = cups[cupIndex2].position;
                currentPositions[cupIndex3] = cups[cupIndex3].position;

                isCupMoving = false;

                canClick = true;
                currentMixingTurns++;
                UpdateCounterText();
            }
            else
            {
                yield return new WaitForSeconds(switchInterval);
            }
        }
    }

    void ToggleMixer()
    {
        // Start butonunu týklanamaz yap
        startButton.interactable = false;

        if (!isMixerActive)
        {
            isMixerActive = true;
            currentMixingTurns = 0;
        }

        if (isMixerActive && !isCupMoving)
        {
            if (!isCupMoving)
            {
                shouldWaitBeforeMoving = true;
            }

            if (elevatedCup != null)
            {
                StartCoroutine(MoveElevatedCupUpAndDownWithDelay(1.5f));
            }

            foreach (Transform cup in cups)
            {
                if (cup != elevatedCup)
                {
                    StartCoroutine(MoveCupUpAndDown(cup, currentPositions[cups.IndexOf(cup)]));
                }
            }

            MoveCoinToRandomCup();
        }
    }

    IEnumerator MoveElevatedCupUpAndDownWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 originalPos = elevatedCup.position;
        Vector3 targetPos = currentPositions[cups.IndexOf(elevatedCup)];
        float originalCoinHeight = coin.position.y;

        float journeyStartTime = Time.time;
        float duration = 1.0f / upDownSpeed;

        while (Time.time - journeyStartTime < duration)
        {
            float journeyProgress = (Time.time - journeyStartTime) / duration;
            float coinHeight = Mathf.Lerp(originalCoinHeight, targetPos.y, journeyProgress);

            elevatedCup.position = Vector3.Lerp(originalPos, targetPos, journeyProgress);
            coin.position = new Vector3(coin.position.x, coinHeight, coin.position.z);

            yield return null;
        }

        elevatedCup.position = targetPos;
        elevatedCup = null;
    }

    IEnumerator MoveCupUpAndDown(Transform cup, Vector3 originalPos)
    {
        float journeyStartTime = Time.time;
        float duration = 1.0f / upDownSpeed;

        while (Time.time - journeyStartTime < duration)
        {
            float journeyProgress = (Time.time - journeyStartTime) / duration;
            cup.position = Vector3.Lerp(originalPos, new Vector3(originalPos.x, originalPos.y + clickHeightIncrease, originalPos.z), journeyProgress);

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        journeyStartTime = Time.time;

        while (Time.time - journeyStartTime < duration)
        {
            float journeyProgress = (Time.time - journeyStartTime) / duration;
            cup.position = Vector3.Lerp(new Vector3(originalPos.x, originalPos.y + clickHeightIncrease, originalPos.z), originalPos, journeyProgress);

            yield return null;
        }

        canClick = true;
    }

    void OnCupClicked(Transform cup)
    {
        if (canClick && !isCupMoving)
        {
            canClick = false;

            Vector3 newPosition = cup.position + new Vector3(0, clickHeightIncrease, 0);
            cup.position = newPosition;

            if (coin.parent == cup)
            {
                Vector3 coinLocalPosition = coin.localPosition;
                coinLocalPosition.y -= clickHeightIncrease;
                coin.localPosition = coinLocalPosition;
                coinCounter++;
                UpdateCounterText();
            }

            elevatedCup = cup;
            elevatedCupOriginalPosition = elevatedCup.position;

            // Bardak týklandýðýnda Start butonunu týklanabilir yap
            startButton.interactable = true;
        }
    }

    void UpdateCounterText()
    {
        counterText.text = "Counter: " + coinCounter.ToString();
    }

    IEnumerator MoveCoinToNewCup(Transform newCup)
    {
        Vector3 targetPos = new Vector3(newCup.position.x, coin.position.y, newCup.position.z);
        Vector3 originalPos = coin.position;
        float journeyStartTime = Time.time;
        float duration = 1.0f / upDownSpeed;

        while (Time.time - journeyStartTime < duration)
        {
            float journeyProgress = (Time.time - journeyStartTime) / duration;
            coin.position = Vector3.Lerp(originalPos, targetPos, journeyProgress);

            yield return null;
        }

        coin.SetParent(newCup);
    }

    void MoveCoinToRandomCup()
    {
        int randomIndex = Random.Range(0, cups.Count);
        Transform newCup = cups[randomIndex];

        StartCoroutine(MoveCoinToNewCup(newCup));
    }
}