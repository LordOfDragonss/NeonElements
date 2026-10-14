using UnityEngine;

public class Saw : MonoBehaviour
{
    public int damage = 1;
    public int knockbackForce = 5;
    public void SpawnSaw(Vector3 spawnLocation)
    {
        Instantiate(gameObject, spawnLocation, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.GetComponent<HealthSystem>().TakeDamage(damage);
            collision.gameObject.GetComponent<PlayerController>().Knockback(knockbackForce, transform.position);
        }
    }
}
