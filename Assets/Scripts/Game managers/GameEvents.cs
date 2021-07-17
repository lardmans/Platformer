using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{

    public static GameEvents Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public event Action<int> onEnterRoom;
    public void EnterRoom(int roomID)
    {
        onEnterRoom?.Invoke(roomID);
    }

    public event Action<int> onExitRoom;
    public void ExitRoom(int roomID)
    {
        onExitRoom?.Invoke(roomID);
    }

    public event Action onDie;
    public void Die()
    {
        onDie?.Invoke();
    }

    public event Action onRespawn;
    public void Respawn()
    {
        onRespawn?.Invoke();
    }


}
