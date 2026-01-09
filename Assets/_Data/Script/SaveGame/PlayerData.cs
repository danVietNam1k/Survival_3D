using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
[System.Serializable]
public class PlayerData 
{
    public float[] playerStates;// [0] = health, [1] = calories, [2] = hydration
    public float[] playerPositionAndRotation;
    public string[] inventoryContent;
    public string[] inQuickSlotContent;
    public PlayerData(float[] _playerStates, float[] _playerPositionAndRotation, string[] _inventoryContent, string[] _inQuickSlotContent)
    {
        this.playerStates = _playerStates;
        this.playerPositionAndRotation = _playerPositionAndRotation;
        this.inventoryContent = _inventoryContent;
        this.inQuickSlotContent = _inQuickSlotContent;
    }
}
