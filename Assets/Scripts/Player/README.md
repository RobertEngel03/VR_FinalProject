# Player FSM (Optional)

This directory contains the **Player Finite State Machine (FSM)** system.

## Overview
The Player FSM was originally implemented to handle discrete player states such as `Idle`, `Walk`, `CastSpell`, or `Grab`. It follows a ScriptableObject-driven design:

- **PlayerStateSO** → ScriptableObject describing a state  
- **PlayerRuntimeState** → Runtime logic for a state  
- **PlayerFSM** → Handles state transitions and calls `Enter`, `Tick`, `Exit`  

This system was designed with flexibility and type safety in mind, and can be extended with additional states if needed.

## Usage in VR
In VR, player behavior is **mostly continuous** and event-driven. Most player actions (hand interactions, locomotion, gestures) do **not naturally fit a discrete state machine**, and using a full FSM for all player actions is generally unnecessary.

That said, the FSM can still be useful for:

- **High-level player modes**, e.g., Interaction Mode, Combat Mode, UI Mode  
- **Special interactions** that require discrete sequences (charging a spell, multi-step gestures, cutscenes)  
- **Testing or prototyping** discrete behavior for player states  

## Notes
- The Player FSM is **optional**. Most VR systems will function better using component-based and event-driven logic.  
- If you decide to use it, the FSM is ready to plug in and extend using ScriptableObjects and runtime states.  
- The code also serves as an example of a strongly-typed, ScriptableObject-driven FSM for y'all to learn from.
