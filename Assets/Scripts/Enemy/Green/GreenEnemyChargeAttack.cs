using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GreenEnemyChargeAttack", menuName = "Scriptable Objects/Attacks/GreenEnemyChargeAttack")]
public class GreenEnemyChargeAttack : Attack
{
    [SerializeField] private float windUpTime = 0.5f;
    [SerializeField] private float chargeSpeed = 12f;
    [SerializeField] private float chargeDuration = 0.6f;

    public override void Execute(Enemy enemy, Transform playerTransform)
    {
        enemy.StartCoroutine(ChargeRoutine(enemy, playerTransform));
    }

    private IEnumerator ChargeRoutine(Enemy enemy, Transform playerTransform)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        enemy.IsBusy = true;

        Vector3 playerPos = playerTransform.position;

        // Wind-up: stand still (play telegraph animation here)
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(windUpTime);

        // Lock the direction at the start so the player can dodge
        float dir = Mathf.Sign(playerPos.x - enemy.transform.position.x);

        float timer = 0f;
        while (timer < chargeDuration)
        {
            rb.linearVelocity = new Vector2(dir * chargeSpeed, rb.linearVelocity.y);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        enemy.IsBusy = false;
    }
}
