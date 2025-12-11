public interface IState
{
    /// <summary>
    /// Called once when the state is entered.
    /// </summary>
    /// <param name="context">Optional runtime context data</param>
    void Enter(StateContext context);

    /// <summary>
    /// Called every frame the state is active.
    /// </summary>
    /// <param name="context">Optional runtime context data</param>
    void Tick(StateContext context);

    /// <summary>
    /// Called once when exiting the state.
    /// </summary>
    void Exit();
}