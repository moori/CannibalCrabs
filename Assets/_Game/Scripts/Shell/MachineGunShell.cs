using UnityEngine;
using FMODUnity;

public class MachineGunShell : Shell
{
    public Spike spikePrefab;
    public Dash dashPrefab;

    public override void Shoot(Vector2 direction)
    {
        if (!canShoot)
            return;

        Spike spike = Instantiate(spikePrefab);
        spike.Go(owner, (direction + Random.insideUnitCircle.normalized * 0.3f).normalized);
        timeLastShot = Time.time;
        shootEventEmitter.start();
    }

    public override void Sacrifice(Vector2 direction)
    {
        base.Sacrifice(direction);
        Dash dash = Instantiate(dashPrefab);
        dash.Activate(owner, transform.position, direction);
    }
}
