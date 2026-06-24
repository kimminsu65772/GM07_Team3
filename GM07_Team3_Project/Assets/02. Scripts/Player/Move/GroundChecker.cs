using UnityEngine;

public sealed class GroundChecker : MonoBehaviour
{
    [Header("지면 검사")]
    [SerializeField] private float sphereRadius = 0.5f;
    [SerializeField] private float startOffset = 0.05f;
    [SerializeField] private float probeDistance = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    public bool hasGround { get; private set; }
    public Vector3 groundNormal { get; private set; } = Vector3.up;
    public float groundAngle { get; private set; }
    public Vector3 groundPoint { get; private set; }
    public float groundDistance { get; private set; }
    public Collider groundCollider { get; private set; }


    //[Header("런타임 검사 결과")]
    //[SerializeField] private bool hasGround;
    //[SerializeField] private Vector3 groundNormal = Vector3.up;
    //[SerializeField] private float groundAngle;
    //[SerializeField] private Vector3 groundPoint;
    //[SerializeField] private float groundDistance;
    //[SerializeField] private Collider groundCollider;

    //public bool HasGround => hasGround;
    //public Vector3 GroundNormal => groundNormal;
    //public float GroundAngle => groundAngle;
    //public Vector3 GroundPoint => groundPoint;
    //public float GroundDistance => groundDistance;
    //public Collider GroundCollider => groundCollider;

    public bool GroundCheck()
    {
        CalculateSphereCast(out Vector3 origin, out float castDistance);

        bool hasHit = Physics.SphereCast(
            origin,
            sphereRadius,
            Vector3.down,
            out RaycastHit hit,
            castDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore);

        if (!hasHit)
        {
            ResetHitInformation();
            return false;
        }

        hasGround = true;
        groundNormal = hit.normal.normalized;
        groundAngle = Vector3.Angle(Vector3.up, groundNormal);
        groundPoint = hit.point;
        groundCollider = hit.collider;

        groundDistance = Mathf.Max(0f, hit.distance - startOffset);

        return true;
    }

    private void CalculateSphereCast(out Vector3 origin, out float castDistance)
    {
        origin = transform.position + Vector3.up * (sphereRadius + startOffset);
        castDistance = startOffset + probeDistance;
    }

    private void ResetHitInformation()
    {
        hasGround = false;
        groundNormal = Vector3.up;
        groundAngle = 0f;
        groundPoint = Vector3.zero;
        groundDistance = 0f;
        groundCollider = null;
    }

    private void OnDrawGizmosSelected()
    {
        CalculateSphereCast(out Vector3 origin, out float castDistance);

        Vector3 end = origin + Vector3.down * castDistance;

        Gizmos.color = hasGround ? Color.green : Color.red;

        Gizmos.DrawWireSphere(origin, sphereRadius);
        Gizmos.DrawWireSphere(end, sphereRadius);
        Gizmos.DrawLine(origin, end);

        if (!hasGround)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(groundPoint, groundPoint + groundNormal * 2.5f);
    }
}
