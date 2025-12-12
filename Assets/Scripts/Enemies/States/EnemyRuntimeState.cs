using UnityEngine;

/// <summary>
/// Generic runtime state wrapping a StateSO
/// </summary>
/// <typeparam name="TSO">Type of the ScriptableObject</typeparam>
public abstract class EnemyRuntimeState : StateBase 
{
    protected readonly EnemyFSM stateMachine;
    protected readonly EnemyStateSO stateSO;

    private Animator Animator => stateMachine.Animator;

    private readonly int StateHash = Animator.StringToHash("State");

    protected EnemyRuntimeState(EnemyStateSO stateSO, EnemyFSM stateMachine)
    {
        this.stateMachine = stateMachine;
        this.stateSO = stateSO;
    }

    /// <summary>
    /// Accessor for SO name
    /// </summary>
    public string Name => stateSO.Name;

    /// <summary>
    /// Optional override AnimationClip
    /// </summary>
    public AnimationClip OverrideAnimationClip => stateSO.OverrideAnimationClip;

    public override void Enter(StateContext? context)
    {
        base.Enter(context);

        Animator.SetInteger(StateHash, (int)stateSO.StateID);

        // Default: can be overridden in concrete runtime states
    }

    public override void Tick(StateContext? context)
    {
        base.Tick(context);

        Debug.Log($"{context?.Agent.EntityName} Currently in {GetType()}");
        // Default: can be overridden
    }

    public override void Exit(StateContext? context)
    {
        base.Exit(context);

        // Debug.Log($"{context?.Agent.EntityName} exited {GetType()}");
        // Default: can be overridden
    }

    protected void ChangeState(EnemyStateID stateID, StateContext context, bool force = false)
        => stateMachine.ChangeState(stateID, context, force);

    public virtual void HandleTargetDetected(TargetDetectedEvent evt)
    {
        Debug.Log($"{evt.SourceAgent.EntityName} detected {evt.Target.name}");

        if (evt.CanAttack)
        {
            ChangeState(EnemyStateID.Attack, stateMachine.Context);
            return;
        }
    }
}
