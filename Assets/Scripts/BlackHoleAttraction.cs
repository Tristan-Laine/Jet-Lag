using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleAttraction : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 100f;
    public float maxForce = 50f;

    private Rigidbody playerRb;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (playerRb == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > maxDistance) return;
        
        float forceMagnitude = Mathf.Clamp(maxForce / (distance * distance), 0, maxForce);
        Vector3 direction = (transform.position - player.position).normalized;
        Vector3 force = direction * forceMagnitude;

        playerRb.AddForce(force);
    }
}

