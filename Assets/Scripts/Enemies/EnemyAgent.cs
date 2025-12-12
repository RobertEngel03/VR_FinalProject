using UnityEngine;

/// <summary>
/// Enemy-specific agent
/// </summary>
public class EnemyAgent : Agent
{
    private EnemyFSM _stateMachine;

    [field: SerializeField] public EnemyStateMachineConfig AgentConfig { get; private set; }

    public StateContext Context { get; private set; }   

    public override void InitializeAgent()
    {
        if (container == null)
        {
            Debug.LogError($"{name} EnemyAgent missing DIContainer!");
            return;
        }

        // Setup context
        Context = new StateContext(this);

        // Subscriptions
        EventBus.Instance.Subscribe<TargetDetectedEvent>(
                evt => _ = evt.SourceAgent == this,
                HandleTargetDetected
            );

        CreateFSM();
    }

    private void CreateFSM()
    {
        _stateMachine = gameObject.AddComponent<EnemyFSM>();
        _stateMachine.Initialize(this);
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

    private void HandleTargetDetected(TargetDetectedEvent evt)
    {
        _stateMachine.HandleTargetDetected(evt);
    }

    // FOR DEBUGGING
    public void Idle() => _stateMachine.ChangeState(EnemyStateID.Idle, Context);
    public void Patrol() => _stateMachine.ChangeState(EnemyStateID.Patrol, Context);
    public void Chase() => _stateMachine.ChangeState(EnemyStateID.Chase, Context);
    public void Attack() => _stateMachine.ChangeState(EnemyStateID.Attack, Context);
}
