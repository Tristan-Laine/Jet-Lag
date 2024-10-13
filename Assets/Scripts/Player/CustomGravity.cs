using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public float gravityMultiplier = 1.5f; 
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
}
