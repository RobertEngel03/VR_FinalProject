using System;
using System.Collections.Generic;
using UnityEngine;

public interface IService
{
    public void Register();
}

/// <summary>
/// A simple global service locator for registering and retrieving services by type.
/// 
/// This static class allows you to:
/// 1. Register services globally using <see cref="Register{T}(T)"/>.
/// 2. Retrieve registered services using <see cref="Get{T}"/>.
/// 3. Clear all registered services using <see cref="Clear"/>.
///
/// Use this for globally shared systems like AudioManager, InputManager, or AnalyticsManager.
/// Avoid overusing for per-object or modular systems; for those, consider using the per-object DIContainer.
/// </summary>
public static class ServiceLocator
{
    public static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    /// <summary>
    /// Registers a global service instance of type T.
    /// If a service of this type already exists, it will be overwritten.
    /// </summary>
    public static void Register<T>(T service) where T : class
    {
        _services[typeof(T)] = service;
        Debug.Log($"Registered {service.GetType()}");
    }

    /// <summary>
    /// Retrieves a previously registered service of type T.
    /// Returns null if no service of that type has been registered.
    /// </summary>
    public static T Get<T>() where T : class
    {
        return _services.TryGetValue(typeof(T), out var service) ? service as T : null;
    }

    /// <summary>
    /// Clears all registered services from the service locator.
    /// Useful for scene transitions or resetting global state.
    /// </summary>
    public static void Clear()
    {
        Debug.Log($"Services Cleared");
        _services.Clear();
    }
}
