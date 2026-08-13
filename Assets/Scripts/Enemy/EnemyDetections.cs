using UnityEngine;

public class EnemyDetections : MonoBehaviour
{
    public CustomEnemyTrigger detectionTrigger;
    public CustomEnemyTrigger attackTrigger;

    [SerializeField] private LayerMask playerDetectionLayers;
    [SerializeField] private LayerMask floorDetectionLayers;
    [SerializeField] private float seeingRange = 5f;
    [SerializeField] private Vector2 rayOffset = new Vector2(0.25f, 0.25f);

    public bool playerInProximity;
    public bool playerInAttackRange;

    private void Awake()
    {
        detectionTrigger.EnteredTrigger += OnDetectionTriggerEnter;
        detectionTrigger.ExitedTrigger += OnDetectionTriggerExit;

        attackTrigger.EnteredTrigger += OnAttackTriggerEnter;
        attackTrigger.ExitedTrigger += OnAttackTriggerExit;
    }

    private void OnDetectionTriggerEnter(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerInProximity = true;
    }

    private void OnDetectionTriggerExit(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerInProximity = false;
    }

    private void OnAttackTriggerEnter(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerInAttackRange = true;
    }

    private void OnAttackTriggerExit(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerInAttackRange = false;
    }

    private void Update()
    {
        
    }

    // This method checks if the player is within the enemy's line of sight using raycasting.
    public bool IsPlayerSeen() 
    {
        int numberOfRays = 10;
        float angleStep = 15f;
        float startAngle = -angleStep * (numberOfRays - 1) / 2;

        Vector2 rayOrigin = transform.TransformPoint(rayOffset);

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = startAngle + angleStep * i;

            // direction based on the enemy's facing direction
            Vector2 direction = Quaternion.Euler(0, 0, angle) * (transform.localScale.x > 0 ? Vector2.right : Vector2.left);


            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                direction,
                seeingRange,
                playerDetectionLayers
            );

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(rayOrigin, direction * hit.distance, Color.red);
                return true;
            }
            Debug.DrawRay(rayOrigin, direction * seeingRange, Color.yellow);
        }

        return false;
    }

    public bool IsGroundAhead()
    {
        Vector2 rayOrigin = transform.TransformPoint(new Vector2(0.25f,0.10f));

        Vector2 rayDirection = transform.localScale.x > 0
            ? (Vector2.down + Vector2.right).normalized
            : (Vector2.down + Vector2.left).normalized;

        Debug.DrawRay(rayOrigin, rayDirection, Color.green);

        return Physics2D.Raycast(rayOrigin, rayDirection, 1f, floorDetectionLayers);
    }
}