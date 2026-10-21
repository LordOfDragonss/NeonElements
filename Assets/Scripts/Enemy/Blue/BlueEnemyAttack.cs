using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attacks/BlueEnemyAttack")]
public class BlueEnemyAttack : Attack
{
    public float slowAmount;
    public float slowDuration;
    public float BeamMaxDistance;
    public float BeamDuration;

    public override void Execute(Transform transform, Transform playerTransform)
    {
        BlueLaserAttack laserAttack = transform.GetComponentInChildren<BlueLaserAttack>();
        if (laserAttack != null)
        {
            laserAttack.Setup(slowAmount, slowDuration, BeamMaxDistance, BeamDuration);
            laserAttack.SetDirection(Mathf.Sign(transform.localScale.x));
            laserAttack.SpawnLaser();
        }
    }
}
