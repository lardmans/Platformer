using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    public bool renewable;
    public int roomID;
    public Vector3 defaultPosition;

    public void Awake()
    {
        defaultPosition = transform.position;
    }

    // Start is called before the first frame update
    public void Start()
    {
        SetRoomID();
        RenewableManager.Singleton.Register(roomID, this);
    }

    void SetRoomID()
    {
        roomID = RoomManager.Singleton.GetRoomIDFromPosition(transform.position);
    }

    public virtual void Renew()
    {

    }

    public virtual void SetDefaultValues()
    {

    }

}
