using UnityEngine;

/// <summary>
/// Runtime data passed to states. Can be extended per enemy type.
/// </summary>
public class StateContext
{
    public Agent Agent;
    public Vector3 Position => Agent.transform.position;
    public Rigidbody Rigidbody => Agent.Rigidbody;

    public StateContext(Agent agent)
    {
        Agent = agent;
    }

    public T Get<T>() where T : Component
    {
        return Agent.Get<T>();
    }
}
