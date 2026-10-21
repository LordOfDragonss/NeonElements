using UnityEngine;

public class HammerHitbox : MonoBehaviour
{
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.GetComponent<HealthSystem>().TakeDamage(damage);
        }
    }
}
