using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;
using UnityEngine.UI;



public class BoardManager : MonoBehaviour
{
    // the gameboard & its paramaters

    // prefabs
    public GameObject boardHolder;
    public GameObject playerPositionIndicator;
    public GameObject player;
    public GameObject[] roomSet;
    public GameObject[] roomTileSet;
    public GameObject[] mapSet;
    public GameObject[] teleporters;

    // parameters
    public static int roomSpacingX;
    public static int roomSpacingY;
    public int roomCountMaximum;
    public int roomCountMinimum;
    public int totalNumberRooms;

    // lists used in board creation
    public HashSet<Vector3> inputRoomPositions = new HashSet<Vector3>();
    public HashSet<Vector3> returnRoomPositions = new HashSet<Vector3>();
    public HashSet<Vector3> defaultTypeRoomPositions = new HashSet<Vector3>();
    public HashSet<Vector3> mirrorAccessTypeRoomPositions = new HashSet<Vector3>();
    public HashSet<Vector3> roomPositions = new HashSet<Vector3>();

    public List<Vector3> playerSpawnPoints = new List<Vector3>();
   

    // start
    private void Start()
    {
        boardHolder = Instantiate(boardHolder);
        playerPositionIndicator = Instantiate(playerPositionIndicator);
        player = Instantiate(player);
        MovementManager.cameraTarget = player;
    }

    // creates and places board
    public void BoardController(int lmid)                                                               
    {
        Debug.Log("BoardController: lmid = " + lmid);

        // set number of rooms for this floor, clear lists before instantiating new floor
        totalNumberRooms = Random.Range(roomCountMinimum, roomCountMaximum);
        inputRoomPositions.Clear();
        returnRoomPositions.Clear();
        roomPositions.Clear();
        defaultTypeRoomPositions.Clear();
        mirrorAccessTypeRoomPositions.Clear();
        playerSpawnPoints.Clear();

        // add origin room position
        inputRoomPositions.Add(new Vector3(0, 0, 0f));
        roomPositions.Add(new Vector3(0, 0, 0f));

        // while we don't have as many rooms as we need to continue, 
        //add new rooms to roompositions and input new rooms into 
        //method to create new rooms
         
        while (roomPositions.Count < totalNumberRooms)                                                  
        {                                                                                              
            returnRoomPositions = BoardMaker(inputRoomPositions);
            inputRoomPositions.Clear();

            foreach (Vector3 position in returnRoomPositions)
            {
                    inputRoomPositions.Add(position);
                if (roomPositions.Count < totalNumberRooms)
                {
                    roomPositions.Add(position);
                }     
            }
        }

        foreach (Vector3 position in roomPositions)                 // sets room type using unity random
        {
            int di = Random.Range(0, 3);

            if (di < 2)
            {
                defaultTypeRoomPositions.Add(position);
            }
            if (di == 2)
            {
                mirrorAccessTypeRoomPositions.Add(position);
            }
        }

        foreach (Vector3 position in defaultTypeRoomPositions)
        {
            Debug.Log(position);
        }
        foreach (Vector3 position in mirrorAccessTypeRoomPositions)
        {
            Debug.Log(position);
        }
        // place rooms based on position
        foreach (Vector3 position in defaultTypeRoomPositions)          // default type room instantiation                                           // instantiates random room prefab at all of our room locations, places the map in the topright corner
        {
            // set floor
            GameObject floor = Instantiate(roomSet[Random.Range(0, roomSet.Length)],
            new Vector3(position.x * roomSpacingX, position.y * roomSpacingY, 0f), Quaternion.identity) as GameObject;
            // set map
            GameObject map = Instantiate(mapSet[0], new Vector3(Camera.main.transform.position.x + (position.x / 2) + 9,
                Camera.main.transform.position.y + ((position.y / 2) + 4)), Quaternion.identity) as GameObject;
            // set player spawn point options
            playerSpawnPoints.Add(new Vector3(position.x, position.y));                                                         // adds all positions which are default rooms to possible player spawn points                
                                                                                                                                // set teleporters
            List<Vector3> surroundingRooms = CheckForAdjacentRooms(position, defaultTypeRoomPositions);
            SetTeleportersToAdjacentRooms(position, surroundingRooms);

            map.transform.SetParent(Camera.main.transform);
            floor.transform.SetParent(boardHolder.transform);
        }
        foreach (Vector3 position in mirrorAccessTypeRoomPositions)     // mirror access type room instantiation
        {
            GameObject floor = Instantiate(roomSet[Random.Range(0, roomSet.Length)],
               new Vector3(position.x * roomSpacingX, position.y * roomSpacingY, 0f), Quaternion.identity) as GameObject;
            GameObject map = Instantiate(mapSet[1], new Vector3(Camera.main.transform.position.x + (position.x / 2) + 9,
                Camera.main.transform.position.y + ((position.y / 2) + 4)), Quaternion.identity) as GameObject;

            map.transform.SetParent(Camera.main.transform);
            floor.transform.SetParent(boardHolder.transform);
        }

        // set the player position and player indicator position, focus camera on player
        Vector3 playerSpawnPoint = playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)];                 // instantiates the player at one of the possible spawn points, sets player indicator, sets camera target
        GameManager.boardScript.playerPositionIndicator.transform.position = new Vector3(playerSpawnPoint.x * roomSpacingX + 13, playerSpawnPoint.y * roomSpacingY + 7);
        GameManager.boardScript.player.transform.position = new Vector3(playerSpawnPoint.x * roomSpacingX + 13, playerSpawnPoint.y * roomSpacingY + 7); 
        MovementManager.cameraTarget = player;
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
    }

    // adds room positions based on rng and preset layouts 
    // eg. if(4) {place tiles in square around current input position}
    public HashSet<Vector3> BoardMaker(HashSet<Vector3> inputRoomPositions)
    {
        HashSet<Vector3> newRoomPositions = new HashSet<Vector3>();

        foreach (Vector3 room in inputRoomPositions)
        {
            int di = Random.Range(0, 6);           

            if (di == 0) // up
            {
                Vector3 a = new Vector3(room.x, room.y + 1, room.z);
                newRoomPositions.Add(a);
            }
            if (di == 1) // down
            {
                Vector3 b = new Vector3(room.x, room.y - 1, room.z);
                newRoomPositions.Add(b);               
            }
            if (di == 2) // left
            {
                
                Vector3[] c = new Vector3[2];
                c[0] = new Vector3(room.x - 1, room.y, room.z);
                c[1] = new Vector3(room.x - 2, room.y, room.z);       
                newRoomPositions.Add(c[0]);
                newRoomPositions.Add(c[1]);
                
            }
            if (di == 3) // right
            {
                Vector3[] d = new Vector3[2];
                d[0] = new Vector3(room.x + 1, room.y, room.z);
                d[1] = new Vector3(room.x + 2, room.y, room.z);
                newRoomPositions.Add(d[0]);
                newRoomPositions.Add(d[1]);
            }
            if (di == 4)
            {
                Vector3[] e = new Vector3[4];
                e[0] = new Vector3(room.x, room.y + 1, room.z);
                e[1] = new Vector3(room.x, room.y - 1, room.z);
                e[2] = new Vector3(room.x - 1, room.y, room.z);
                e[3] = new Vector3(room.x + 1, room.y, room.z);
                newRoomPositions.Add(e[0]);
                newRoomPositions.Add(e[1]);
                newRoomPositions.Add(e[2]);
                newRoomPositions.Add(e[3]);
            }
            if (di == 5)
            {
                Vector3[] f = new Vector3[7];
                f[0] = new Vector3(room.x - 1, room.y, room.z);
                f[1] = new Vector3(room.x + 1, room.y, room.z);
                f[2] = new Vector3(room.x - 1, room.y + 1, room.z);
                f[3] = new Vector3(room.x + 1, room.y + 1, room.z);
                f[4] = new Vector3(room.x - 1, room.y + 2, room.z);
                f[5] = new Vector3(room.x + 1, room.y + 2, room.z);
                f[6] = new Vector3(room.x, room.y + 2, room.z);
                newRoomPositions.Add(f[0]);
                newRoomPositions.Add(f[1]);
                newRoomPositions.Add(f[2]);
                newRoomPositions.Add(f[3]);
                newRoomPositions.Add(f[4]);
                newRoomPositions.Add(f[5]);
                newRoomPositions.Add(f[6]);
            }
        }

        return newRoomPositions;
    }

    // checks for surrounding rooms in cardinal directions, returns a list of all rooms
    // surrounding current room
    public List<Vector3> CheckForAdjacentRooms(Vector3 currentRoom, HashSet<Vector3> allRooms)
    {
        List<Vector3> adjacentRooms = new List<Vector3>();

        Vector3 roomUp = new Vector3(currentRoom.x, currentRoom.y + 1);
        Vector3 roomDown = new Vector3(currentRoom.x, currentRoom.y - 1);
        Vector3 roomRight = new Vector3(currentRoom.x + 1, currentRoom.y);
        Vector3 roomLeft = new Vector3(currentRoom.x - 1, currentRoom.y);

        if (allRooms.Contains(roomUp))
        {
            adjacentRooms.Add(roomUp);
        }
        if (allRooms.Contains(roomDown))
        {
            adjacentRooms.Add(roomDown);
        }
        if (allRooms.Contains(roomRight))
        {
            adjacentRooms.Add(roomRight);
        }
        if (allRooms.Contains(roomLeft))
        {
            adjacentRooms.Add(roomLeft);
        }

        return adjacentRooms;
    }
    // sets teleporters between adjacent rooms
    public void SetTeleportersToAdjacentRooms(Vector3 currentRoom, List<Vector3> adjacentRooms)
    {

        GameObject teleporter;

        foreach (Vector3 adjacentRoom in adjacentRooms)
        {
            if (adjacentRoom.x == currentRoom.x)
            {                                       // the adjacent room is up or down
                if (adjacentRoom.y > currentRoom.y) // the adjacent room is up
                {
                    teleporter = Instantiate(teleporters[0],
                new Vector3(currentRoom.x * roomSpacingX +14, currentRoom.y * roomSpacingY + 13, 0f), Quaternion.identity) as GameObject;
                    teleporter.transform.SetParent(boardHolder.transform);
                }
                else                                // the adjacent room is down
                {
                    teleporter = Instantiate(teleporters[1],
                new Vector3(currentRoom.x * roomSpacingX + 14, currentRoom.y * roomSpacingY, 0f), Quaternion.identity) as GameObject;
                    teleporter.transform.SetParent(boardHolder.transform);
                }
            }
            if (adjacentRoom.y == currentRoom.y)    // the adjacent room is left or right
            {
                if (adjacentRoom.x < currentRoom.x) // the adjacent room is left
                {
                    teleporter = Instantiate(teleporters[2],
                new Vector3(currentRoom.x * roomSpacingX, currentRoom.y * roomSpacingY + 7, 0f), Quaternion.identity) as GameObject;
                    teleporter.transform.SetParent(boardHolder.transform);
                }
                else                                // the adjacent room is right
                {
                    teleporter = Instantiate(teleporters[3],
                new Vector3(currentRoom.x * roomSpacingX + 26, currentRoom.y * roomSpacingY + 7, 0f), Quaternion.identity) as GameObject;
                    teleporter.transform.SetParent(boardHolder.transform);
                }
            }
        }
    }
}

// rooms class
public class Room : MonoBehaviour
{
    public string RoomNumber { get; set; }
    public int[] Width { get; set; }
    public int[] Height { get; set; }
    public int NumberBreaks { get; set; }
    public int[] XOffsets { get; set; }
    public int[] TileBeforeBreak { get; set; }
    public GameObject[] TileSet { get; set; }
    public Transform Parent { get; set; }

    public Room(string roomNumber, int[] width, int[] height, int numberBreaks, int[] xOffsets, int[] tileBeforeBreak, GameObject[] tileSet, Transform parent)
    {
        RoomNumber = roomNumber;
        Width = width;
        Height = height;
        NumberBreaks = numberBreaks;
        XOffsets = xOffsets;
        TileBeforeBreak = tileBeforeBreak;
        TileSet = tileSet;
        Parent = Parent;

    }

    public void RoomMaker(Room room)
    {
        for (int i = 0; i < room.NumberBreaks; i++)
        {
            for (int x = 0; x < room.Width[i]; x++)
            {
                for (int y = room.Height[i]; y < room.Height[i + 1]; y++)
                {
                    GameObject toInstantiate = room.TileSet[i];
                    GameObject instance =
                    Instantiate(toInstantiate, new Vector3(x + room.XOffsets[i], y, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(GameManager.boardScript.boardHolder.transform);
                    GameManager.boardScript.boardHolder.name = room.RoomNumber;
                }
            }
        }
    }  
}
