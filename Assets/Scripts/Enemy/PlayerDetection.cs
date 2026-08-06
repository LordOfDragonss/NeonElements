using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public CustomEnemyTrigger detectionTrigger;
    public CustomEnemyTrigger attackTrigger;

    [SerializeField] private LayerMask detectionLayers;
    [SerializeField] private float seeingRange = 5f;
    [SerializeField] private Vector2 rayOffset = new Vector2(0.5f, 0f);

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
                detectionLayers
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
}