
/// <summary>
/// Abstract base class for all runtime states
/// </summary>
public abstract class StateBase
{
    public virtual void Enter(StateContext? context) { }
    public virtual void Tick(StateContext? context) { }
    public virtual void Exit() { }
}
