[CreateAssetMenu(fileName = "NewState", menuName = "Scriptables/FSM/Enemy/DeathState")]
public class EnemyDeathStateSO : EnemyStateSO
{
    public override EnemyStateID StateID => EnemyStateID.Death;

    public override Type RuntimeType => typeof(EnemyDeathState);

    public override EnemyRuntimeState CreateRuntime(EnemyFSM stateMachine)
    {
        return new EnemyDeathState(this, stateMachine);
    }
}