using UnityEngine;
using System;

/// <summary>
/// Base class for all agents (players, enemies, NPCs)
/// </summary>
[RequireComponent(typeof(DIContainer))]
public abstract class Agent : MonoBehaviour
{
    [Header("Agent Identity")]
    public string EntityName;
    public Guid EntityId { get; private set; }

    public Rigidbody Rigidbody { get; private set; }

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
}
