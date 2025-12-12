using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewState", menuName = "Scriptables/FSM/Enemy/AttackState")]
public class EnemyAttackStateSO : EnemyStateSO
{
    public override EnemyStateID StateID => EnemyStateID.Attack;

    public override Type RuntimeType => typeof(EnemyAttackState);

    public float AttackDistance;
    public float Damage;
    public float WindupTime;

    public override EnemyRuntimeState CreateRuntime(EnemyFSM stateMachine)
    {
        return new EnemyAttackState(this, stateMachine);
    }
}
