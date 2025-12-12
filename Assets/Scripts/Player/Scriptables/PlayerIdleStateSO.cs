using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewState", menuName = "Scriptables/FSM/IdleState")]
public class PlayerIdleStateSO : PlayerStateSO
{
    public override PlayerStateID StateID => PlayerStateID.Idle;

    public override Type RuntimeType => typeof(PlayerIdleState);

    public float MinDuration, MaxDuration;

    public override PlayerRuntimeState CreateRuntime(PlayerFSM stateMachine)
    {
        return new PlayerIdleState(this, stateMachine);
    }
}
