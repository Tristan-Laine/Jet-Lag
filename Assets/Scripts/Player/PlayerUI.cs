using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerJetPack playerJetPack;

    public Image ReservoirBar;
    public Image ThrustForceBar;

    public TMP_Text ReservoirText;
    public TMP_Text ThrustForceText;

    public float barAnimationDuration = 0.2f; 

    private Coroutine reservoirBarCoroutine;
    private Coroutine thrustForceBarCoroutine;

    void Start()
    {
        if (playerJetPack != null)
        {
            playerJetPack.reservoirThrustChanged.AddListener(UpdateReservoirUI);
            playerJetPack.thrustForceChanged.AddListener(UpdateThrustForcedUI);

            UpdateReservoirUI(playerJetPack.reservoir, playerJetPack.reservoirMax);
            UpdateThrustForcedUI(playerJetPack.thrustForce, playerJetPack.thrustForceMax);
        }
    }

    private void OnDestroy()
    {
        if (playerJetPack != null)
        {
            playerJetPack.reservoirThrustChanged.RemoveListener(UpdateReservoirUI);
            playerJetPack.thrustForceChanged.RemoveListener(UpdateThrustForcedUI);
        }
    }

    public void UpdateReservoirUI(float reservoir, float reservoirMax)
    {
        if (ReservoirBar != null)
        {
            UpdateBar(reservoir, reservoirMax, ReservoirBar, ReservoirText, ref reservoirBarCoroutine);
        }
    }

    public void UpdateThrustForcedUI(float thrustForce, float thrustForceMax)
    {
        if (ThrustForceBar != null)
        {
            UpdateBar(thrustForce, thrustForceMax, ThrustForceBar, ThrustForceText, ref thrustForceBarCoroutine);
        }
    }

    public void UpdateBar(float currentValue, float maxValue, Image bar, TMP_Text text, ref Coroutine barCoroutine)
    {
        float targetRatio = currentValue / maxValue;
        text.text = ((int)currentValue).ToString();

        if (barCoroutine != null)
        {
            StopCoroutine(barCoroutine);
        }

        barCoroutine = StartCoroutine(AnimateBar(bar, targetRatio));
    }

    private IEnumerator AnimateBar(Image bar, float targetRatio)
    {
        float initialRatio = bar.fillAmount;
        float timeElapsed = 0f;

        while (timeElapsed < barAnimationDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / barAnimationDuration;
            bar.fillAmount = Mathf.Lerp(initialRatio, targetRatio, t);
            yield return null;
        }

        bar.fillAmount = targetRatio;
    }
}
