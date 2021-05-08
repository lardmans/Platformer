using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    public KeyCode jumpKey;
    Player player;

    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        player.SetDirectionalInput(directionalInput);

        if (Input.GetKeyDown(jumpKey))
        {
            player.OnJumpInputDown();
        }

        if (Input.GetKeyUp(jumpKey))
        {
            player.OnJumpInputUp();
        }

    }
}
