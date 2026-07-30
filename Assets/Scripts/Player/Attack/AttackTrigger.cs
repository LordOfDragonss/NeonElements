using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public System.Action<Collider2D> OnAttackTriggerEnter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnAttackTriggerEnter?.Invoke(collision);
    }
}
