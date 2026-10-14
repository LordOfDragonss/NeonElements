using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Attack[] attacks;
    private GameObject player;

    // Doesnt work if enemy has multiple attacks: FIX MS
    public Attack CurrentAttack => attacks.Length > 0 ? attacks[Random.Range(0, attacks.Length)] : null;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Attack()
    {
        if (CurrentAttack != null)
        {
            CurrentAttack.Execute(transform, player.transform);
        }
    }
}
