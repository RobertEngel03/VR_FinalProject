using UnityEngine;

/// <summary>
/// Enemy-specific agent
/// </summary>
public class EnemyAgent : Agent
{
    private EnemyFSM _stateMachine;

    [field: SerializeField] public EnemyStateMachineConfig AgentConfig { get; private set; }

    private StateContext context;

    public override void InitializeAgent()
    {
        if (container == null)
        {
            Debug.LogError($"{name} EnemyAgent missing DIContainer!");
            return;
        }

        // Setup context
        context = new StateContext(this);

        CreateFSM();
    }

    private void CreateFSM()
    {
        _stateMachine = gameObject.AddComponent<EnemyFSM>();
        _stateMachine.Initialize(this, context);
    }

    public override void Update()
    {
        base.Update();

        // Per-frame agent logic if needed
        // FSM Tick is handled internally in EnemyFSM
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // FOR DEBUGGING
    public void Idle() => _stateMachine.ChangeState(EnemyStateID.Idle, context);
    public void Patrol() => _stateMachine.ChangeState(EnemyStateID.Patrol, context);
    public void Chase() => _stateMachine.ChangeState(EnemyStateID.Chase, context);
    public void Attack() => _stateMachine.ChangeState(EnemyStateID.Attack, context);
}
