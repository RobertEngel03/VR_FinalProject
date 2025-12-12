using UnityEngine;

public class EnemyPatrolState : EnemyRuntimeState
{
    private Transform _transform;

    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    public float idleInterval = 5f; 

    private Vector3 moveDirection;
    private float _dirTimer, _idleTimer;

    private Rigidbody _rigidbody => stateMachine.Rigidbody;

    public EnemyPatrolState(EnemyPatrolStateSO stateSO, EnemyFSM stateMachine) : base(stateSO, stateMachine)
    {
        _transform = stateMachine.transform;
    }

    public override void Enter(StateContext context)
    {
        base.Enter(context);

        _dirTimer = 0f;
        _idleTimer = 0f;
        PickRandomDirection();
    }

    public override void Tick(StateContext context)
    {
        base.Tick(context);

        _dirTimer += Time.deltaTime;
        _idleTimer += Time.deltaTime;

        // Pick a new direction periodically
        if (_dirTimer >= changeDirectionInterval)
        {
            PickRandomDirection();
        }

        if (_idleTimer >= idleInterval)
        {
            ChangeState(EnemyStateID.Idle, context);
        }

        Vector3 horizontalDir = new Vector3(moveDirection.x, 0f, moveDirection.z).normalized;

        Vector3 targetPos = _rigidbody.position + horizontalDir * moveSpeed * Time.deltaTime;

        targetPos.y = _rigidbody.position.y;

        _rigidbody.MovePosition(targetPos);

        // --- ROTATION (smooth + stable) ---
        if (horizontalDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(horizontalDir, Vector3.up);
            _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, targetRot, Time.deltaTime * 5f));
        }
    }

    public override void Exit(StateContext context)
    {
        base.Exit(context);

        _dirTimer = 0f;
        _idleTimer = 0f;
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
