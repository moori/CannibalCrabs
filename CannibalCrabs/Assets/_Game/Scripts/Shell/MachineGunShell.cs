using UnityEngine;

public class MachineGunShell : Shell
{
    public Spike spikePrefab;

    public override void Shoot(Vector2 direction)
    {
        if (canShoot)
        {
            Spike spike = Instantiate(spikePrefab);
            spike.Go(owner, (direction + Random.insideUnitCircle.normalized * 0.3f).normalized);
            timeLastShot = Time.time;
        }
    }
}
