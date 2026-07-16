using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public float Movementspeed;
    public float JumpForce;
    public float groundCheckDistance = 0.5f;
    public int ExtraJumpsRemaining;
    public int TotalExtraJumps;
    Rigidbody2D rb;
    Vector3 moveinput;
    public LayerMask floorLayers;
    public Animator animator;
    public GameObject Sprite;
    public PlayerAttack playerAttack;
    float lastDirection = 1f; // 1 = right, -1 = left
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ExtraJumpsRemaining = TotalExtraJumps;
    }

    private void Update()
    {
        rb.linearVelocity = (new Vector2(moveinput.x * Movementspeed, rb.linearVelocity.y));
        if (animator != null)
        {
            animator.SetBool("Walking", rb.linearVelocityX != 0);
        }
        if (rb.linearVelocityX != 0)
        {
            lastDirection = Mathf.Sign(rb.linearVelocityX);
        }
        if (Sprite != null)
        {
            float yRotation = lastDirection < 0 ? 180f : 0f;
            Sprite.transform.rotation = Quaternion.Euler(Sprite.transform.rotation.x, yRotation, Sprite.transform.rotation.z);
        }
        if (isOnGround())
        {
            ExtraJumpsRemaining = TotalExtraJumps;
        }
    }



    public void OnMove(InputValue value)
    {
        moveinput = value.Get<Vector2>();
        playerAttack.SetFacingDirection(moveinput);
    }

    public void OnJump()
    {
        if (ExtraJumpsRemaining > 0)
        {
            ExtraJumpsRemaining--;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
            if (animator != null)
                animator.SetTrigger("JumpTrigger");
        }
    }

    public bool isOnGround()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, floorLayers) ||
               Physics2D.Raycast(transform.position + new Vector3(0.5f, 0, 0), Vector2.down, groundCheckDistance, floorLayers) ||
               Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0, 0), Vector2.down, groundCheckDistance, floorLayers);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(0.5f, 0, 0), transform.position + new Vector3(0.5f, 0, 0) + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(-0.5f, 0, 0), transform.position + new Vector3(-0.5f, 0, 0) + Vector3.down * groundCheckDistance);
    }
}
