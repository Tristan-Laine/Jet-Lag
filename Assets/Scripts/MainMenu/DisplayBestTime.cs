using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class DisplayBestTime : MonoBehaviour
{
    public TMP_Text bestTimeText;
    public GameObject resetBestTimeButton;

    void Start()
    {
        UpdateBestTimeDisplay();
    }

    public void UpdateBestTimeDisplay()
    {
        BestTimeManager bestTimeManager = FindObjectOfType<BestTimeManager>();
        float? bestTime = bestTimeManager.GetBestTime();

        if (bestTime.HasValue)
        {
            bestTimeText.text = $"Meilleur Temps : {FormatTime(bestTime.Value)}";
            resetBestTimeButton.SetActive(true);
        }
        else
        {
            bestTimeText.text = "";
            resetBestTimeButton.SetActive(false);
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);
        return $"{minutes:D2}:{seconds:D2}.{milliseconds:D3}";
    }
}
