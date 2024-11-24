using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public float gravityMultiplier = 2.5f;
    private float defaultGravityMultiplier;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultGravityMultiplier = gravityMultiplier; 
    }

    void FixedUpdate()
    {
        ApplyCustomGravity();
    }

    void ApplyCustomGravity()
    {
        Vector3 customGravity = Physics.gravity * gravityMultiplier;
        rb.AddForce(customGravity, ForceMode.Acceleration);
    }

    public void SetGravityMultiplier(float newMultiplier)
    {
        gravityMultiplier = newMultiplier;
    }

    public void ResetGravityMultiplier()
    {
        gravityMultiplier = defaultGravityMultiplier;
    }
}

