using System.Collections;
using UnityEngine;

public class CharmProjectile : MonoBehaviour
{
    private string projectileOwner;
    private Transform origin;
    private Transform target;

    private float damage;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 3f;

    public void Initialize(Transform origin, Transform target, float damage, string projectileOwner)
    {
        this.projectileOwner = projectileOwner;
        this.origin = origin;
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
        if (collision.CompareTag("Player") && projectileOwner != collision.tag)
        {
            Debug.Log("Player hit by projectile");
            Debug.Log(origin.position);
            if (collision.TryGetComponent(out PlayerController playerController))
                playerController.OnCharmed(origin, 2f);

            Destroy(gameObject);

        }

        if (collision.CompareTag("Enemy") && projectileOwner != collision.tag)
        {
            Debug.Log("Enemy hit by projectile");
            Debug.Log(origin.position);
            if (collision.TryGetComponent(out PlayerController playerController))
                playerController.OnCharmed(origin, 2f);

            Destroy(gameObject);

        }

        else if (collision.CompareTag("Terain"))
        {
            Debug.Log("Projectile hit terain");
            Destroy(gameObject);
        }
    }
}