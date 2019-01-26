using UnityEngine;

public class GranadeShell : Shell
{
    public Granade granadePrefab;
    public Vector2 target;

    public override void Shoot(Vector2 direction)
    {
        if (canShoot)
        {
            var granade = Instantiate(granadePrefab);
            granade.transform.position = transform.position;
            granade.Go(target);
            timeLastShot = Time.time;
        }
    }
}
