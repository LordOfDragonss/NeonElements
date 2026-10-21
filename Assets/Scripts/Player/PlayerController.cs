using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour,ISlowable
{
    public float Movementspeed;
    private float currentSpeedMultiplier = 1f;
    private Coroutine slowCoroutine;
    public float JumpForce;
    public float groundCheckDistance = 0.5f;
    public int ExtraJumpsRemaining;
    public int TotalExtraJumps;
    [SerializeField] float worldBottomBoundary = -50f;
    Rigidbody2D rb;
    Vector3 moveinput;
    public LayerMask floorLayers;
    public Animator animator;
    public GameObject Sprite;
    public PlayerAttack playerAttack;
    public HealthSystem healthSystem;

    public float lastDirection = 1f; // 1 = right, -1 = left

    // Status Effects
    // CHARM
    private Coroutine charmRoutine;
    private Transform charmSource;
    private bool isCharmed;
    [SerializeField] private float charmedSpeed = 1.5f;
    [SerializeField] private float charmStopDistance = .5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ExtraJumpsRemaining = TotalExtraJumps;
    }

    private void Update()
    {
        if (isCharmed && charmSource != null) 
        {
            float xVelocity;
            float dx = charmSource.position.x - transform.position.x;
            xVelocity = Mathf.Abs(dx) > charmStopDistance ? Mathf.Sign(dx) * charmedSpeed : 0f;

            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
        }
        else rb.linearVelocity = (new Vector2(moveinput.x * Movementspeed * currentSpeedMultiplier, rb.linearVelocity.y));

        if (animator != null)
        {
            animator.SetBool("Walking", rb.linearVelocityX != 0);
        }
        if (rb.linearVelocityX != 0)
        {
            lastDirection = Mathf.Sign(rb.linearVelocityX);
        }
        if (Sprite != null)
        {
            float yRotation = lastDirection < 0 ? 180f : 0f;
            Sprite.transform.rotation = Quaternion.Euler(Sprite.transform.rotation.x, yRotation, Sprite.transform.rotation.z);
        }
        if (isOnGround())
        {
            ExtraJumpsRemaining = TotalExtraJumps;
        }
        CheckBounds();

        if(healthSystem.currentHealth == 0)//temp player death
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            AudioManager.instance.Play("ReturnByDeath");
        }
    }


    public void CheckBounds()
    {
        if (transform.position.y < worldBottomBoundary)
        {
            GetComponent<HealthSystem>().TakeDamage(9999); // Inflict a large amount of damage to ensure death
        }
    }


    public void OnMove(InputValue value)
    {
        moveinput = value.Get<Vector2>();
        playerAttack.SetFacingDirection(moveinput);
    }

    public void OnJump()
    {
        if (ExtraJumpsRemaining > 0 && !isCharmed)
        {
            ExtraJumpsRemaining--;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce * currentSpeedMultiplier);
            if (animator != null)
                animator.SetTrigger("JumpTrigger");
        }
    }

    public void OnCharmed(Transform charmOrigin, float charmDuration)
    {
        charmSource = charmOrigin;

        // Restart the timer if already charmed
        if (charmRoutine != null)
            StopCoroutine(charmRoutine);

        charmRoutine = StartCoroutine(CharmRoutine(charmDuration));
    }

    private IEnumerator CharmRoutine(float duration)
    {
        isCharmed = true;
        yield return new WaitForSeconds(duration);
        isCharmed = false;
        charmSource = null;
        charmRoutine = null;
    }

    public bool isOnGround()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, floorLayers) ||
               Physics2D.Raycast(transform.position + new Vector3(0.5f, 0, 0), Vector2.down, groundCheckDistance, floorLayers) ||
               Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0, 0), Vector2.down, groundCheckDistance, floorLayers);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(0.5f, 0, 0), transform.position + new Vector3(0.5f, 0, 0) + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(-0.5f, 0, 0), transform.position + new Vector3(-0.5f, 0, 0) + Vector3.down * groundCheckDistance);
    }

    public void ApplySlow(float amount, float duration)
    {
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        slowCoroutine = StartCoroutine(Slow(amount, duration));
    }

    private IEnumerator Slow(float amount, float duration)
    {
        currentSpeedMultiplier = 1f - amount;

        yield return new WaitForSeconds(duration);

        currentSpeedMultiplier = 1f;
        slowCoroutine = null;
    }
}
