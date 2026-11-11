using UnityEngine;

[CreateAssetMenu(fileName = "PinkEnemyCharmAttack", menuName = "Scriptable Objects/Attacks/PinkEnemyCharmAttack")]
public class PinkEnemyCharmAttack : Attack
{
    [SerializeField] CharmProjectile projectilePrefab;
    [SerializeField] Vector3 projectileSpawnOffset;

    private string enemyTag = "Enemy";

    public override void Execute(Enemy enemy, Transform playerTransform)
    {
        CharmProjectile projectile = 
            Instantiate(
                projectilePrefab, 
                enemy.transform.position + projectileSpawnOffset, 
                Quaternion.identity
                );

        projectile.Initialize(enemy.transform, playerTransform, damage, enemyTag);
    }
}
