public enum PlayerStateID
{
    Idle,
    Walk,
    CastSpell,
    Grab
}

/// <summary>
/// Base class for all player-specific state SOs
/// </summary>
public abstract class PlayerStateSO : StateSO
{
    /// <summary>
    /// Strongly typed enum for this SO
    /// </summary>
    public abstract PlayerStateID StateID { get; }

    public abstract PlayerRuntimeState CreateRuntime();
}
