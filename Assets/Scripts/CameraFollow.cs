using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    private Rigidbody targetRb;

    [Header("Framing")]
    public float followDistance = 7f;    
    public float height = 3f;            
    public float lookUpOffset = 1.2f;    

    [Header("Smoothing")]
    public float positionSmoothTime = 0.12f;
    public float rotationSmoothSpeed = 10f;

    [Header("Collision")]
    public LayerMask collisionMask;
    public float camCollisionRadius = 0.2f;

    private Vector3 _posVel;
    private Vector3 lastForward = Vector3.forward;

    void Start()
    {
        FindNewTarget();
    }

    void LateUpdate()
    {
        if (target == null)
        {
            FindNewTarget();
            if (target == null) return;
        }

        if (targetRb == null) targetRb = target.GetComponent<Rigidbody>();

        // Get player's flat velocity
        Vector3 flatVel = targetRb ? Vector3.ProjectOnPlane(targetRb.linearVelocity, Vector3.up) : Vector3.zero;

        // Determine forward direction
        Vector3 desiredForward = flatVel.sqrMagnitude > 0.15f * 0.15f ? flatVel.normalized : lastForward;
        if (flatVel.sqrMagnitude > 0.15f * 0.15f) lastForward = desiredForward;

        // Desired camera position
        Vector3 rawDesiredPos = target.position - desiredForward * followDistance + Vector3.up * height;

        // Focus point (where camera looks)
        Vector3 focusPoint = target.position + Vector3.up * lookUpOffset;

        // Collision check
        Vector3 camDir = rawDesiredPos - focusPoint;
        float camDist = camDir.magnitude;
        Vector3 desiredPos = rawDesiredPos;

        if (camDist > 0.001f)
        {
            camDir /= camDist;
            if (Physics.SphereCast(focusPoint, camCollisionRadius, camDir, out RaycastHit hit, camDist, collisionMask))
            {
                desiredPos = hit.point - camDir * 0.05f;
            }
        }

        // Smooth position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref _posVel, positionSmoothTime);

        // Smooth rotation
        Quaternion targetRot = Quaternion.LookRotation(focusPoint - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSmoothSpeed * Time.deltaTime);
    }

    public void FindNewTarget()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
        {
            target = playerObj.transform;
            targetRb = playerObj.GetComponent<Rigidbody>();
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetRb = newTarget.GetComponent<Rigidbody>();
    }
}