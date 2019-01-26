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
        float h = Input.GetAxisRaw(playerString + "Horizontal");
        float v = Input.GetAxisRaw(playerString + "Vertical");

        float aim_h = Input.GetAxisRaw(playerString + "RightHorizontal");
        float aim_v = Input.GetAxisRaw(playerString + "RightVertical");

        player.Move(h, v);
        player.Aim(h, v, aim_h, aim_v);
    }

    private void Update()
    {
        bool shoot = Input.GetAxis(playerString + "Shoot") > 0f || Input.GetButtonDown(playerString + "Shoot");
        bool sacrifice = Input.GetButtonDown(playerString + "Sacrifice");
        
        if (shoot)
            player.Shoot();

        if (sacrifice)
            player.Sacrifice();
    }
}
