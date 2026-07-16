using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public List<string> AttackableTags = new List<string>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(string tag in AttackableTags)
        {
            if (collision.CompareTag(tag))
            {
                // Get the EnemyHealth component from the enemy
                HealthSystem enemyHealth = collision.GetComponent<HealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(1); // You can adjust the damage value as needed
                }
            }
        }

    }
}
