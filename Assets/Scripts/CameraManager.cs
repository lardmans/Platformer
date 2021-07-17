using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraManager : MonoBehaviour
{
    public float ZIndex = -10;

    new Camera camera;
    Player player;

    int currentRoomID;
    
    private void Awake()
    {
        camera = GetComponent<Camera>();
        camera.rect = new Rect((9f/16f)/2f, 0f, 9f/16f, 1f );
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GameEvents.Singleton.onEnterRoom += PanToRoom;
    }

    private void PanToRoom(int newRoomID)
    {
        currentRoomID = newRoomID;
        Vector3 center = RoomManager.Singleton.rooms[currentRoomID].center;
        transform.position = new Vector3(center.x, center.y, ZIndex);
    }
}
