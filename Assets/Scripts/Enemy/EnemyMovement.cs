using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private float groundCheckDistance = 2f;
    [SerializeField] private LayerMask floorLayers;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void LinearMovement(float direction)
    {
        if (direction != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) * direction,
                transform.localScale.y,
                transform.localScale.z);
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    public bool IsGroundDetected()
    {
        Vector2 downLeft = (Vector2.down + Vector2.left).normalized;
        Vector2 downRight = (Vector2.down + Vector2.right).normalized;

        Vector2 direction = transform.localScale.x > 0 ? downRight : downLeft;

        Debug.DrawRay(transform.position, direction * groundCheckDistance, Color.green);

        return Physics2D.Raycast(transform.position, direction, groundCheckDistance, floorLayers);
    }
}
