using UnityEngine;

public class SniperShell : Shell
{
    public Spike spikePrefab;

    public override void Shoot(Vector2 direction)
    {
        Spike spike = Instantiate(spikePrefab);
        spike.Go(owner, direction);
    }
}
