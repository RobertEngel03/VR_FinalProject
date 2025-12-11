# Enemy FSM

This directory contains the **Enemy Finite State Machine (FSM)** system. The EnemyFSM is designed to handle discrete enemy behaviors in a modular, ScriptableObject-driven manner.

---

## Overview

The Enemy FSM provides a framework for managing **enemy AI states** such as:

- Idle
- Patrol
- Chase
- Attack
- Dead

It follows a strongly-typed, ScriptableObject-based design:

- **EnemyStateSO** → ScriptableObject describing an enemy state
- **EnemyRuntimeState** → Runtime logic for a state
- **EnemyFSM** → Handles state transitions, calling `Enter`, `Tick`, and `Exit` on runtime states
- **EnemyStateMachineConfig** → Configures the list of states and the default state for the FSM

The FSM is ideal for enemies because their behavior is **discrete and sequential**, unlike the VR player whose actions are mostly continuous.

---

## Usage

### 1. Create State ScriptableObjects

For each enemy state, create a ScriptableObject that inherits from `EnemyStateSO`. Define the `RuntimeType` pointing to the corresponding `EnemyRuntimeState` class.

Example:

```csharp
[CreateAssetMenu(fileName = "EnemyIdleSO", menuName = "AI/FSM/Enemy/Idle")]
public class EnemyIdleStateSO : EnemyStateSO
{
    public override EnemyStateID StateID => EnemyStateID.Idle;
    public override Type RuntimeType => typeof(EnemyIdleState);
}

### 2. Create Config ScriptableObjects per Enemy 