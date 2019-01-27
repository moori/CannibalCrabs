using UnityEngine;

public class SniperShell : Shell
{
    public Rock rockPrefab;
    public NinjaBomb ninjaBombPrefab;

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
        NinjaBomb ninjaBomb = Instantiate(ninjaBombPrefab);
        ninjaBomb.Activate(owner, transform.position);
    }
}
