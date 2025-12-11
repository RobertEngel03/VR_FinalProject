using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerFSMConfig", menuName = "Scriptables/FSM/PlayerConfig")]
public class PlayerStateMachineConfig : ScriptableObject
{
    public PlayerStateID defaultState;
    public List<PlayerStateMapping> stateMappings;

    [Serializable]
    public struct PlayerStateMapping
    {
        public PlayerStateID stateID;
        public PlayerStateSO stateSO;
    }
}