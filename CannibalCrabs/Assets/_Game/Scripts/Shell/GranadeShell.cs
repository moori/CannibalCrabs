using UnityEngine;

public class GranadeShell : Shell
{
    public Granade granadePrefab;
    public ShelterBomb shelterBombPrefab;

    [FMODUnity.EventRef]
    public FMOD.Studio.EventInstance shootEventEmitter;

    public override void Awake()
    {
        base.Awake();
        shootEventEmitter = FMODUnity.RuntimeManager.CreateInstance("event:/SndFx/bomber_basic_fall");
    }

    public override void Shoot(Vector2 direction)
    {
        if (!canShoot)
            return;

        Granade granade = Instantiate(granadePrefab);
        granade.transform.position = transform.position;
        granade.Go(owner, direction);
        timeLastShot = Time.time;
        shootEventEmitter.start();
    }

    public override void Sacrifice(Vector2 direction)
    {
        base.Sacrifice(direction);
        ShelterBomb shelterBomb = Instantiate(shelterBombPrefab);
        shelterBomb.Activate(owner, transform.position);
    }
}
