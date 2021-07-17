using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RenewableManager : MonoBehaviour
{
    public static RenewableManager Singleton;

    Dictionary<int, List<RoomBehavior>> renewables;

    private void Awake()
    {
        Singleton = this;

        renewables = new Dictionary<int, List<RoomBehavior>>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Singleton.onExitRoom += RenewBehaviorsInRoom;
        GameEvents.Singleton.onRespawn += PlayerRespawned;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (var element in renewables)
            {
                Debug.Log(element.Key + ": " + element.Value);
            }
        }
    }

    public void Register(int roomID, RoomBehavior roomBehavior)
    {
        if (renewables.ContainsKey(roomID))
        {
            renewables[roomID].Add(roomBehavior);
        }
        else
        {
            List<RoomBehavior> list = new List<RoomBehavior>();
            list.Add(roomBehavior);
            renewables.Add(roomID, list);
        }
    }

    public void PlayerRespawned()
    {
        RenewBehaviorsInRoom(RoomManager.Singleton.currentRoom.id);
    }

    public void RenewBehaviorsInRoom(int roomID)
    {
        if (renewables.ContainsKey(roomID))
        {
            foreach (RoomBehavior roomBehavior in renewables[roomID])
            {
                if (roomBehavior.renewable)
                {
                    roomBehavior.Renew();
                }
            }
        }
    }


}
