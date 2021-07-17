using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomManager : MonoBehaviour
{
    [Header("Settings")]
    public Vector2Int worldSize;
    public Vector2Int roomSize;
    public bool generateRoomObjects;
    public GameObject roomHolderHolder;
    public GameObject roomHolderPrefab;

    public static RoomManager Singleton;
    public Room[] rooms;
    public Room currentRoom;


    Player player;

    private void Awake()
    {
        Singleton = this;
        InitRooms();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentRoom();
    }

    void UpdateCurrentRoom()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].bounds.Contains(player.transform.position))
            {
                if (rooms[i].id != currentRoom.id)
                {
                    // aha this is a new room
                    GameEvents.Singleton.ExitRoom(currentRoom.id);
                    currentRoom = rooms[i];
                    GameEvents.Singleton.EnterRoom(currentRoom.id);
                    break;
                }
            }
        }
    }

    private void OnValidate()
    {
        if (generateRoomObjects)
        {
            generateRoomObjects = false;

            if (roomHolderHolder == null || roomHolderPrefab == null)
            {
                Debug.Log("References not set.");
                return;
            }

            InitRooms();

            foreach (Room room in rooms)
            {
                GameObject holder = GameObject.Find("Room " + room.id);

                if (holder == null)
                {
                    holder = Instantiate(roomHolderPrefab, room.center, Quaternion.identity, roomHolderHolder.transform);
                    holder.name = "Room " + room.id;
                }
                else
                {
                    holder.transform.position = room.center;
                }
            }
        }
    }

    void InitRooms()
    {
        rooms = new Room[(int)worldSize.x * (int)worldSize.y];
        int id = 0;
        for (int y = 0; y < worldSize.y; y++)
        {
            for (int x = 0; x < worldSize.x; x++)
            {
                Room room = new Room();

                room.bottomLeft = new Vector3(x * roomSize.x, y * roomSize.y, 0);
                room.topLeft = new Vector3(x * roomSize.x, (y + 1) * roomSize.y, 0);
                room.bottomRight = new Vector3((x + 1) * roomSize.x, y * roomSize.y, 0);
                room.topRight = new Vector3((x + 1) * roomSize.x, (y + 1) * roomSize.y, 0);

                room.center = new Vector3(room.topRight.x - ((float)roomSize.x/2), room.topLeft.y - ((float)roomSize.y/2), 0);

                room.bounds = new Bounds(room.center, new Vector3(roomSize.x, roomSize.y));

                room.id = id;
                rooms[id] = room;
                id++;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (rooms == null)
        {
            InitRooms();
        }
        else
        {
            foreach (Room room in rooms)
            {
                Debug.DrawLine(room.bottomLeft, room.topLeft, Color.red);
                Debug.DrawLine(room.topLeft, room.topRight, Color.red);
                Debug.DrawLine(room.topRight, room.bottomRight, Color.red);
                Debug.DrawLine(room.bottomRight, room.bottomLeft, Color.red);
                Handles.Label(room.center + Vector3.up * 8, "Room " + room.id);
            }
        }
    }

    public bool PositionIsInRoom(Vector3 pos, int roomID)
    {
        Room room = rooms[roomID];

        if (pos.x > room.topLeft.x &&
            pos.x < room.topRight.x &&
            pos.y > room.bottomLeft.y &&
            pos.y < room.topLeft.y)
        {
            return true;
        }
        else return false;
    }

    public int GetRoomIDFromPosition(Vector3 pos)
    {
        int a = (int) Mathf.Floor(pos.x / roomSize.x);
        int b = (int) Mathf.Floor(pos.y / roomSize.y);

        return b * worldSize.x + a;
    }

    public bool RoomExists(int roomID)
    {
        return (roomID >= 0 && roomID < worldSize.x * worldSize.y);
    }

}

public struct Room
{
    public int id;
    public Bounds bounds;

    public Vector3 bottomLeft, topLeft, bottomRight, topRight, center;
}