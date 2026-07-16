using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    bool isAttacking = false;
    public Animator animator;
    public Collider2D attackCollider;
    public float AttackTransitionDuration = 0.2f;
    private Vector2 facingDirection = Vector2.down;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void Update()
    {
        SetAttackCollider();
    }

    public void SetFacingDirection(Vector2 moveInput)
    {
        Vector2 snapped = Get8Direction(moveInput);
        if (snapped != Vector2.zero)
        {
            facingDirection = snapped;
        }
    }
    public Vector2 Get8Direction(Vector2 input)
    {
        if (input.magnitude < 0.1f)
            return Vector2.zero;

        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

        // Snap to nearest 45 degrees
        angle = Mathf.Round(angle / 45f) * 45f;

        float radians = angle * Mathf.Deg2Rad;

        return new Vector2(
            Mathf.Round(Mathf.Cos(radians)),
            Mathf.Round(Mathf.Sin(radians))
        );
    }


    private void SetAttackCollider()
    {
        attackCollider.isTrigger = isAttacking;
    }

    public void OnAttack(InputValue value)
    {

        float buttonValue = value.Get<float>();
        if (buttonValue > 0f)
        {
            StartCoroutine(Attack());
        }

    }

    public IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetFloat("AttackX",facingDirection.x);
        animator.SetFloat("AttackY",facingDirection.y);
        animator.SetTrigger("AttackTrigger");
        while (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            yield return null;
        }

        // Wait until the attack animation finishes
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(AttackTransitionDuration);
        isAttacking = false;
    }
}
