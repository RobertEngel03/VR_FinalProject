using UnityEngine;

public class TargetDetectedEvent
{
    public Agent SourceAgent { get; }

    /// <summary>
    /// The detected target transform.
    /// </summary>
    public Transform Target { get; }
    
    public bool CanAttack { get; }

    /// <summary>
    /// Creates a new TargetDetectedEvent with the detected transform.
    /// </summary>
    public TargetDetectedEvent(Agent source, Transform target, bool inRange)
    {
        SourceAgent = source;
        Target = target;

        var attackReady = source.Get<WeaponManager>().ready;

        CanAttack = inRange && attackReady;
    }
}

public class DeathEvent
{
    public Agent DeadAgent { get; }

    public DeathEvent(Agent deadAgent)
    {
        DeadAgent = deadAgent;
    }
}
