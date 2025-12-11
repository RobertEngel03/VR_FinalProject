using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    [Header("Agent Config")]
    [SerializeField] private EnemyStateMachineConfig agentConfig;

    private Dictionary<EnemyStateID, EnemyRuntimeState> runtimeStates = new();
    private EnemyRuntimeState currentState;
    private StateContext context;
    public EnemyStateID CurrentStateID { get; private set; }

    public void Initialize(StateContext initContext)
    {
        context = initContext;
        runtimeStates.Clear();

        if (agentConfig == null)
        {
            Debug.LogError($"{name} FSM has no AgentConfig assigned!");
            return;
        }

        foreach (var mapping in agentConfig.stateMappings)
        {
            if (mapping.stateSO == null)
            {
                Debug.LogWarning($"AgentConfig {agentConfig.name} has null StateSO for {mapping.stateID}");
                continue;
            }

            var runtimeState = mapping.stateSO.CreateRuntime();
            runtimeStates[mapping.stateID] = runtimeState;
        }

        if (!runtimeStates.TryGetValue(agentConfig.defaultState, out var defaultState))
        {
            Debug.LogError($"Default state {agentConfig.defaultState} not found!");
            return;
        }

        ChangeState(agentConfig.defaultState, context, true);
    }

    public void ChangeState(EnemyStateID newStateID, StateContext context, bool force = false)
    {
        if (!runtimeStates.TryGetValue(newStateID, out var newState))
        {
            Debug.LogWarning($"EnemyFSM: state {newStateID} not found");
            return;
        }

        if (currentState == newState && !force) return;

        currentState?.Exit();
        currentState = newState;
        CurrentStateID = newStateID;
        currentState.Enter(context);
    }

    private void Update()
    {
        currentState?.Tick(context);
    }
}
