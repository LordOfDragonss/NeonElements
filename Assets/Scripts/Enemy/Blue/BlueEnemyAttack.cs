using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attacks/BlueEnemyAttack")]
public class BlueEnemyAttack : Attack
{
    public float slowAmount;
    public float slowDuration;
    public float BeamMaxDistance;
    public float BeamDuration;

    public override void Execute(Enemy enemy, Transform playerTransform)
    {
        BlueLaserAttack laserAttack = enemy.transform.GetComponentInChildren<BlueLaserAttack>();
        if (laserAttack != null)
        {
            laserAttack.Setup(slowAmount, slowDuration, BeamMaxDistance, BeamDuration);
            laserAttack.SetDirection(Mathf.Sign(enemy.transform.localScale.x));
            laserAttack.SpawnLaser();
        }
    }
}
