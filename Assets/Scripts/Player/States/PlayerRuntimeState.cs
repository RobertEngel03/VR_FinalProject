using UnityEngine;
/// <summary>
/// Generic runtime state wrapping a StateSO
/// </summary>
/// <typeparam name="TSO">Type of the ScriptableObject</typeparam>
public abstract class PlayerRuntimeState : StateBase 
{
    protected readonly PlayerFSM stateMachine;
    protected readonly PlayerStateSO stateSO;

    protected PlayerRuntimeState(PlayerStateSO stateSO, PlayerFSM stateMachine)
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
        // Default: can be overridden in concrete runtime states
    }

    public override void Tick(StateContext? context)
    {
        // Default: can be overridden
    }

    public override void Exit(StateContext? context)
    {
        // Default: can be overridden
    }

    protected void ChangeState(PlayerStateID stateID, StateContext context, bool force = false)
        => stateMachine.ChangeState(stateID, context, force);
}
