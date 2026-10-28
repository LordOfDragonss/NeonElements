using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class HammerController : MonoBehaviour, ISlowable
{
    private Coroutine slowCoroutine;
    private float originalspeed;
    private bool isSlowed;
    public enum HitboxSwingingSides
    {
        Left,
        Right,
        Both
    }

    public HitboxSwingingSides hitboxSwingingSide;

    public float swingSpeed = 1f;

    public AnimationClip hammerSwingAnimation;
    public AnimationClip hammerdoubleSwingAnimation;
    [HideInInspector]
    public Animator hammerAnimator;

    private void Start()
    {
        hammerAnimator = GetComponent<Animator>();
        SetSwingAnimation();
    }

    public void SetSwingAnimation()
    {

        switch (hitboxSwingingSide)
        {
            case HitboxSwingingSides.Left:
                hammerAnimator.Play(hammerSwingAnimation.name);
                break;
            case HitboxSwingingSides.Right:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                hammerAnimator.Play(hammerSwingAnimation.name);
                break;
            case HitboxSwingingSides.Both:
                hammerAnimator.Play(hammerdoubleSwingAnimation.name);
                break;
            default:
                hammerAnimator.Play(hammerSwingAnimation.name);
                break;
        }
        hammerAnimator.speed = swingSpeed;
    }

    public void ApplySlow(float amount, float duration)
    {
        if (slowCoroutine != null && isSlowed)
            StopCoroutine(slowCoroutine);

        slowCoroutine = StartCoroutine(Slow(amount, duration));
    }

    private IEnumerator Slow(float amount, float duration)
    {
        originalspeed = swingSpeed;
        isSlowed = true;

        hammerAnimator.speed = originalspeed * (1f - amount);

        yield return new WaitForSeconds(duration);

        hammerAnimator.speed = originalspeed;

        isSlowed = false;
        slowCoroutine = null;
    }

    //public void SetSwingAnimation()
    //{
    //    switch (hitboxSwingingSide)
    //    {
    //        case HitboxSwingingSides.Left:
    //            hammerAnimator.SetInteger("SwingType", 0);
    //            break;
    //        case HitboxSwingingSides.Right:
    //            transform.rotation = Quaternion.Euler(0, 180, 0);
    //            hammerAnimator.SetInteger("SwingType", 0);
    //            break;
    //        case HitboxSwingingSides.Both:
    //            hammerAnimator.SetInteger("SwingType", 1);
    //            break;
    //        default:
    //            hammerAnimator.SetInteger("SwingType", 0);
    //            break;
    //    }
    //}
}
