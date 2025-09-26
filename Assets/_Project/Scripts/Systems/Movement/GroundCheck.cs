using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private PlayerConfig config;
    public bool IsGrounded { get; private set; }
    public Vector3 GroundNormal { get; private set; } = Vector3.up;

    void FixedUpdate()
    {
        var pos = transform.position + config.groundCheckOffset;
        IsGrounded = Physics.CheckSphere(pos, config.groundCheckRadius, config.groundMask, QueryTriggerInteraction.Ignore);
        if (Physics.Raycast(pos, Vector3.down, out var hit, 0.6f, config.groundMask, QueryTriggerInteraction.Ignore))
            GroundNormal = hit.normal;
        else
            GroundNormal = Vector3.up;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!config) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + config.groundCheckOffset, config.groundCheckRadius);
    }
#endif
}
