using UnityEngine;

public class AirResistance : MonoBehaviour
{
    public float noDrag = 0f;
    public float airDrag = 0.5f;
    private Rigidbody rb;

    private CustomGravity customGravity; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        customGravity = GetComponent<CustomGravity>();
    }

    void FixedUpdate()
    {
        if (customGravity != null && customGravity.gravityMultiplier <= 1f)
        {
            rb.drag = noDrag;
        }
        else
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
}
