using System;
using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Aiming")]
    private bool IsAiming;
    [SerializeField]
    private GameObject GunVisual;
    [SerializeField]
    private CustomTrigger PlayerFollowRadius;
    [SerializeField]
    private float RotationOffset;

    [SerializeField]
    private CustomTrigger AttackRadius;
    [SerializeField]
    private LayerMask ObstacleLayer;
    [SerializeField]
    private LineRenderer AimLine;

    [SerializeField]
    private GameObject AimVisual;

    [SerializeField]
    private Transform FirePoint;

    private Transform CurrentTarget;
    [Header("Firing")]

    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private float WindupTime = 0.75f;
    [SerializeField] private float FireDelay = 2f;

    private Coroutine FiringRoutine;
    [Header("Bullet")]
    public GunFireStyle FireStyle;
    [SerializeField]
    private int BulletDamage;
    [SerializeField]
    private int BulletSpeed;
    [Header("Laser")]
    [SerializeField]
    private int LaserDamage;
    [SerializeField] private LineRenderer LaserVisual;
    [SerializeField] private LayerMask LaserHitMask;
    [SerializeField] private float LaserRange = 20f;
    [SerializeField] private float LaserDuration = 0.15f;


    private void Start()
    {
        PlayerFollowRadius.OnTriggerStay += FollowPlayer;
        AttackRadius.OnTriggerEnter += StartAiming;
        AttackRadius.OnTriggerExit += StopAiming;
    }

    private void Update()
    {
        if (CurrentTarget == null)
        {
            IsAiming = false;
            AimLine.enabled = false;
            return;
        }

        UpdateAimLine();
    }

    private void FollowPlayer(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;
        if (IsAiming) return;
        if (!HasLineOfSight(collider.transform)) return;

        Vector3 dir = collider.transform.position - GunVisual.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        GunVisual.transform.rotation =
            Quaternion.AngleAxis(angle + RotationOffset, Vector3.forward);
    }

    private void UpdateAimLine()
    {
        if (IsAiming) return;

        if (!HasLineOfSight(CurrentTarget))
        {
            AimLine.enabled = false;
            return;
        }

        AimLine.enabled = true;

        AimLine.SetPosition(0, FirePoint.position);
        AimLine.SetPosition(1, CurrentTarget.position);
    }


    private void StartAiming(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        CurrentTarget = collider.transform;
        AimLine.enabled = true;

        FiringRoutine = StartCoroutine(FireRoutine());
    }

    private void StopAiming(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        CurrentTarget = null;
        IsAiming = false;

        AimLine.enabled = false;

        if (FiringRoutine != null)
        {
            StopCoroutine(FiringRoutine);
            FiringRoutine = null;
        }
    }

    private bool HasLineOfSight(Transform target)
    {
        Vector2 start = transform.position;
        Vector2 direction = ((Vector2)target.position - start).normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            start,
            direction,
            Vector2.Distance(start, target.position),
            ObstacleLayer
        );

        return hit.collider == null;
    }

    private IEnumerator FireRoutine()
    {
        while (CurrentTarget != null)
        {
            IsAiming = false;

            // Wait until the player is visible.
            while (CurrentTarget != null && !HasLineOfSight(CurrentTarget))
            {
                yield return null;
            }

            if (CurrentTarget == null) break;

            // Cooldown.
            yield return new WaitForSeconds(FireDelay);

            if (CurrentTarget == null) break;

            if (!HasLineOfSight(CurrentTarget))
                continue;

            // Begin wind-up and lock the gun.
            IsAiming = true;

            float timer = 0f;
            bool interrupted = false;

            while (timer < WindupTime)
            {
                if (CurrentTarget == null || !HasLineOfSight(CurrentTarget))
                {
                    interrupted = true;
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            // Unlock if the wind-up was interrupted.
            if (interrupted)
            {
                IsAiming = false;
                continue;
            }

            if (CurrentTarget == null) break;

            if (FireStyle == GunFireStyle.Bullet)
                ShootBullet();
            else if (FireStyle == GunFireStyle.Laser)
                ShootLaser();

            IsAiming = false;
        }

        // Clean up when the routine finishes.
        IsAiming = false;
        AimLine.enabled = false;
        FiringRoutine = null;
    }


    private void ShootBullet()
    {
        Vector2 direction = (CurrentTarget.position - FirePoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Bullet bullet = Instantiate(
            ProjectilePrefab,
            FirePoint.position,
            Quaternion.Euler(0, 0, angle)
        ).GetComponent<Bullet>();
        bullet.Damage = BulletDamage;
        bullet.speed = BulletSpeed;
    }

    private void ShootLaser()
    {
        Vector2 start = FirePoint.position;
        Vector2 direction = -FirePoint.right;

        RaycastHit2D hit = Physics2D.Raycast(
            start,
            direction,
            LaserRange,
            LaserHitMask
        );

        Vector2 end = hit.collider != null
            ? hit.point
            : start + direction * LaserRange;

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            hit.collider.GetComponentInParent<HealthSystem>().TakeDamage(LaserDamage);
        }

        StartCoroutine(ShowLaser(start, end));
    }

    private IEnumerator ShowLaser(Vector2 start, Vector2 end)
    {
        LaserVisual.SetPosition(0, start);
        LaserVisual.SetPosition(1, end);
        LaserVisual.enabled = true;

        yield return new WaitForSeconds(LaserDuration);

        LaserVisual.enabled = false;
    }
}
