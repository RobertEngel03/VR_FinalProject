using UnityEngine;

public class PlayerAgent : Agent
{
    private PlayerFSM _stateMachine;

    [field: SerializeField] public PlayerStateMachineConfig AgentConfig { get; private set; }

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

        CreateFSM();
    }

    private void CreateFSM()
    {
        _stateMachine = gameObject.AddComponent<PlayerFSM>();
        _stateMachine.Initialize(this);
    }
}