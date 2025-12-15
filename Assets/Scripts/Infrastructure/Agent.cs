using UnityEngine;
using System;

public class DamageContext
{
    public Agent Source;
    public float Damage;
    public float KnockbackForce;

    public DamageContext(Agent source, float damage, float knockbackForce)
    {
        Source = source;
        Damage = damage;
        KnockbackForce = knockbackForce;
    }
}

public interface IDamageable
{
    public float Health { get; set; }

    public void DealDamage(DamageContext context);
}

/// <summary>
/// Base class for all agents (players, enemies, NPCs)
/// </summary>
[RequireComponent(typeof(DIContainer))]
public abstract class Agent : MonoBehaviour, IDamageable
{
    [Header("Agent Identity")]
    public string EntityName;
    public Guid EntityId { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }

    // Really we would move this onto an AttributesProcessor class where we do Dictionary<Attribute, value>
    [SerializeField] private float _health = 100f;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0)
            {
                EventBus.Instance.Publish(new DeathEvent(this));
            }
        }
    }

    protected DIContainer container;

    protected virtual void Awake()
    {
        EntityId = Guid.NewGuid(); // unique identifier for this agent

        // Grab the local DIContainer
        if (!TryGetComponent(out container))
        {
            Debug.LogError($"{name} Agent requires a DIContainer component!");
        }

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();

        InitializeAgent();
    }

    /// <summary>
    /// Initialize the agent (dependencies, components, FSMs, etc.)
    /// </summary>
    public abstract void InitializeAgent();

    public virtual void Update() { }
    public virtual void FixedUpdate() { }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        
    }

    protected virtual void OnCollisionExit(Collision collision)
    {

    }

    /// <summary>
    /// Resolve a component from this agent's DIContainer
    /// </summary>
    public T Get<T>() where T : Component
    {
        if (container == null)
        {
            Debug.LogError($"{name} has no DIContainer to resolve {typeof(T)}");
            return null;
        }

        return container.Get<T>();
    }

    public void DealDamage(DamageContext context)
    {
        Debug.Log($"{context.Source?.EntityName} hit {EntityName} for {context.Damage} damage");
        Health -= context.Damage;
    }
}
