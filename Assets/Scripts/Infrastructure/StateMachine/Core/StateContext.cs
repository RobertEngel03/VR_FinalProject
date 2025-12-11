using UnityEngine;
/// <summary>
/// Runtime data passed to states. Can be extended per enemy type.
/// </summary>
public class StateContext
{
    public Agent Agent;

    public StateContext(Agent agent)
    {
        Agent = agent;
    }
}
