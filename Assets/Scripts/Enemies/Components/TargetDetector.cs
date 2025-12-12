using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class TargetDetector : MonoBehaviour
{
    private Agent _agent;

    [Header("Detection Settings")]
    [Tooltip("Layers that count as valid targets.")]
    public LayerMask targetLayers;

    [Tooltip("Maximum distance to check for targets.")]
    public float detectionRange = 5f;

    public float attackRadius = 1f;

    [Header("Read-Only")]
    [SerializeField] private Transform currentTarget;

    /// <summary>
    /// Returns the currently detected target transform.
    /// </summary>
    public Transform CurrentTarget => currentTarget;

    private void Awake()
    {
        _agent = GetComponent<Agent>();
    }

    private void Update()
    {
        Detect();
    }

    /// <summary>
    /// Runs detection logic and updates the CurrentTarget.
    /// </summary>
    private void Detect()
    {
        Collider[] hits;

        hits = Physics.OverlapSphere(transform.position, detectionRange, targetLayers, QueryTriggerInteraction.Collide);

        // Find closest collider
        float bestDist = Mathf.Infinity;
        Transform best = null;

        bool inRange = false;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < bestDist)
            {
                bestDist = dist;
                best = hit.transform;
            }

            inRange = dist <= attackRadius;
        }

        currentTarget = best;

        if (currentTarget != null) EventBus.Instance.Publish(new TargetDetectedEvent(_agent, currentTarget, inRange));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

#if UNITY_EDITOR
        if (currentTarget != null)
        {
            // Draw red line to target
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentTarget.position);

            // Draw small label with distance
            float dist = Vector3.Distance(transform.position, currentTarget.position);
            if (dist >= attackRadius) Gizmos.color = Color.blue;
            Vector3 labelPos = Vector3.Lerp(transform.position, currentTarget.position, 0.5f) + Vector3.up * 0.2f;
            Handles.color = Color.white;
            Handles.Label(labelPos, dist.ToString("F2"), new GUIStyle()
            {
                fontSize = 10,
                normal = new GUIStyleState() { textColor = Color.white }
            });
        }
#endif
    }
}
