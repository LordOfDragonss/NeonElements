using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    public float ConveyorSpeed = 1f;
    public float ConveyorForce = 1f;
    public bool ConveyorEnabled = true;
    public float ConveyorSpeedMultiplier = 1f;
    public bool FlipDirection = false;
    private SurfaceEffector2D effector;

    public void Start()
    {
        effector = GetComponent<SurfaceEffector2D>();
        if (effector != null)
        {
            effector.enabled = ConveyorEnabled;
            SetConveyorSpeed(ConveyorSpeed);
            SetConveyorForce(ConveyorForce);
        }
    }

    public void Update()
    {
        if (effector == null) return;

        SetConveyorSpeed(ConveyorSpeed);
        effector.enabled = ConveyorEnabled;
    }

    public void SetConveyorSpeed(float speed)
    {
        float direction = FlipDirection ? -1f : 1f;
        effector.speed = speed * ConveyorSpeedMultiplier * direction;
    }
    public void SetConveyorForce(float force)
    {
        effector.forceScale = force;
    }

    public void FlipConveyorDirection()
    {
        FlipDirection = !FlipDirection;
    }
}
