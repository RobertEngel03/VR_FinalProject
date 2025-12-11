using UnityEngine;
/// <summary>
/// Runtime data passed to states. Can be extended per enemy type.
/// </summary>
public class StateContext
{
    public Transform AgentTransform { get; private set; }
    public Transform TargetTransform { get; private set; }
    public float DeltaTime { get; private set; }

    public StateContext(Transform agent, Transform target, float deltaTime)
    {
        AgentTransform = agent;
        TargetTransform = target;
        DeltaTime = deltaTime;
    }

    /// <summary>
    /// Updates deltaTime each frame.
    /// </summary>
    public void UpdateDeltaTime(float deltaTime)
    {
        DeltaTime = deltaTime;
    }

    // Add more runtime data as needed (speed, health, etc.)
}
