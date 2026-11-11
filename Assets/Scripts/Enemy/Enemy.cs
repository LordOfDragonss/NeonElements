using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    public HealthSystem healthSystem;
    public EnemyStateMachine stateMachine;

    public AbilityType EnemyType;
    public Attack[] Attacks;
    private float[] nextAttackTime;

    public bool IsBusy { get; set; }
    public bool IsImmune { get; set; }
    public bool IsAttackBlocked { get; set; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        healthSystem = GetComponent<HealthSystem>();
        stateMachine = GetComponent<EnemyStateMachine>();
    }

    void Start()
    {
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
        if ((player == null || Attacks.Length == 0)) return false;

        for (int i = 0; i < Attacks.Length; i++)
        {
            if (Time.time < nextAttackTime[i]) continue;

            Attacks[i].Execute(this, player.transform);
            nextAttackTime[i] = Time.time + Attacks[i].Cooldown;
            return true;
        }

        return false;
    }

    public void OnTakeDamage(int damage)
    {
        if (!IsImmune || !IsAttackBlocked)
            healthSystem.TakeDamage(damage);

        if (healthSystem.currentHealth <= 0)
            stateMachine.enemyState = EnemyStateMachine.EnemyState.Death;
    }
}
