using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private LayerMask floorLayers;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float groundAheadCheckDistance = 2f;
    [SerializeField] private float jumpForce = 10f;

    public bool isGrounded;
    private bool shouldJump;
    
    private Rigidbody2D rb;
    private Transform player;
    private EnemyStateMachine enemyStateMachine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyStateMachine = GetComponent<EnemyStateMachine>();
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

    public bool IsGroundAhead()
    {
        Vector2 rayDirection = transform.localScale.x > 0
            ? (Vector2.down + Vector2.right).normalized
            : (Vector2.down + Vector2.left).normalized;

        Debug.DrawRay(transform.position, rayDirection * groundAheadCheckDistance, Color.green);

        return Physics2D.Raycast(transform.position, rayDirection, groundAheadCheckDistance, floorLayers);
    }

    private void Update()
    {
        // Enemy grounded check
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, floorLayers);

        // check if player is above or below enemy and enemy is chasing player, if so, jump
        if (enemyStateMachine.enemyState == EnemyStateMachine.EnemyState.Chasing &&
            ((player.position.y-3f)  > transform.position.y || (player.position.y+3f)  < transform.position.y))
        {
            shouldJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (shouldJump && isGrounded)
            Jump();
    }

    public void Jump()
    {
        shouldJump = false;
        
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 jumpDirection = direction * jumpForce;

        rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
    }
}