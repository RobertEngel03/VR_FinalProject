using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyRuntimeState
{
    private float _breakingDistance;

    public EnemyChaseState(EnemyChaseStateSO stateSO, EnemyFSM stateMachine) : base(stateSO, stateMachine)
    {
        _breakingDistance = stateSO.BreakingDistance;
    }

    public override void Tick(StateContext context)
    {
        base.Tick(context);

        var detector = stateMachine.Context.Get<TargetDetector>();
        var target = detector.CurrentTarget;

        if (target == null)
        {
            ChangeState(EnemyStateID.Idle, context);
            return;
        }

        float dist = Vector3.Distance(
            context.Position,
            target.position
        );

        // Movement logic (choose one region)
        ChaseRigidbody(target);
        // OR
        // ChaseNavmesh();
    }

    #region BASIC RIGIDBODY MOVEMENT
    private void ChaseRigidbody(Transform targetTransform)
    {
        Rigidbody rb = stateMachine.Rigidbody;
        if (rb == null)
        {
            Debug.LogWarning($"No Rigidbody found on {stateMachine.agent.EntityName}");
            return;
        }

        Vector3 dir = (targetTransform.position - rb.position).normalized;

        float distance = Vector3.Distance(rb.position, targetTransform.position);
        if (distance <= _breakingDistance)
        {
            return;
        }

        // TODO: Replace 5f with a reference to some configuration movement speed stat
        rb.MovePosition(rb.position + dir * 5f * Time.deltaTime);

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
