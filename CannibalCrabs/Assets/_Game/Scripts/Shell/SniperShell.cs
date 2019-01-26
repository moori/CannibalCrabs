using UnityEngine;

public class SniperShell : Shell
{
    public Spike spikePrefab;

    public override void Shoot(Vector2 direction)
    {
        if (canShoot)
        {
            Spike spike = Instantiate(spikePrefab);
            spike.Go(owner, direction);
            timeLastShot = Time.time;
        }
    }
}
