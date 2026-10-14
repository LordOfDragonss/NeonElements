using UnityEngine;

[CreateAssetMenu(fileName = "PinkEnemyAttack", menuName = "Scriptable Objects/Attacks/PinkEnemyAttack")]
public class PinkEnemyAttack : Attack
{
    [SerializeField] PinkEnemyProjectile projectilePrefab;

    [SerializeField] Vector3 projectileSpawnOffset;

    public override void Execute(Transform transform, Transform playerTransform)
    {
        PinkEnemyProjectile projectile = 
            Instantiate(
                projectilePrefab, 
                transform.position + projectileSpawnOffset, 
                Quaternion.identity
                );

        projectile.Initialize(transform, playerTransform, damage);
    }
}
