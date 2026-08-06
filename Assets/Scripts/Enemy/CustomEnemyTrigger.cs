using UnityEngine;
using UnityEngine.Events;

public class CustomEnemyTrigger : MonoBehaviour
{
    public event System.Action<Collider2D> EnteredTrigger;
    public event System.Action<Collider2D> ExitedTrigger;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnteredTrigger?.Invoke(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        ExitedTrigger?.Invoke(collider);
    }
}

