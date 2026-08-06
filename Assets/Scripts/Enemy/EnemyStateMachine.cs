using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public enum EnemyState
    {
        Roaming,
        Chasing,
        Jumping,
        Attacking
    }

    [Header("Roaming")]
    [SerializeField] private float maxRoamingDistance = 5f;
    [SerializeField] private float minimalDistanceRoamed = 2f;

    [Header("Jumping")]
    [SerializeField] private float jumpHeightDifference = 3.5f;

    public EnemyState enemyState;

    private EnemyMovement enemyMovement;
    private PlayerDetection playerDetection;
    private Transform player;

    private Vector2 startingPosition;
    private Vector2 roamingTarget;

    private bool jumpStarted;

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
            case EnemyState.Jumping:
                Jumping();
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

        // Jumping logic
        float yDifference = Mathf.Abs(player.position.y - transform.position.y);
        bool needsJump = enemyMovement.isGrounded && yDifference > jumpHeightDifference;

        if (needsJump)
        {
            jumpStarted = false;
            enemyState = EnemyState.Jumping;
            return;
        }

        if (enemyMovement.IsGroundAhead())
            enemyMovement.MoveTo(player.position);
        else
            enemyMovement.StopMoving();
    }

    private void Jumping()
    {
        if (player == null)
        {
            enemyState = EnemyState.Chasing;
            return;
        }

        if (!jumpStarted)
        {
            enemyMovement.Jump(player.position);
            jumpStarted = true;
        }

        enemyMovement.MoveTo(player.position);

        if (jumpStarted &&
            enemyMovement.isGrounded &&
            enemyMovement.HasJumpFinished)
        {
            jumpStarted = false;
            enemyState = EnemyState.Chasing;
        }
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
