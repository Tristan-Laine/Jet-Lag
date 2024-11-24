using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestTimeManager : MonoBehaviour
{
    private const string BestTimeKey = "BestTime";

    public void SaveBestTime(float newTime)
    {
        if (PlayerPrefs.HasKey(BestTimeKey))
        {
            float currentBest = PlayerPrefs.GetFloat(BestTimeKey);
            if (newTime < currentBest)
            {
                PlayerPrefs.SetFloat(BestTimeKey, newTime);
                PlayerPrefs.Save();
            }
        }
        else
        {
            PlayerPrefs.SetFloat(BestTimeKey, newTime);
            PlayerPrefs.Save();
        }
    }

    public float? GetBestTime()
    {
        if (PlayerPrefs.HasKey(BestTimeKey))
        {
            return PlayerPrefs.GetFloat(BestTimeKey);
        }
        return null;
    }

    public void ResetBestTime()
    {
        PlayerPrefs.DeleteKey(BestTimeKey);

        DisplayBestTime displayBestTime = FindObjectOfType<DisplayBestTime>();
        if (displayBestTime != null)
        {
            displayBestTime.UpdateBestTimeDisplay();
        }
    }

}
