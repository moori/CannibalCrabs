using UnityEngine;

public class SniperShell : Shell
{
    public Rock rockPrefab;
    public Shield shieldPrefab;

    public override void Shoot(Vector2 direction)
    {
        if (!canShoot)
            return;

        Rock rock = Instantiate(rockPrefab);
        rock.Go(owner, direction);
        timeLastShot = Time.time;
        shootEventEmitter.start();
    }

    public override void Sacrifice(Vector2 direction)
    {
        base.Sacrifice(direction);
        Shield shield = Instantiate(shieldPrefab);
        shield.Activate(owner);
    }
}
