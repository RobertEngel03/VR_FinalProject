/* I CHOSE NOT TO USE THIS BUT ITS HERE FOR YOU GUYS TO LEARN FROM
/// <summary>
/// Generic, abstract base class for finite state machines (FSMs) in Unity.
/// 
/// This class provides a framework for runtime states driven by ScriptableObjects.
/// It maps a set of enum-based state IDs to their corresponding runtime state instances,
/// handles state transitions, and updates the current state each frame.
///
/// Type Parameters:
/// - TStateEnum: The enum type representing possible states (e.g., PlayerStateID, EnemyStateID).
/// - TRuntime: The base type for all runtime state classes (must inherit from StateBase).
/// - TConfig: The ScriptableObject type holding configuration for this FSM (must inherit from StateMachineConfig<TStateEnum>).
///
/// Key Features:
/// - Stores runtime states in a dictionary keyed by enum.
/// - Handles state transitions via ChangeState, including optional forced re-entry.
/// - Calls Enter, Tick, and Exit methods on states automatically.
/// - Can be initialized with a StateContext that contains runtime data (agent transform, target, deltaTime, etc.).
/// - Includes safety checks to ensure runtime states are properly configured and instantiated.
///
/// Notes:
/// - Runtime states are instantiated via Activator.CreateInstance from the RuntimeType specified in the StateSO.
/// - Default state is automatically entered on Initialize.
/// - Tick is called automatically in Update each frame.
///
/// Usage:
/// - Derive a concrete FSM class (e.g., PlayerFSM, EnemyFSM) specifying the enum, runtime state, and config types.
/// - Assign a StateMachineConfig ScriptableObject in the inspector.
/// - Call Initialize() at runtime with the appropriate StateContext.
/// </summary>
public abstract class StateMachine<TStateEnum, TRuntime, TConfig> : MonoBehaviour
    where TStateEnum : Enum
    where TRuntime : StateBase
    where TConfig : StateMachineConfig<TStateEnum>
{
    protected Dictionary<TStateEnum, TRuntime> runtimeStates = new();
    protected TRuntime currentState;
    public TStateEnum CurrentStateID { get; private set; }

    [Header("Agent Config")]
    [SerializeField] protected TConfig agentConfig;

    protected StateContext context;

    /// <summary>
    /// Initialize FSM with context
    /// </summary>
    public virtual void Initialize(StateContext initContext)
    {
        if (agentConfig == null)
        {
            Debug.LogError($"{name} FSM has no AgentConfig assigned!");
            return;
        }

        context = initContext;
        runtimeStates.Clear();

        foreach (var mapping in agentConfig.stateMappings)
        {
            if (mapping.stateSO == null)
            {
                Debug.LogWarning($"AgentConfig {agentConfig.name} has null StateSO for {mapping.stateID}");
                continue;
            }

            Type runtimeType = mapping.stateSO.RuntimeType;

            if (runtimeType == null)
            {
                Debug.LogError($"StateSO {mapping.stateSO.name} has null RuntimeType!");
                continue;
            }

            if (runtimeType.IsAbstract)
            {
                Debug.LogError($"StateSO {mapping.stateSO.name} RuntimeType cannot be abstract!");
                continue;
            }

            if (!typeof(TRuntime).IsAssignableFrom(runtimeType))
            {
                Debug.LogError($"StateSO {mapping.stateSO.name} RuntimeType {runtimeType} is not assignable to {typeof(TRuntime)}");
                continue;
            }

            try
            {
                var runtimeState = (TRuntime)Activator.CreateInstance(runtimeType, mapping.stateSO);
                runtimeStates[mapping.stateID] = runtimeState;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create runtime state for {mapping.stateSO.name}: {e}");
            }
        }

        // Set default state
        if (!runtimeStates.TryGetValue(agentConfig.defaultState, out var defaultState))
        {
            Debug.LogError($"Default state {agentConfig.defaultState} not found in runtimeStates!");
            return;
        }

        ChangeState(agentConfig.defaultState, context, true);
    }

    public virtual void ChangeState(TStateEnum newStateID, StateContext context, bool force = false)
    {
        if (!runtimeStates.TryGetValue(newStateID, out var newState))
        {
            Debug.LogWarning($"StateMachine: state {newStateID} not found");
            return;
        }

        if (currentState == newState && !force) return;

        currentState?.Exit();
        currentState = newState;
        CurrentStateID = newStateID;
        currentState.Enter(context);
    }

    protected virtual void Update()
    {
        if (currentState != null && context != null)
        {
            currentState.Tick(context);
        }
    }
}
*/
