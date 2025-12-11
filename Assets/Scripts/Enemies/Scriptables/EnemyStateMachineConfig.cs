using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyFSMConfig", menuName = "Scriptables/FSM/EnemyConfig")]
public class EnemyStateMachineConfig : ScriptableObject
{
    public EnemyStateID defaultState;
    public List<EnemyStateMapping> stateMappings;

    [Serializable]
    public struct EnemyStateMapping
    {
        public EnemyStateID stateID;
        public EnemyStateSO stateSO;
    }
}
