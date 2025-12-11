public enum EnemyStateID
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Dead
}

/// <summary>
/// Base class for all enemy-specific state SOs
/// </summary>
public abstract class EnemyStateSO : StateSO
{
    /// <summary>
    /// Strongly typed enum for this SO
    /// </summary>
    public abstract EnemyStateID StateID { get; }

    public abstract EnemyRuntimeState CreateRuntime(EnemyFSM stateMachine);
}
