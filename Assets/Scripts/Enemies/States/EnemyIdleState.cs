using UnityEngine;

public class EnemyIdleState : EnemyRuntimeState
{
    private float _minDuration, _maxDuration;
    private float _idleDuration;

    private float _timer;

    public EnemyIdleState(EnemyIdleStateSO stateSO, EnemyFSM stateMachine) : base(stateSO, stateMachine)
    {
        _minDuration = stateSO.MinDuration;
        _maxDuration = stateSO.MaxDuration;
    }

    public override void Enter(StateContext context)
    {
        base.Enter(context);

        _idleDuration = Random.Range(_minDuration, _maxDuration);
        _timer = 0f;
    }

    public override void Tick(StateContext context)
    {
        base.Tick(context);

        _timer += Time.deltaTime;

        if (_timer > _idleDuration)
        {
            _timer = 0f;
            ChangeState(EnemyStateID.Patrol, context);
        }
    }

    public override void HandleTargetDetected(TargetDetectedEvent evt)
    {
        base.HandleTargetDetected(evt);

        ChangeState(EnemyStateID.Chase, stateMachine.Context);
    }
}
