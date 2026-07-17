using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    enum EnemyState
    {
        Roaming,
        Chasing,
        Attacking
    }

    private EnemyState enemyState;
    private EnemyMovement enemyMovement;
    private PlayerDetection playerDetection;

    private Vector2 startingPosition;
    private float roamingDirectionX;
    private float minimalDistanceRoamed = 2f;
    private float maxRoamingDistance = 5f;
    
    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        playerDetection = GetComponent<PlayerDetection>();
    }

    private void Start()
    {
        startingPosition = transform.position;
        roamingDirectionX = GetRoamingDirectionX();
        enemyState = EnemyState.Roaming;
    }

    private void FixedUpdate()
    {
        switch(enemyState)
        {
            case EnemyState.Roaming:
                Roam();
                break;
            case EnemyState.Chasing:
                Chasing();
                break;
            case EnemyState.Attacking:
                // Implement attacking behavior
                break;
        }
       
    }

    private void Roam()
    {
        float direction = Mathf.Sign(roamingDirectionX - transform.position.x);

        enemyMovement.LinearMovement(direction);

        bool reachedTarget = Mathf.Abs(transform.position.x - roamingDirectionX) < 0.1f;
        bool noGround = !enemyMovement.IsGroundDetected();

        if (reachedTarget || noGround)
        {
            roamingDirectionX = GetRoamingDirectionX();
        }

        if (playerDetection.IsPlayerSeen())
        {
            enemyState = EnemyState.Chasing;
        }
    }

    private void Chasing()
    {
        // get the player's position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
            enemyMovement.LinearMovement(direction);
            // Check if the player is still seen or detected
            if (!playerDetection.playerInProximity && !playerDetection.IsPlayerSeen())
            {
                enemyState = EnemyState.Roaming;
                roamingDirectionX = GetRoamingDirectionX();
            }
        }
        else
        {
            // If the player is not found, return to roaming state
            enemyState = EnemyState.Roaming;
            roamingDirectionX = GetRoamingDirectionX();
        }
    }

    private float GetRoamingDirectionX()
    {
        float newPosition;

        do
        {
            newPosition = startingPosition.x + Random.Range(-maxRoamingDistance, maxRoamingDistance);

        } while (Mathf.Abs(newPosition - transform.position.x) < minimalDistanceRoamed);

        return newPosition;
    }
}
