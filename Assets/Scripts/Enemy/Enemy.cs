using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;

    public AbilityType EnemyType;
    public Attack[] Attacks;
    private float[] nextAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nextAttackTime = new float[Attacks.Length];
    }

    public bool CanAttack()
    {
        for (int i = 0; i < Attacks.Length; i++)
            if (Time.time >= nextAttackTime[i]) return true;
        return false;
    }

    public bool TryAttack()
    {
        if (player == null || Attacks.Length == 0) return false;

        for (int i = 0; i < Attacks.Length; i++)
        {
            if (Time.time < nextAttackTime[i]) continue;

            Attacks[i].Execute(transform, player.transform);
            nextAttackTime[i] = Time.time + Attacks[i].Cooldown;
            return true;
        }

        return false;
    }
}
