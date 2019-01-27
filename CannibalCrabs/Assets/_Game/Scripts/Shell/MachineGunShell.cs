using UnityEngine;

public class MachineGunShell : Shell
{
    public Spike spikePrefab;
    public Dash dashPrefab;

    [FMODUnity.EventRef]
    public FMOD.Studio.EventInstance shootEventEmitter;

    public override void Awake()
    {
        base.Awake();
        shootEventEmitter = FMODUnity.RuntimeManager.CreateInstance("event:/SndFx/spiker_basic");
    }

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
