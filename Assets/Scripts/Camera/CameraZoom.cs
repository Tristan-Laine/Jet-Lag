using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    public Transform player;
    public Transform zoomOutTarget; 
    public float zoomOutDuration = 2f; 
    public float zoomInDuration = 0.5f;

    public ParticleSystem masterParticleSystem;
    public Light zoneLight;
    public AudioSource fireSound;
    public float lightTransitionDuration = 1f;
    public float soundTransitionDuration = 1f;


    private Camera mainCamera;
    private Vector3 originalPosition;
    private bool isInZoomZone = false;
    private bool isZoomingOut = false;

    public CameraFollow cameraFollow;

    void Start()
    {
        mainCamera = Camera.main;
        originalPosition = player.position + cameraFollow.offset;

        DeactivateMasterEffects();
    }

    void Update()
    {
        if (isInZoomZone && Input.GetKey(KeyCode.E))
        {
            if (!isZoomingOut)
            {
                StopAllCoroutines();
                cameraFollow.enabled = false; 
                StartCoroutine(ZoomOut());
            }
        }
        else
        {
            if (isZoomingOut)
            {
                StopAllCoroutines();
                cameraFollow.enabled = true; 
                isZoomingOut = false;
            }
        }
    }

    private IEnumerator ZoomOut()
    {
        isZoomingOut = true;
        float elapsedTime = 0f;
        Vector3 startPosition = mainCamera.transform.position;

        while (elapsedTime < zoomOutDuration)
        {
            float t = elapsedTime / zoomOutDuration;

            t = 1 - Mathf.Pow(1 - t, 3);

            mainCamera.transform.position = Vector3.Lerp(startPosition, zoomOutTarget.position, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = zoomOutTarget.position;
    }

    private IEnumerator ZoomIn()
    {
        isZoomingOut = false;
        float elapsedTime = 0f;
        Vector3 startPosition = mainCamera.transform.position;

        while (elapsedTime < zoomInDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, originalPosition, elapsedTime / zoomInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalPosition;
        cameraFollow.enabled = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInZoomZone = true;

            ActivateEffects();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInZoomZone = false;

            DeactivateMasterEffects();
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
