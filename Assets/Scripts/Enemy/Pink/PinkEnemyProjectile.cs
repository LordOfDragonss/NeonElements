using UnityEngine;

public class PinkEnemyProjectile : MonoBehaviour
{
    private Transform target;
    private float damage;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 3f;

    public void Initialize(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (target == null)
            return;
        
            Vector2 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player hit by projectile");
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Wall"))
        {
            Debug.Log("Projectile hit wall");
            Destroy(gameObject);
        }
    }
}