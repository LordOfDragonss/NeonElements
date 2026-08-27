using UnityEngine;

public class ConditionalDoor : MonoBehaviour
{
    [SerializeField] private Condition[] conditions;
    [SerializeField] private Animator doorAnimator;

    private void Update()
    {
        if (AreConditionsSatisfied())
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private bool AreConditionsSatisfied()
    {
        foreach (Condition condition in conditions)
        {
            if (!condition.IsSatisfied())
            {
                return false;
            }
        }
        return true;
    }

    private void OpenDoor()
    {
        //doorAnimator.SetBool("isOpen", true);
        //temporary solution to make the door disappear when conditions are satisfied
        gameObject.SetActive(false);
    }

    private void CloseDoor()
    {
        //doorAnimator.SetBool("isOpen", false);
        //temporary solution to make the door appear when conditions are not satisfied
        gameObject.SetActive(true);
    }
}
