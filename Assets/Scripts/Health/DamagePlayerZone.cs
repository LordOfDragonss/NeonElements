using UnityEngine;

public class DamagePlayerZone : MonoBehaviour
{
    public int damageAmount = 1;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damageAmount);
        }
    }
}
