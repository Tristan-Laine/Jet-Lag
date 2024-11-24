using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    private float startTime;
    private bool isTiming = false;
    private float elapsedTime;

    public TMP_Text endGameTimeText;
    public TMP_Text realTimeGameTimeText;

    public TMP_Text bestTimeText;


    void Start()
    {
        StartTimer();
    }

    void FixedUpdate()
    {
        if (isTiming)
        {
            elapsedTime = Time.time - startTime;

            if (realTimeGameTimeText != null)
            {
                realTimeGameTimeText.text = FormatTime(elapsedTime);
            }
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isTiming = true;
    }

    public void StopTimer()
    {
        isTiming = false;
        DisplayFinalTime();

        OnGameEnd(elapsedTime);
        GetBestTime(elapsedTime);
    }

    void DisplayFinalTime()
    {
        if (endGameTimeText != null)
        {
            endGameTimeText.text = FormatTime(elapsedTime);
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        float fraction = time * 100 % 100;
        return string.Format("{0:00}:{1:00}<size=70%>:{2:00}</size>", minutes, seconds, fraction);
    }

    public void OnGameEnd(float playerTime)
    {
        BestTimeManager bestTimeManager = FindObjectOfType<BestTimeManager>();
        bestTimeManager.SaveBestTime(playerTime);
    }
    public void GetBestTime(float playerTime)
    {
        BestTimeManager bestTimeManager = FindObjectOfType<BestTimeManager>();
        float? bestTime = bestTimeManager.GetBestTime();

        if (bestTimeManager == null)
        {
            Debug.LogError("BestTimeManager non trouvé dans la scène !");
            return;
        }

        Debug.Log(bestTimeManager.GetBestTime());
        Debug.Log(bestTime);
        Debug.Log(bestTime.HasValue);
        Debug.Log(bestTime.Value);


        
        if (bestTime.HasValue)
        {
            bestTimeText.text = $"Meilleur Temps : {FormatTime(bestTime.Value)}";
        }
        else
        {
            bestTimeText.text = "";
        }
    }
}
