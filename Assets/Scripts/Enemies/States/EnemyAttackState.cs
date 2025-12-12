using UnityEngine;

public class EnemyAttackState : EnemyRuntimeState
{
    public float damage { get; private set; }
    public float knockbackForce { get; private set; }

    private float _duration;
    private float _timer;

    public EnemyAttackState(EnemyAttackStateSO stateSO, EnemyFSM stateMachine) : base(stateSO, stateMachine)
    {
        damage = stateSO.Damage;
        knockbackForce = stateSO.KnockbackForce;
        _duration = stateSO.AttackDuration;
    }

    public override void Enter(StateContext context)
    {
        base.Enter(context);

        var weaponManager = context.Get<WeaponManager>();
        weaponManager.PlayAttack(this, 0.5f);
    }

    public override void Tick(StateContext context)
    {
        base.Tick(context);

        _timer += Time.deltaTime;

        if (_timer >= _duration)
        {
            ChangeState(EnemyStateID.Idle, context);
        }
    }

    public override void Exit(StateContext context)
    {
        base.Exit(context);

        _timer = 0f;
    }

    public override void HandleTargetDetected(TargetDetectedEvent evt)
    {
        if (evt.CanAttack) return;
    }
}