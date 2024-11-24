using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayerMask;

    void Update()
    {
        bool isGrounded = IsGrounded();
        animator.SetBool("isOnTheGround", isGrounded);
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;

        Debug.DrawRay(origin, direction * groundCheckDistance, Color.red);

        return Physics.Raycast(origin, direction, groundCheckDistance, groundLayerMask);
    }
}

