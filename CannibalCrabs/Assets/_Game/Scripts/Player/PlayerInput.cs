using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string playerString;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        var h = Input.GetAxisRaw(playerString + "Horizontal");
        var v = Input.GetAxisRaw(playerString + "Vertical");

        var aim_h = Input.GetAxisRaw(playerString + "RightHorizontal");
        var aim_v = Input.GetAxisRaw(playerString + "RightVertical");

        var shoot = Input.GetAxis(playerString + "Shoot") > 0f || Input.GetButtonDown(playerString + "Shoot");

        player.Move(h, v);
        player.Aim(h, v, aim_h, aim_v);
        if (shoot)
        {
            player.Shoot();
        }
    }
}
