using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewState", menuName = "Scriptables/FSM/Enemy/AttackState")]
public class EnemyAttackStateSO : EnemyStateSO
{
    public override EnemyStateID StateID => EnemyStateID.Attack;

    public override Type RuntimeType => typeof(EnemyAttackState);

    public float Damage;
    public float KnockbackForce;
    public float AttackDuration;

    public override EnemyRuntimeState CreateRuntime(EnemyFSM stateMachine)
    {
        return new EnemyAttackState(this, stateMachine);
    }
}
