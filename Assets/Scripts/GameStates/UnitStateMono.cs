using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

// Monobehavior classes are not serializable, so to pull from serialized
// data we need to use a memento class. I do this by including a memento varialbe,
// Memento, of the PlayerStateSerial class into my non-serializable monobehavior
// class then using a revert method to revert that PlayerStateSerial, setting
// the rest of the PlayerStateMono classes variables
public class UnitStateMono : MonoBehaviour
{
    public UnitStateSerial Memento { get; set; }

    public Transform Transform { get; set; }
    public int Facing { get; set; }
    public bool Active { get; set; }

    public UnitStateMono Revert(UnitStateMono usm)
    {
        Transform = Memento.Transform;
        Facing = Memento.Facing;
        Active = Memento.Active;
        return usm;
    }
}

