using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Attack[] attacks;

    public Attack CurrentAttack => attacks.Length > 0 ? attacks[Random.Range(0, attacks.Length)] : null;

    public void Attack()
    {
        if (CurrentAttack != null)
        {
            CurrentAttack.Execute(transform);
        }
    }
}
