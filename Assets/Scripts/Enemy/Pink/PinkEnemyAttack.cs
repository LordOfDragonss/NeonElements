using UnityEngine;

[CreateAssetMenu(fileName = "PinkEnemyAttack", menuName = "Scriptable Objects/Attacks/PinkEnemyAttack")]
public class PinkEnemyAttack : Attack
{
    [SerializeField] PinkEnemyProjectile projectilePrefab;

    public override void Execute(Transform transform, Transform playerTransform)
    {
        PinkEnemyProjectile projectile = 
            Instantiate(
                projectilePrefab, 
                transform.position, 
                Quaternion.identity
                );

        projectile.Initialize(playerTransform, damage);
    }
}
