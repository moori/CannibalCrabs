using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string playerString ="";

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        var h = Input.GetAxisRaw(playerString + "Horizontal");
        var v = Input.GetAxisRaw(playerString + "Vertical");

        player.Move(h, v);
    }
}
