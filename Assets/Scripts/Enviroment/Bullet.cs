using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage;
    public float speed;
    public float DecayTime;

    private float timer;

    private void Start()
    {
        Destroy(gameObject, DecayTime);
    }

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        collision.GetComponent<HealthSystem>().TakeDamage(Damage);
        Destroy(gameObject);
    }
}