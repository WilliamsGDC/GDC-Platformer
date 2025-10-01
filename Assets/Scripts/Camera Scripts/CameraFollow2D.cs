using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform followTarget; 
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);
    public float smoothTime = 0.1f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (followTarget == null) return;

        Vector3 targetPosition = followTarget.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
