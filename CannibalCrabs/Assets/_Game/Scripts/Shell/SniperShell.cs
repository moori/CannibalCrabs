using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperShell : Shell
{
    public Spike spikePrefab;

    public override void Shoot(Vector2 direction)
    {
        Spike spike = Instantiate(spikePrefab);
        spike.Go(gameObject.transform.position, direction);
    }
}
