using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BlueLaserAttack : MonoBehaviour
{
    [SerializeField] private float slowAmount;
    [SerializeField] private float slowDuration;
    [SerializeField] private float BeamMaxDistance;
    [SerializeField] private float BeamDuration;
    public LineRenderer lineRenderer;
    public LayerMask blockinglayers;
    public LayerMask targetLayers;
    private Vector3 laserDirection;
    private float direction;
    private bool firing;

    public void Setup(float slowamount, float slowduration, float beamMax, float beamDuration)
    {
        slowAmount = slowamount;
        BeamMaxDistance = beamMax;
        BeamDuration = beamDuration;
        slowDuration = slowduration;
        UpdateLaserDirection();
    }

    public void SetDirection(float direction)
    {
        this.direction = Mathf.Sign(direction);
    }
    public void UpdateLaserDirection()
    {

        laserDirection = transform.position + (Vector3.right * direction * BeamMaxDistance);
    }

    public void SpawnLaser()
    {
        Debug.Log("FIRING MY LASOR!");

        if (!firing)
            StartCoroutine(Laser());
    }

    private void Update()
    {
        UpdateLaserDirection();
        if (firing)
        {
            CastRay();
        }
    }

    public IEnumerator Laser()
    {
        lineRenderer.enabled = true;
        firing = true;
        yield return new WaitForSeconds(BeamDuration);
        lineRenderer.enabled = false;
        firing = false;
    }

    public void CastRay()
    {
        Vector2 direction = (laserDirection - transform.position).normalized;
        RaycastHit2D blockingHit = Physics2D.Raycast(transform.position, direction, BeamMaxDistance, blockinglayers);

        if (blockingHit)
        {
            UpdateLaserPosition(transform.position, blockingHit.point);
        }
        else
        {
            UpdateLaserPosition(transform.position, laserDirection);
            Debug.Log("defaulting");
        }
        Vector2 laserEnd = blockingHit? blockingHit.point: laserDirection;

        RaycastHit2D targetHit = Physics2D.Raycast(transform.position,direction,Vector2.Distance(transform.position, laserEnd), targetLayers);

        if (targetHit)
        {
            ISlowable slowable = targetHit.collider.GetComponent<ISlowable>();

            if (slowable != null)
            {
                slowable.ApplySlow(slowAmount, slowDuration);
            }
        }
    }

    public void UpdateLaserPosition(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, laserDirection);
    }
}
