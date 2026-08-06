using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public enum EnemyState
    {
        Roaming,
        Chasing,
        Attacking
    }

    [Header("Roaming")]
    [SerializeField] private float maxRoamingDistance = 5f;
    [SerializeField] private float minimalDistanceRoamed = 2f;


    public EnemyState enemyState;
    private EnemyMovement enemyMovement;
    private PlayerDetection playerDetection;

    private Transform player;

    private Vector2 startingPosition;
    private Vector2 roamingTarget;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        playerDetection = GetComponent<PlayerDetection>();
    }

    private void Start()
    {
        startingPosition = transform.position;
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
        
        StartRoaming();
    }

    private void FixedUpdate()
    {
        switch(enemyState)
        {
            case EnemyState.Roaming:
                Roaming();
                break;
            case EnemyState.Chasing:
                Chasing();
                break;
            case EnemyState.Attacking:
                Attacking();
                break;
        }
       
    }

    private void Roaming()
    {
        enemyMovement.MoveTo(roamingTarget);

        bool targetReached = Mathf.Abs(transform.position.x - roamingTarget.x) < 0.1f;

        if (targetReached || !enemyMovement.IsGroundAhead())
        {
            roamingTarget.x = GetRoamingDirectionX();
        }

        if (playerDetection.IsPlayerSeen())
        {
            enemyState = EnemyState.Chasing;
        }
    }

    private void Chasing()
    {
        if (player == null)
        {
            StartRoaming();
            return;
        }

        if (!playerDetection.playerInProximity && !playerDetection.IsPlayerSeen())
        {
            StartRoaming();
            return;
        }

        if (playerDetection.playerInAttackRange)
        {
            enemyState = EnemyState.Attacking;
            return;
        }

        if (enemyMovement.IsGroundAhead() || !playerDetection.IsPlayerSeen() || !enemyMovement.isGrounded)
            enemyMovement.MoveTo(player.position);
        else 
            enemyMovement.StopMoving();
    }

    private void Attacking()
    {
        enemyMovement.StopMoving();

        if (!playerDetection.playerInAttackRange)
        {
            enemyState = EnemyState.Chasing;
            return;
        }

        // Attack functionality here TBC
    }

    private void StartRoaming()
    {
        enemyState = EnemyState.Roaming;
        roamingTarget.x = GetRoamingDirectionX();
    }

    private float GetRoamingDirectionX()
    {
        float target;

        do
        {
            target = startingPosition.x + Random.Range(-maxRoamingDistance, maxRoamingDistance);
        } while (Mathf.Abs(target - transform.position.x) < minimalDistanceRoamed);

        return target;
    }
}
