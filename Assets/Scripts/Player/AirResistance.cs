using UnityEngine;

public class AirResistance : MonoBehaviour
{
    public float noDrag = 0f;
    public float airDrag = 0.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb.velocity.y < 0) 
        {
            rb.drag = airDrag;
        }
        else
        {
            rb.drag = noDrag; 
        }
    }

}
