using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public float anticipationDistance = 4f;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 anticipationOffset = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            anticipationOffset += Vector3.up * anticipationDistance;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            anticipationOffset += Vector3.left * anticipationDistance;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            anticipationOffset += Vector3.down * anticipationDistance;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            anticipationOffset += Vector3.right * anticipationDistance;
        }

        Vector3 targetPosition = player.position + offset + anticipationOffset;

        if (targetPosition.y < 2)
        {
            targetPosition.y = 2;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
    }
}
