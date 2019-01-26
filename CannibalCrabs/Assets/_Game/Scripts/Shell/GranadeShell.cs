using UnityEngine;

public class GranadeShell : Shell
{
    public Granade granadePrefab;

    public override void Shoot(Vector2 direction)
    {
        if (canShoot)
        {
            var granade = Instantiate(granadePrefab);
            granade.transform.position = transform.position;
            granade.Go(owner, direction);
            timeLastShot = Time.time;
        }
    }
}
