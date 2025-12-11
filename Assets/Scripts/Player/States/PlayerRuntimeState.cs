using UnityEngine;
/// <summary>
/// Generic runtime state wrapping a StateSO
/// </summary>
/// <typeparam name="TSO">Type of the ScriptableObject</typeparam>
public abstract class PlayerRuntimeState : StateBase 
{
    protected readonly PlayerStateSO stateSO;

    protected PlayerRuntimeState(PlayerStateSO stateSO)
    {
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

    public override void Exit()
    {
        // Default: can be overridden
    }
}
