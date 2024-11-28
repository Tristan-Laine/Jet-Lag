using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;
    public CheckpointManager checkpointManager;

    public ParticleSystem masterParticleSystem;
    public Light zoneLight;
    public AudioSource fireSound;
    public float lightTransitionDuration = 1f;
    public float soundTransitionDuration = 1f;

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

        if (fireSound != null)
            StartCoroutine(ChangeSoundVolume(fireSound, fireSound.volume, 0.3f));
    }

    private void DeactivateMasterEffects()
    {
        if (masterParticleSystem != null)
            masterParticleSystem.Stop(); 

        if (zoneLight != null)
            StartCoroutine(ChangeLightIntensity(zoneLight, zoneLight.intensity, 0f));

        if (fireSound != null)
            StartCoroutine(ChangeSoundVolume(fireSound, fireSound.volume, 0f));
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

        private IEnumerator ChangeSoundVolume(AudioSource audioSource, float startVolume, float targetVolume)
    {
        float elapsedTime = 0f;

        if (targetVolume != 0f && !audioSource.isPlaying)
            audioSource.Play();

        while (elapsedTime < soundTransitionDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / soundTransitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;

        if (targetVolume == 0f)
            audioSource.Stop();
    }
}
