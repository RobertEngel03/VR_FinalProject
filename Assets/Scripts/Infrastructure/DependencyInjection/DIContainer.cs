using System;
using System.Collections.Generic;
using UnityEngine;

// GLOBAL STYLE DI
/*
public static class DIContainer
{
    private static readonly Dictionary<Type, Func<object>> _bindings = new();
    private static readonly Dictionary<Type, object> _singletons = new();

    #region BINDING
    // Binds an interface to a factory method
    public static void Bind<T>(Func<T> factory)
    {
        _bindings[typeof(T)] = () => factory();
    }

    // Binds a concrete instance as a singleton
    public static void BindInstance<T>(T instance)
    {
        _singletons[typeof(T)] = instance;
    }

    // Binds to constructor (automatic)
    public static void Bind<T>() where T : new()
    {
        _bindings[typeof(T)] = () => new T();
    }

    #endregion

    // Generic resolve
    public static T Resolve<T>()
    {
        var type = typeof(T);

        // Check instance
        if (_singletons.TryGetValue(type, out var instance))
            return (T)instance;

        // Check factory
        if (_bindings.TryGetValue(type, out var factory))
            return (T)factory();

        throw new Exception($"[DIContainer] No binding found for type {type}.");
    }

    // Try resolve for optional dependencies
    public static bool TryResolve<T>(out T value)
    {
        var type = typeof(T);

        if (_singletons.TryGetValue(type, out var instance))
        {
            value = (T)instance;
            return true;
        }

        if (_bindings.TryGetValue(type, out var factory))
        {
            value = (T)factory();
            return true;
        }

        value = default;
        return false;
    }

    // Clear everything (use on scene unload)
    public static void Clear()
    {
        _bindings.Clear();
        _singletons.Clear();
    }
}
*/

/// <summary>
/// GameObject/Local DI (For consolidating components, makes composition workflow easier)
/// </summary>
[DisallowMultipleComponent]
public class DIContainer : MonoBehaviour
{
    private Dictionary<Type, Component> _components = new();

    private void Awake()
    {
        // Index all MonoBehaviour components on this GameObject
        var comps = GetComponents<Component>();
        foreach (var c in comps)
        {
            // Skip self
            if (c == this) continue;

            var type = c.GetType();
            if (!_components.ContainsKey(type))
            {
                _components[type] = c;
            }
        }
    }

    // Generic getter
    public T Get<T>() where T : Component
    {
        if (_components.TryGetValue(typeof(T), out var comp))
            return (T)comp;

        throw new Exception($"DIContainer: Component of type {typeof(T)} not found on {gameObject.name}");
    }

    // Try-get version
    public bool TryGet<T>(out T comp) where T : Component
    {
        if (_components.TryGetValue(typeof(T), out var c))
        {
            comp = (T)c;
            return true;
        }

        comp = null;
        return false;
    }

    // Optional: Add a component at runtime and register it
    public void Add<T>(T comp) where T : Component
    {
        _components[typeof(T)] = comp;
    }
}
