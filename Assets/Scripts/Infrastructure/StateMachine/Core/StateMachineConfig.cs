using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentConfig", menuName = "AI/FSM/AgentConfig")]
public abstract class StateMachineConfig<TEnum> : ScriptableObject where TEnum : Enum
{
    [Header("Default State")]
    public TEnum defaultState;

    [Header("States")]
    public List<StateMapping> stateMappings = new List<StateMapping>();

    [System.Serializable]
    public struct StateMapping
    {
        public TEnum stateID;
        public StateSO stateSO;
    }

    /// <summary>
    /// Helper to fetch a StateSO by enum
    /// </summary>
    public StateSO GetStateSO(TEnum stateID)
    {
        foreach (var mapping in stateMappings)
        {
            if (mapping.stateID.Equals(stateID))
                return mapping.stateSO;
        }
        return null;
    }
}
