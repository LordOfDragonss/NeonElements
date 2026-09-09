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

    private Enemy enemy;
    private EnemyMovement enemyMovement;
    private EnemyDetections enemyDetections;
    private float enemyAttackTimer;

    private Transform player;

    private Vector2 startRoamingPosition;
    private Vector2 roamingTarget;

    private bool jumpStarted = false;

    

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyDetections = GetComponent<EnemyDetections>();
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        StartRoaming();
    }

    private void FixedUpdate()
    {
        switch (enemyState)
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

        if (targetReached || !enemyDetections.IsGroundAhead() || enemyDetections.EnemyCollidesWithEnemy() || enemyDetections.EnemyCollidesWithWall())
        {
            roamingTarget.x = GetRoamingDirectionX();
        }

        if (enemyDetections.IsPlayerSeen())
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

        if (!enemyDetections.playerInProximity && !enemyDetections.IsPlayerSeen())
        {
            StartRoaming();
            return;
        }

        if (enemyDetections.playerInAttackRange)
        {
            enemyState = EnemyState.Attacking;
            return;
        }

        // Jumping logic
        float yDifference = Mathf.Abs(player.position.y - transform.position.y);
        bool needsJump = ((enemyMovement.isGrounded && yDifference > jumpHeightDifference) || !enemyDetections.IsGroundAhead()) && enemyMovement.jumpCooldown <= 0 && enemyMovement.HasJumpFinished;

        if (needsJump)
        {
            enemyState = EnemyState.Jumping;
            return;
        }

        if (enemyDetections.IsGroundAhead() && !enemyDetections.EnemyCollidesWithEnemy() && !enemyDetections.EnemyCollidesWithWall())
            enemyMovement.MoveTo(player.position);
        else
            enemyMovement.StopMoving();
    }

    private void Jumping()
    {
        if (player == null)
        {
            jumpStarted = false;
            enemyState = EnemyState.Chasing;
            return;
        }

        if (!jumpStarted)
        {
            enemyMovement.Jump(player.position);
            jumpStarted = true;
        }

        if (enemyDetections.EnemyCollidesWithWall())
            enemyMovement.StopMoving();
        else
            enemyMovement.MoveTo(player.position);

        if (jumpStarted && enemyMovement.HasJumpFinished)
        {
            jumpStarted = false;
            enemyState = EnemyState.Chasing;
        }
    }

    private void Attacking()
    {
        enemyMovement.StopMoving();

        if (!enemyDetections.playerInAttackRange)
        {
            enemyAttackTimer = 0f;
            enemyState = EnemyState.Chasing;
            return;
        }

        // Attack functionality here TBC

        if (enemyAttackTimer > 0f)
        {
            enemyAttackTimer -= Time.fixedDeltaTime;
            return;
        }
        
        enemy.Attack();

        enemyAttackTimer = enemy.CurrentAttack.Cooldown;
    }

    private void StartRoaming()
    {
        startRoamingPosition = transform.position;
        enemyState = EnemyState.Roaming;
        roamingTarget.x = GetRoamingDirectionX();
    }

    private float GetRoamingDirectionX()
    {
        float target;

        do
        {
            target = startRoamingPosition.x + Random.Range(-maxRoamingDistance, maxRoamingDistance);
        } while (Mathf.Abs(target - transform.position.x) < minimalDistanceRoamed);

        return target;
    }
}
