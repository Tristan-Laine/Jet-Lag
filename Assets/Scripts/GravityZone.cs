using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
    public float newGravityMultiplier = 1.5f;
    private CustomGravity playerGravityScript;
    private static int activeGravityZones = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerGravityScript = other.GetComponent<CustomGravity>();
            if (playerGravityScript != null)
            {
                playerGravityScript.SetGravityMultiplier(newGravityMultiplier);
                activeGravityZones++;
                Debug.Log("Enter in a new Gravity Zone ! = " + newGravityMultiplier);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerGravityScript != null)
        {
            activeGravityZones--;

            if (activeGravityZones <= 0)
            {
                playerGravityScript.ResetGravityMultiplier();
                Debug.Log("Reset Gravity to default.");
                activeGravityZones = 0;
            }

            playerGravityScript = null;
            Debug.Log("Exit a Gravity Zone !");
        }
    }
}