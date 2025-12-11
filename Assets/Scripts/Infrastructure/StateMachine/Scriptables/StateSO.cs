using System;
using UnityEngine;

/// <summary>
/// Abstract base class for all State ScriptableObjects.
/// Contains only data needed to identify the state.
/// </summary>
public abstract class StateSO : ScriptableObject
{
    public abstract Type RuntimeType { get; }

    /// <summary>
    /// Name of the state (SO asset name)
    /// </summary>
    public string Name => this.name;

    [Header("Animator")]
    /// <summary>
    /// Optional override AnimationClip
    /// </summary>
    public AnimationClip OverrideAnimationClip;
}
