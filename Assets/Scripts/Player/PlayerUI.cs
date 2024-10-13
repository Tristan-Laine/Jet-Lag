using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public Image ReservoirBar;
    public Image ThrustForceBar;

    public TMP_Text ReservoirText;
    public TMP_Text ThrustForceText;

    public float barAnimationDuration = 0.2f; 

    private Coroutine reservoirBarCoroutine;
    private Coroutine thrustForceBarCoroutine;

    void Start()
    {
        if (playerMovement != null)
        {
            playerMovement.reservoirThrustChanged.AddListener(UpdateReservoirUI);
            playerMovement.thrustForceChanged.AddListener(UpdateThrustForcedUI);

            UpdateReservoirUI(playerMovement.reservoir, playerMovement.reservoirMax);
            UpdateThrustForcedUI(playerMovement.thrustForce, playerMovement.thrustForceMax);
        }
    }

    private void OnDestroy()
    {
        if (playerMovement != null)
        {
            playerMovement.reservoirThrustChanged.RemoveListener(UpdateReservoirUI);
            playerMovement.thrustForceChanged.RemoveListener(UpdateThrustForcedUI);
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
