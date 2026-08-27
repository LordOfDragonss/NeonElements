using UnityEngine;

public abstract class Condition : MonoBehaviour, ICondition
{
    public abstract bool IsSatisfied();
}
