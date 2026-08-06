using UnityEngine;

public class HealPlayerZone : MonoBehaviour
{
    public int healAmount = 1;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthSystem>().RecoverHealth(healAmount);
        }
    }
}
