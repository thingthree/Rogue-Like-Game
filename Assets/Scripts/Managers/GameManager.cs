using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    // declares variables needed to ensure this is the only instance of the game running 
    // and calls the correct function to start the game, right now it either takes you to
    // the main menu or lets you edit rooms if editingrooms is set to true

    public static GameManager Instance = null;

    public static BoardManager boardScript;
    public static TextManager textScript;
    public static PlayerUnitManager playerUnitScript;
    public static LogicManager logicScript;
    public static UnitStateMono unitStateMonoScript;
    public static MovementManager movementScript;

    public static bool editingRooms = false;
    public static bool inMenu = false;
    public static bool pause = true;


    void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardManager>();
        textScript = GetComponent<TextManager>();
        playerUnitScript = GetComponent<PlayerUnitManager>();
        logicScript = GetComponent<LogicManager>();
        unitStateMonoScript = GetComponent<UnitStateMono>();
        movementScript = GetComponent<MovementManager>();

        GameState currentGameState = new GameState(Player1Controller.playerStateSerial);

        InitGame();
    }

    void InitGame()
    {
        if (editingRooms == true)
        {
            string number = "1";
            int[] width = new int[1] { 27 };
            int[] height = new int[2] { 0, 14 }; // possition [0] is bottom & final position is top
            int numberBreaks = 1; // should be no less than 1, the top of the room
            int[] xOffsets = new int[3] { 0, 0, 0 }; 
            int[] tileBeforeBreak = new int[3] { 0, 0, 0 }; // 14 means 15th tile will start new section

            Room room = new Room(number, width, height, numberBreaks, xOffsets, tileBeforeBreak, GameManager.boardScript.roomTileSet, GameManager.boardScript.boardHolder.transform);
            room.RoomMaker(room);
            Debug.Log(1);
        }
        else
        {
            logicScript.LogicController(0);
        }
    }

    private void Update()
    {
        GameManager.movementScript.SmoothFollow(Camera.main.transform, MovementManager.cameraTarget.transform);
        
        if (Input.GetKeyDown("t"))
        {
            GameObject toTeleportTo = new GameObject();
            toTeleportTo.transform.position = new Vector3(0, 0, 0f);
            GameManager.movementScript.Teleport(GameManager.boardScript.player, toTeleportTo.transform);
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}

   