using Rewired;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public int playerIndex;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        float h = ReInput.players.GetPlayer(playerIndex).GetAxis("Move Horizontal");
        float v = ReInput.players.GetPlayer(playerIndex).GetAxis("Move Vertical");

        float aim_h = ReInput.players.GetPlayer(playerIndex).GetAxis("Aim Horizontal");
        float aim_v = ReInput.players.GetPlayer(playerIndex).GetAxis("Aim Vertical");

        player.Move(h, v);
        player.Aim(h, v, aim_h, aim_v);
    }

    private void Update()
    {
        bool shoot = ReInput.players.GetPlayer(playerIndex).GetButton("Shoot");
        bool sacrifice = ReInput.players.GetPlayer(playerIndex).GetButtonDown("Sacrifice");

        if (shoot)
            player.Shoot();

        if (sacrifice)
            player.Sacrifice();
    }
}
