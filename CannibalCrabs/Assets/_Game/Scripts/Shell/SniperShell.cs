using UnityEngine;

public class SniperShell : Shell
{
    public Rock rockPrefab;

    public override void Shoot(Vector2 direction)
    {
        if (canShoot)
        {
            Rock rock = Instantiate(rockPrefab);
            rock.Go(owner, direction);
            timeLastShot = Time.time;
        }
    }
}
