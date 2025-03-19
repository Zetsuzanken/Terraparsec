using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new(target.position.x + offset.x, transform.position.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    public void SnapToTarget()
    {
        if (target == null) return;
        Vector3 snapPosition = new(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        transform.position = snapPosition;
    }
}
