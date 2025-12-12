using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public EnemyAgent agent { get; private set; }
    public EnemyStateMachineConfig _agentConfig => agent != null ? agent.AgentConfig : null;
    public StateContext Context => agent != null ? agent.Context : null;

    private Dictionary<EnemyStateID, EnemyRuntimeState> runtimeStates = new();
    private EnemyRuntimeState currentState;
    public EnemyStateID CurrentStateID { get; private set; }

    public void Initialize(EnemyAgent agent)
    {
        this.agent = agent;

        runtimeStates.Clear();

        if (_agentConfig == null)
        {
            Debug.LogError($"{name} FSM has no AgentConfig assigned!");
            return;
        }

        foreach (var mapping in _agentConfig.stateMappings)
        {
            if (mapping.stateSO == null)
            {
                Debug.LogWarning($"AgentConfig {_agentConfig.name} has null StateSO for {mapping.stateID}");
                continue;
            }

            var runtimeState = mapping.stateSO.CreateRuntime(this);
            runtimeStates[mapping.stateID] = runtimeState;
        }

        if (!runtimeStates.TryGetValue(_agentConfig.defaultState, out var defaultState))
        {
            Debug.LogError($"Default state {_agentConfig.defaultState} not found!");
            return;
        }

        ChangeState(_agentConfig.defaultState, Context, true);
    }

    public T Get<T>() where T : Component => agent.Get<T>();

    public void ChangeState(EnemyStateID newStateID, StateContext context, bool force = false)
    {
        // Debug.Log($"{context?.Agent.EntityName} trying to change to {newStateID}");

        if (!runtimeStates.TryGetValue(newStateID, out var newState))
        {
            Debug.LogWarning($"EnemyFSM: state {newStateID} not found");
            return;
        }

        if (currentState == newState && !force) return;

        currentState?.Exit(context);
        currentState = newState;
        CurrentStateID = newStateID;
        currentState.Enter(context);
    }

    private void Update()
    {
        currentState?.Tick(Context);
    }

    public void HandleTargetDetected(TargetDetectedEvent evt)
    {
        currentState.HandleTargetDetected(evt);
    }
}
