using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

// data needed to save the game are kept in the GameState class

[Serializable]
public class GameState 
{
    public static GameState Current;

    public UnitStateSerial Player1Serial { get; set; }
    public string Location { get; set; }
    public string DateTime { get; set; }

    public GameState(UnitStateSerial player1Serial)
    {
        Player1Serial = Player1Controller.playerStateSerial;
        string location = Location;
        string dateTime = DateTime;
    }
}
