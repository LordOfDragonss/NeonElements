using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attack")]
public abstract class Attack : ScriptableObject
{
    [SerializeField] protected float cooldown = 1f;
    [SerializeField] protected float damage;

    public float Cooldown => cooldown;

    public abstract void Execute(Transform transform);
}