using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewState", menuName = "Scriptables/FSM/Enemy/PatrolState")]
public class EnemyPatrolStateSO : EnemyStateSO
{
    public override EnemyStateID StateID => EnemyStateID.Patrol;

    public override Type RuntimeType => typeof(EnemyPatrolState);   

    public override EnemyRuntimeState CreateRuntime(EnemyFSM stateMachine)
    {
        return new EnemyPatrolState(this, stateMachine);
    }
}
