using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // object to follow
    public Vector2 offset = new Vector2(0.0f, 0.0f);
    
    float zOffset;
    
    public float smoothSpeed = 0.01f; // Smoothing factor, 0 = no follow, 1 = snapping to target 

    void Start()
    {
        // save z of camera
        zOffset = transform.position.z;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3
        (
            target.position.x + offset.x,
            target.position.y + offset.y,
            zOffset
        );
        
        // move camera from its current position toward desired position, every frame
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}