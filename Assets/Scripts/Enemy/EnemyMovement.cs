using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private LayerMask floorLayers;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;

    public bool isGrounded;
    public bool HasJumpFinished;

    private float jumpCooldownTime = 2.5f;
    public float jumpCooldown;

    private bool wasGrounded;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCooldown = jumpCooldownTime;
    }

    private void FixedUpdate()
    {
        // jump cooldown
        if (jumpCooldown > 0)
            jumpCooldown -= Time.fixedDeltaTime;

        wasGrounded = isGrounded;

        // Enemy grounded check
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, floorLayers);

        // draw the raycast in the editor for debugging
        Debug.DrawRay(transform.position, Vector2.down * 0.1f, isGrounded ? Color.green : Color.red);

        // Detect landing
        if (!wasGrounded && isGrounded)
        {
            jumpCooldown = jumpCooldownTime;
            HasJumpFinished = true;
        } 
    }

    public void MoveTo(Vector2 target)
    {
        float direction = Mathf.Sign(target.x - transform.position.x);

        // Stops the enemy from jittering when it reaches the target position
        if (Mathf.Abs(target.x- transform.position.x) < 0.05f)
        {
            StopMoving();
            return;
        }

        FaceDirection(direction);

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    public void StopMoving()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void FaceDirection(float direction) 
    {
        if (direction == 0)
            return;

        transform.localScale = new Vector3( 
            Mathf.Abs(transform.localScale.x) * direction, 
            transform.localScale.y, 
            transform.localScale.z);
    }

    public void Jump(Vector2 target)
    {
        if (!isGrounded || jumpCooldown > 0)
            return;

        HasJumpFinished = false;

        Vector2 direction =
            (target - (Vector2)transform.position).normalized;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            0);

        rb.AddForce(
            new Vector2(direction.x * jumpForce, jumpForce),
            ForceMode2D.Impulse);
    }
}