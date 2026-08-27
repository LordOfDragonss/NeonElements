using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    public Animator animator;

    public LayerMask playerLayer;

    public UnityEvent onSwitch;

    public bool OneShot;
    private bool hasSwitched = false;
    public void SwitchLever()
    {
        animator.SetTrigger("FlipLever");
        onSwitch?.Invoke();
        if (OneShot)
        {
            hasSwitched = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (OneShot && hasSwitched)
            {
                return;
            }
            SwitchLever();
        }
    }
}
