using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] collisionSounds;
    public float speedThreshold = 1f; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    void FixedUpdate(){
        float speed = rb.velocity.magnitude;
        //Debug.Log("Vitesse actuelle : " + speed);
    }


    void OnCollisionEnter(Collision collision)
    {
        float speed = rb.velocity.magnitude;
        Debug.Log("Collision !");


        if (speed > speedThreshold)
        {
            int randomIndex = Random.Range(0, collisionSounds.Length);
            AudioClip chosenClip = collisionSounds[randomIndex];

            audioSource.clip = chosenClip;
            audioSource.Play();
        }
    }
}
