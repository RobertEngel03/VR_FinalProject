# DIContainer (Per-Object Dependency Injection)

This directory contains the **DIContainer**, a lightweight, per-object dependency injection system for modular agents in the project.  

---

## Overview

The DIContainer is designed to manage dependencies for **agents** (players, enemies, or other characters) without relying on global singletons. It allows components to request references to the systems they need, keeping them decoupled and modular.

Key features:

- **Per-object scope**: Each agent has its own DIContainer instance.
- **Automatic injection**: Components receive dependencies via `Inject()` or `Initialize()`.
- **Modular design**: Works with both FSM-driven and component-based systems.
- **Flexible**: Can resolve MonoBehaviours, ScriptableObjects, or plain C# classes.

---

## Usage

### 1. Adding DIContainer to an Agent

### 2. Register Dependencies

### 3. Inject Dependencies into Components
