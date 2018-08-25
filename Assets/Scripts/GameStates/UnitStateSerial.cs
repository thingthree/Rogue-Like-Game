using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
// This is PlayerStateMono's memento class. It is needed for saving the game.
// See UnitStateMono for further description.
public class UnitStateSerial
{
    public Transform Transform { get; set; }
    public int Facing { get; set; }
    public bool Active { get; set; }

    public UnitStateSerial(Transform transform, int facing, bool active)
    {
        Transform = transform;
        Facing = facing;
        Active = active;
    }
}
