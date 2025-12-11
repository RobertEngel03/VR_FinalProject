using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic runtime state wrapping a StateSO
/// </summary>
/// <typeparam name="TSO">Type of the ScriptableObject</typeparam>
public abstract class EnemyRuntimeState : StateBase 
{
    protected readonly EnemyFSM stateMachine;
    protected readonly EnemyStateSO stateSO;

    protected EnemyRuntimeState(EnemyStateSO stateSO, EnemyFSM stateMachine)
    {
        this.stateMachine = stateMachine;
        this.stateSO = stateSO;
    }

    /// <summary>
    /// Accessor for SO name
    /// </summary>
    public string Name => stateSO.Name;

    /// <summary>
    /// Optional override AnimationClip
    /// </summary>
    public AnimationClip OverrideAnimationClip => stateSO.OverrideAnimationClip;

    public override void Enter(StateContext? context)
    {
        base.Enter(context);

        Debug.Log($"{context?.Agent.EntityName} Entered {GetType()}");
        // Default: can be overridden in concrete runtime states
    }

    public override void Tick(StateContext? context)
    {
        base.Tick(context);

        Debug.Log($"{context?.Agent.EntityName} Currently in {GetType()}");
        // Default: can be overridden
    }

    public override void Exit(StateContext? context)
    {
        base.Exit(context);

        Debug.Log($"{context?.Agent.EntityName} exited {GetType()}");
        // Default: can be overridden
    }
}

public class EnemyIdleState : EnemyRuntimeState
{
    private float _minDuration, _maxDuration;

    public EnemyIdleState(EnemyIdleStateSO stateSO, EnemyFSM stateMachine) : base(stateSO, stateMachine)
    {
        _minDuration = stateSO.MinDuration;
        _maxDuration = stateSO.MaxDuration;
    }
}

public class EnemyPatrolState : EnemyRuntimeState
{
    private Transform _transform;

    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f; // seconds between picking a new direction

    private Vector3 moveDirection;
    private float timer;

    public EnemyPatrolState(EnemyPatrolStateSO stateSO, EnemyFSM stateMachine) : base(stateSO, stateMachine)
    {
        _transform = stateMachine.transform;
    }

    public override void Enter(StateContext context)
    {
        base.Enter(context);

        PickRandomDirection();
        timer = 0f;
    }

    public override void Tick(StateContext context)
    {
        base.Tick(context);

        timer += Time.deltaTime;

        // Pick a new random direction periodically
        if (timer >= changeDirectionInterval)
        {
            PickRandomDirection();
            timer = 0f;
        }

        // Move the enemy
        _transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Optional: rotate smoothly towards movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void PickRandomDirection()
    {
        // Random point on the unit circle for XZ plane
        float angle = Random.Range(0f, Mathf.PI * 2f);
        moveDirection = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)).normalized;
    }
}

public class EnemyChaseState : EnemyRuntimeState
{
    private float _chaseDistance;

    public EnemyChaseState(EnemyChaseStateSO stateSO, EnemyFSM stateMachine) : base(stateSO, stateMachine)
    {
        _chaseDistance = stateSO.ChaseDistance;
    }
}

public class EnemyAttackState : EnemyRuntimeState
{
    private float _attackDistance;
    private float _damage;

    public EnemyAttackState(EnemyAttackStateSO stateSO, EnemyFSM stateMachine) : base(stateSO, stateMachine)
    {
        _attackDistance = stateSO.AttackDistance;
        _damage = stateSO.Damage;
    }
}