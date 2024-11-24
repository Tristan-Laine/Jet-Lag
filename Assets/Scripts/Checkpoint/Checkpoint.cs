using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;
    public CheckpointManager checkpointManager;

    public ParticleSystem masterParticleSystem;
    public Light zoneLight;
    public float lightTransitionDuration = 1f;

    void Start()
    {
        DeactivateMasterEffects();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointManager.ActivateCheckpoint(checkpointIndex);

            ActivateEffects();             
        }
    }


    private void ActivateEffects()
    {
        if (masterParticleSystem != null)
            masterParticleSystem.Play(); 

        if (zoneLight != null)
            StartCoroutine(ChangeLightIntensity(zoneLight, zoneLight.intensity, 2.5f));
    }

    private void DeactivateMasterEffects()
    {
        if (masterParticleSystem != null)
            masterParticleSystem.Stop(); 

        if (zoneLight != null)
            StartCoroutine(ChangeLightIntensity(zoneLight, zoneLight.intensity, 0f));
    }

    private IEnumerator ChangeLightIntensity(Light light, float startIntensity, float targetIntensity)
    {
        float elapsedTime = 0f;

        if (targetIntensity != 0f)
            light.enabled = true;

        while (elapsedTime < lightTransitionDuration)
        {
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / lightTransitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        light.intensity = targetIntensity;

        if (targetIntensity == 0f)
            light.enabled = false;
    }
}
