using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public Transform playerTransform;
    public float rotationSpeed = 5f;

    void Update()
    {
        RotatePlayerTowardsMouse();
    }

    void RotatePlayerTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.z));

        float direction = mousePosition.x - playerTransform.position.x;

        Quaternion targetRotation;

        if (direction > 0f)
        {
            targetRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (direction < 0f)
        {
            targetRotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            targetRotation = playerTransform.rotation;
        }

        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
