using UnityEngine;

[CreateAssetMenu(fileName = "PinkEnemyCharmAttack", menuName = "Scriptable Objects/Attacks/PinkEnemyCharmAttack")]
public class PinkEnemyCharmAttack : Attack
{
    [SerializeField] CharmProjectile projectilePrefab;
    [SerializeField] Vector3 projectileSpawnOffset;

    private string enemyTag = "Enemy";

    public override void Execute(Transform transform, Transform playerTransform)
    {
        CharmProjectile projectile = 
            Instantiate(
                projectilePrefab, 
                transform.position + projectileSpawnOffset, 
                Quaternion.identity
                );

        projectile.Initialize(transform, playerTransform, damage, enemyTag);
    }
}
