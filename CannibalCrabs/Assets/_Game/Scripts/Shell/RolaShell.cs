using UnityEngine;

public class RolaShell : Shell
{
    //public Spike spikePrefab;
    public RolaThrow rolaPrefab;

    public override void Shoot(Vector2 direction)
    {
        if (!canShoot)
            return;

        //Spike spike = Instantiate(spikePrefab);
        //spike.Go(owner, (direction + Random.insideUnitCircle.normalized * 0.3f).normalized);
        //timeLastShot = Time.time;
        //shootEventEmitter.start();
    }

    public override void Sacrifice(Vector2 direction)
    {
        // base.Sacrifice(direction);
        RolaThrow rola = Instantiate(rolaPrefab);
        rola.Activate(owner, transform.position, direction);
    }
}
