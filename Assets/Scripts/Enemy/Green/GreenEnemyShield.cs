using UnityEngine;

public class GreenEnemyShield : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) // Change player to player attack tag
            enemy.IsAttackBlocked = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Change player to player attack tag
            enemy.IsAttackBlocked = false;
    }

}
