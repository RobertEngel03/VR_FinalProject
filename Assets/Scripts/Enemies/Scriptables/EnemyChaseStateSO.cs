using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewState", menuName = "Scriptables/FSM/Enemy/ChaseState")]
public class EnemyChaseStateSO : EnemyStateSO
{
    public override EnemyStateID StateID => EnemyStateID.Chase;

    public override Type RuntimeType => typeof(EnemyChaseState);

    public float BreakingDistance;

    public override EnemyRuntimeState CreateRuntime(EnemyFSM stateMachine)
    {
        return new EnemyChaseState(this, stateMachine);
    }
}
