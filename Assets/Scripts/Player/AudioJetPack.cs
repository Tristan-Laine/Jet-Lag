using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioJetPack : MonoBehaviour
{
    public AudioSource smallThrustAudio;
    public AudioSource bigThrustAudio;

    public float smallThrustAudioVolumeMax;
    public float bigThrustAudioVolumeMax;

    public void PlaySmallThrustSound()
    {
        if (!smallThrustAudio.isPlaying)
        {
            smallThrustAudio.Play();
        }
        smallThrustAudio.volume = Mathf.Lerp(smallThrustAudio.volume, smallThrustAudioVolumeMax, Time.deltaTime * 50f);
    }

    public void StopSmallThrustSound()
    {
        smallThrustAudio.volume = Mathf.Lerp(smallThrustAudio.volume, 0.0f, Time.deltaTime * 10f);
        if (smallThrustAudio.volume <= 0.01f)
        {
            smallThrustAudio.Stop();
        }
    }

    public void PlayBigThrustSound()
    {
        if (!bigThrustAudio.isPlaying)
        {
            bigThrustAudio.Play();
        }
        bigThrustAudio.volume = Mathf.Lerp(bigThrustAudio.volume, bigThrustAudioVolumeMax, Time.deltaTime * 50f);
    }

    public void StopBigThrustSound()
    {
        bigThrustAudio.volume = Mathf.Lerp(bigThrustAudio.volume, 0.0f, Time.deltaTime * 5f);
        if (bigThrustAudio.volume <= 0.01f)
        {
            bigThrustAudio.Stop();
        }
    }
}
