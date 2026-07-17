using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private LayerMask detectionLayers;
    [SerializeField] private float seeingRange = 10f;
    [SerializeField] private Vector2 rayOffset = new Vector2(0.5f, 0f);

    public bool playerInProximity;

    // check if the player triggers the enemy's detection collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInProximity = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInProximity = false;
        }
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