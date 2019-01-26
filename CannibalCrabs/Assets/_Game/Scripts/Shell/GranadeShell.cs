using UnityEngine;

public class GranadeShell : Shell
{
    public Granade granadePrefab;
    public Vector2 target;

    public override void Shoot(Vector2 direction)
    {
        if (canShoot)
        {
            target = direction * 2.5f;
            var granade = Instantiate(granadePrefab);
            granade.transform.position = transform.position;
            granade.Go(owner, target);
            timeLastShot = Time.time;
        }
    }
}
