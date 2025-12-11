using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewState", menuName = "Scriptables/FSM/Enemy/IdleState")]
public class EnemyIdleStateSO : EnemyStateSO
{
    public override EnemyStateID StateID => EnemyStateID.Idle;

    public override Type RuntimeType => typeof(EnemyIdleState);

    public float MinDuration, MaxDuration;

    public override EnemyRuntimeState CreateRuntime(EnemyFSM stateMachine)
    {
        return new EnemyIdleState(this, stateMachine);
    }
}
