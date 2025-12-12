using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

        // Debug.Log($"{context?.Agent.EntityName} Entered {GetType()}");
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

        // Debug.Log($"{context?.Agent.EntityName} exited {GetType()}");
        // Default: can be overridden
    }

    protected void ChangeState(EnemyStateID stateID, StateContext context, bool force = false)
        => stateMachine.ChangeState(stateID, context, force);

    public virtual void HandleTargetDetected(TargetDetectedEvent evt)
    {
        Debug.Log($"{evt.SourceAgent.EntityName} detected {evt.Target.name}");

        if (evt.CanAttack)
        {
            ChangeState(EnemyStateID.Attack, stateMachine.Context);
            return;
        }
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

    public override void HandleTargetDetected(TargetDetectedEvent evt)
    {
        base.HandleTargetDetected(evt);

        ChangeState(EnemyStateID.Chase, stateMachine.Context);
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

    public override void HandleTargetDetected(TargetDetectedEvent evt)
    {
        base.HandleTargetDetected(evt);

        ChangeState(EnemyStateID.Chase, stateMachine.Context);
    }
}

public class EnemyChaseState : EnemyRuntimeState
{
    private float _chaseDistance;

    public EnemyChaseState(EnemyChaseStateSO stateSO, EnemyFSM stateMachine) : base(stateSO, stateMachine)
    {
        _chaseDistance = stateSO.ChaseDistance;
    }

    public override void Tick(StateContext context)
    {
        base.Tick(context);

        var detector = stateMachine.Context.Get<TargetDetector>();
        var target = detector.CurrentTarget;

        if (target == null)
        {
            ChangeState(EnemyStateID.Patrol, context);
            return;
        }

        float dist = Vector3.Distance(
            context.Position,
            target.position
        );

        if (dist > _chaseDistance)
        {
            // Return to idle or patrol etc.
            stateMachine.ChangeState(EnemyStateID.Idle, context);
            return;
        }

        // Movement logic (choose one region)
        ChaseRigidbody(target);
        // OR
        // ChaseNavmesh();
    }

    #region BASIC RIGIDBODY MOVEMENT
    private void ChaseRigidbody(Transform targetTransform)
    {
        Rigidbody rb = stateMachine.Context.Rigidbody;
        if (rb == null)
        {
            Debug.LogWarning($"No Rigidbody found on {stateMachine.agent.EntityName}");
            return;
        }

        Vector3 dir = (targetTransform.position - rb.position).normalized;

        // Basic forward velocity
        // TODO: Replace 5f with a reference to some configuration movement speed stat
        rb.MovePosition(rb.position + dir * 5f * Time.deltaTime);
        Debug.Log($"Moving {dir} -> {5f}");

        // Flatten direction so rotation only affects Y axis
        Vector3 flatDir = new Vector3(dir.x, 0f, dir.z);

        if (flatDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(flatDir);
            rb.rotation = targetRot;
        }

    }
    #endregion


    #region NAVMESH MOVEMENT
    private void ChaseNavmesh()
    {
        var detector = stateMachine.Get<TargetDetector>();
        var target = detector.CurrentTarget;

        NavMeshAgent agent = stateMachine.Get<NavMeshAgent>();
        if (agent == null)
            return;

        agent.isStopped = false;
        agent.speed = 5f; // TODO: USE CONFIG SPEED
        agent.SetDestination(target.position);
    }
    #endregion
}

public class EnemyAttackState : EnemyRuntimeState
{
    private float _attackDistance;
    private float _damage;
    private float _windupTime;

    public EnemyAttackState(EnemyAttackStateSO stateSO, EnemyFSM stateMachine) : base(stateSO, stateMachine)
    {
        _attackDistance = stateSO.AttackDistance;
        _damage = stateSO.Damage;
        _windupTime = stateSO.WindupTime;
    }

    public override void Enter(StateContext context)
    {
        base.Enter(context);

        context.Agent.StartCoroutine(PerformAttack(context));
    }

    private IEnumerator PerformAttack(StateContext context)
    {
        // Optional windup delay
        yield return new WaitForSeconds(_windupTime);

        // Simple overlap check in front of the enemy
        Vector3 center = context.Position + context.Agent.transform.forward * (_attackDistance / 2f);
        Vector3 halfExtents = new Vector3(1f, 1f, _attackDistance / 2f); // tweak for your enemy size

        Collider[] hits = Physics.OverlapBox(center, halfExtents, context.Agent.transform.rotation, LayerMask.NameToLayer("Player"));

        foreach (var hit in hits)
        {
            Debug.Log($"{context.Agent.EntityName} hit {hit.name} for {_damage} damage");
        }

        // Attack is done → go back to another state
        // ChangeState(EnemyStateID.Chase, context); 
    }
}