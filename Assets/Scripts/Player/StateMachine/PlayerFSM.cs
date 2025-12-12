using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    public PlayerAgent agent { get; private set; }
    public PlayerStateMachineConfig _agentConfig => agent != null ? agent.AgentConfig : null;
    public StateContext Context => agent != null ? agent.Context : null;

    private Dictionary<PlayerStateID, PlayerRuntimeState> runtimeStates = new();
    private PlayerRuntimeState currentState;
    private StateContext context;
    public PlayerStateID CurrentStateID { get; private set; }

    public void Initialize(PlayerAgent agent)
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

    public void ChangeState(PlayerStateID newStateID, StateContext context, bool force = false)
    {
        if (!runtimeStates.TryGetValue(newStateID, out var newState))
        {
            Debug.LogWarning($"PlayerFSM: state {newStateID} not found");
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
        currentState?.Tick(context);
    }
}
